using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Generators;
using System.Collections;
using System.Compilers.Shaders.Info;
using System.Maths;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Compilers.Shaders;

namespace System.Rendering.Effects.Shaders
{
    public abstract class ShadingPipeline<TEffect>
    {
        private IRenderDevice _render;
        
        private TEffect _currentEffect;
        private PipelineSettings _currentSettings;
        private GlobalBindings[] _currentBindings;
        private Dictionary<ShaderMember, string>[] _currentNames;
        private ShaderStage _currentStage;

        private Dictionary<PipelineSettings, TEffect> _effectsCache;

        private Dictionary<string, ISampler> _samplers = new Dictionary<string, ISampler>();

        public IRenderDevice Render { get { return _render; } }

        public TEffect Effect { get { return _currentEffect; } }

        protected ShadingPipeline(IRenderDevice render, Dictionary<ShaderStage, ShaderCodeGenerator> stages)
        {
            _render = render;
            _effectsCache = new Dictionary<PipelineSettings, TEffect>();
            _currentSettings = new PipelineSettings(this, stages);
        }

        #region [ Apply Effect ]

        /// <summary>
        /// Updates values and apply the effect.
        /// </summary>
        public void UpdateAndApplyEffect(DataDescription vertexDescription)
        {
            #region Build Effect

            _currentSettings.SetVertexDescription(vertexDescription);

            List<CompiledStage> compiledStages = new List<CompiledStage>();

            GlobalBindings[] fullBindings = new GlobalBindings[(int)ShaderStage.Length];
            Dictionary<ShaderMember, string>[] fullNames = new Dictionary<ShaderMember, string>[(int)ShaderStage.Length];

            foreach (var stage in _currentSettings)
            {
                string mainName;
                string code = stage.GetShaderCodeFixed(out fullBindings[(int)stage.Stage], out fullNames[(int)stage.Stage], out mainName);
                compiledStages.Add(CompiledStage.From(stage.Stage, code, mainName));
            }

            if (!_effectsCache.ContainsKey(_currentSettings))
            {
                var ps = (PipelineSettings)_currentSettings.Clone();
                _effectsCache.Add(ps, BuildEffect(compiledStages));
            }

            _currentEffect = _effectsCache[_currentSettings];
            _currentBindings = fullBindings;
            _currentNames = fullNames;

            #endregion

            ApplyEffect();

            #region Updating Globals

            samplerIndex = 0;

            foreach (var stageSettings in _currentSettings)
            {
                var bindings = _currentBindings[(int)stageSettings.Stage];
                var names = _currentNames[(int)stageSettings.Stage];
                _currentStage = stageSettings.Stage;

                foreach (var field in bindings.Fields)
                {
                    var value = bindings.GetValue(field);

                    var fieldName = names[field];

                    SetGlobalValue(fieldName, field.Type, value, stageSettings.CodeGen);
                }
            }

            #endregion

            BeginEffect();
        }

        int samplerIndex = 0;

        protected void SetGlobalValue(string expression, ShaderType expressionType, object value, ShaderCodeGenerator gen)
        {
            Type valueType = value.GetType();

            if (value is ISampler)
            {
                _samplers[expression] = (ISampler)value;
                SetSamplerOnEffect(expression, samplerIndex, (ISampler)value);
                samplerIndex++;
                return;
            }

            if (expressionType.IsBuiltin)
            {
                SetValueOnEffect(expression, value);
                return;
            }

            if (expressionType.IsArray)
            {
                Array a = value as Array;
                int length = a.Length;
                for (int i = 0; i < length; i++)
                    SetGlobalValue(expression + "[" + i + "]", expressionType.ElementType, a.GetValue(i), gen);

                return;
            }

            /// Ugly, very ugly...
            var fields = new List<ShaderField>(expressionType.Members.OfType<ShaderField>());
            var netFields = new List<FieldInfo>(valueType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
            netFields.Sort((f1, f2) => (int)Marshal.OffsetOf(valueType, f1.Name) - (int)Marshal.OffsetOf(valueType, f2.Name));
            for (int i = 0; i < fields.Count; i++)
                SetGlobalValue(expression + "." + _currentNames[(int)_currentStage][fields[i]], fields[i].Type, netFields[i].GetValue(value), gen);
        }

        protected abstract void BeginEffect();

        /// <summary>
        /// Creates the effect for the current pipeline settings.
        /// </summary>
        /// <returns></returns>
        protected abstract TEffect BuildEffect(IEnumerable<CompiledStage> stages);

        /// <summary>
        /// Sets a value to a specific field.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        protected abstract void SetValueOnEffect(string fieldName, object value);
        /// <summary>
        /// Set a sampler to a specific index
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="index"></param>
        /// <param name="sampler"></param>
        protected abstract void SetSamplerOnEffect(string fieldName, int index, ISampler sampler);
        /// <summary>
        /// Clear the use of certain sampler.
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="index"></param>
        /// <param name="sampler"></param>
        protected abstract void ClearSamplerOnEffect(string fieldName, int index, ISampler sampler);
        /// <summary>
        /// Applies an effect.
        /// </summary>
        protected abstract void ApplyEffect();

        protected abstract void UnapplyEffect();

        public void ClearAndUnApplyEffect()
        {
            int c = 0;
            foreach (var samplerName in _samplers.Keys)
                ClearSamplerOnEffect(samplerName, c++, _samplers[samplerName]);
            _samplers.Clear();

            UnapplyEffect();
        }

        #endregion

        /// <summary>
        /// Sets the shader for a specific stage.
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="ast"></param>
        /// <param name="bindings"></param>
        public void SetShaderFor(ShaderStage stage, ShaderProgramAST ast, GlobalBindings bindings)
        {
            _currentSettings.SetShader(stage, ast, bindings);
        }

        class PipelineSettings : ICloneable, IEnumerable<PipelineSettings.PipelineStageSettings>
        {
            private ShadingPipeline<TEffect> _manager;
            private PipelineStageSettings[] _stagesSettings;

            private PipelineStageSettings PiorTo(ShaderStage stage)
            {
                int i = (int)stage - 1;
                while (i >= 0 && _stagesSettings[i] == null) i--;
                if (i >= 0)
                    return _stagesSettings[i];
                return null;
            }

            public IEnumerable<DataComponentDescription> VertexDescription { get; private set; }

            public PipelineSettings(ShadingPipeline<TEffect> manager, Dictionary<ShaderStage, ShaderCodeGenerator> supportedStages)
            {
                _manager = manager;
                _stagesSettings = new PipelineStageSettings[(int)ShaderStage.Length];
                foreach (var stage in supportedStages.Keys)
                    _stagesSettings[(int)stage] = new PipelineStageSettings(this, stage, supportedStages[stage]);

                if (_stagesSettings[0] == null)
                    throw new ArgumentException("Pipelines should support at least vertex shaders.");
            }

            public override bool Equals(object obj)
            {
                var other = (PipelineSettings)obj;

                if (!this.VertexDescription.SequenceEqual(other.VertexDescription))
                    return false;

                return _stagesSettings.SequenceEqual(other._stagesSettings);
            }

            public override int GetHashCode()
            {
                return this.Aggregate(1, (a, x) => a * x.GetHashCode() + 13);
            }

            public object Clone()
            {
                var clone = (PipelineSettings)MemberwiseClone();

                clone._stagesSettings = new PipelineStageSettings[(int)ShaderStage.Length];
                for (int i = 0; i < _stagesSettings.Length; i++)
                    if (_stagesSettings[i] != null)
                        clone._stagesSettings[i] = (PipelineStageSettings)_stagesSettings[i].Clone();
                return clone;
            }

            public IEnumerator<PipelineStageSettings> GetEnumerator()
            {
                foreach (var item in _stagesSettings)
                    if (item != null)
                        yield return item;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void SetShader(ShaderStage stage, ShaderProgramAST shader, GlobalBindings globals)
            {
                _stagesSettings[(int)stage].SetShader(shader, globals);
            }

            internal void SetVertexDescription(DataDescription vertexDescription)
            {
                this.VertexDescription = vertexDescription.Declaration;
            }

            public class PipelineStageSettings : ICloneable
            {
                private ShaderProgramAST _shader;
                private GlobalBindings _shaderBindings;

                private PipelineSettings _pipeline;
                private ShaderStage _stage;
                private ShaderCodeGenerator _codeGenerator;

                public ShaderCodeGenerator CodeGen { get { return _codeGenerator; } }
                public ShaderStage Stage { get { return _stage; } }

                public PipelineStageSettings(PipelineSettings pipeline, ShaderStage stage, ShaderCodeGenerator codeGenerator)
                {
                    this._pipeline = pipeline;
                    this._stage = stage;
                    this._codeGenerator = codeGenerator;
                }

                public override bool Equals(object obj)
                {
                    return obj is PipelineStageSettings && _shader.Equals(((PipelineStageSettings)obj)._shader);
                }

                public override int GetHashCode()
                {
                    return _shader.GetHashCode();
                }

                public object Clone()
                {
                    var clone = this.MemberwiseClone() as PipelineStageSettings;
                    return clone;
                }

                public void SetShader(ShaderProgramAST ast, GlobalBindings bindings)
                {
                    _shader = ast;
                    _shaderBindings = bindings;
                }

                public IEnumerable<Tuple<Semantic, ShaderType>> Input
                {
                    get
                    {
                        if (Stage == ShaderStage.Vertex)
                            foreach (var s in _pipeline.VertexDescription)
                            {
                                ShaderType type = null;
                                switch (s.Components){
                                    case 1: type = _shader.Builtins.Resolve(typeof(Vector1)); break;
                                    case 2: type = _shader.Builtins.Resolve(typeof(Vector2)); break;
                                    case 3: type = _shader.Builtins.Resolve(typeof(Vector3)); break;
                                    case 4: type = _shader.Builtins.Resolve(typeof(Vector4)); break;
                                }
                                yield return Tuple.Create (ShaderSemantics.Resolve (s.Semantic), type);
                            }
                        else
                        {
                            var prev = _pipeline.PiorTo(_stage);

                            foreach (var s in prev.Output.Where(s => !(s.Item1 is ProjectedSemantic)))
                                yield return s;
                        }
                    }
                }

                class ByFirstItemEqCmp<T1, T2> : IEqualityComparer<Tuple<T1, T2>>
                {

                    public bool Equals(Tuple<T1, T2> x, Tuple<T1, T2> y)
                    {
                        return x.Item1.Equals(y.Item1);
                    }

                    public int GetHashCode(Tuple<T1, T2> obj)
                    {
                        return obj.Item1.GetHashCode();
                    }
                }

                public IEnumerable<Tuple<Semantic, ShaderType>> Output
                {
                    get
                    {
                        if (_stage == ShaderStage.Pixel)
                            return new Tuple<Semantic, ShaderType>[] { 
                                Tuple.Create<Semantic, ShaderType>(ShaderSemantics.Color(0), _shader.Builtins.Resolve(typeof(Vector4))) 
                            };

                        var prev = _pipeline.PiorTo(_stage);

                        if (prev == null)
                            return _shader.OutputDeclaration.Union(Input, new ByFirstItemEqCmp<Semantic, ShaderType>());
                        else
                            return _shader.OutputDeclaration.Union(prev.Output, new ByFirstItemEqCmp<Semantic, ShaderType>());
                    }
                }


                Dictionary<ShaderProgramAST, Tuple<string, Dictionary<ShaderMember, string>>> _codeFixedCache = new Dictionary<ShaderProgramAST, Tuple<string, Dictionary<ShaderMember, string>>>();

                public string GetShaderCodeFixed(out GlobalBindings bindings, out Dictionary<ShaderMember, string> names, out string mainName)
                {
                    ShaderProgramAST fixedShader = FixShader(out bindings);

                    if (!_codeFixedCache.ContainsKey(fixedShader))
                    {
                        var code = string.Join("\n", _codeGenerator.GenerateCode(fixedShader, out names));
                        _codeFixedCache.Add(fixedShader, Tuple.Create(code, names));
                    }

                    names = _codeFixedCache[fixedShader].Item2;

                    mainName = names[fixedShader.Main];

                    return _codeFixedCache[fixedShader].Item1;
                }
                
                #region Fixing Shaders with Cache
                private ShaderType DeclareFixedInput(ShaderProgramAST program)
                {
                    var typeDec = program.DeclareNewStruct("INPUT", 0);

                    int i = 0;
                    foreach (var s in this.Input)
                        typeDec.DeclareNewField (s.Item2, "field"+(i++), s.Item1);

                    return typeDec.Type;
                }
                private ShaderType DeclareFixedOutput(ShaderProgramAST program)
                {
                    var typeDec = program.DeclareNewStruct("OUTPUT", 0);

                    int i = 0;
                    foreach (var s in this.Output)
                        typeDec.DeclareNewField(s.Item2, "field" + (i++), s.Item1);

                    return typeDec.Type;
                }
                Dictionary<ShaderProgramAST, Tuple<ShaderProgramAST, Dictionary<ShaderMember, ShaderMember>>> _fixedProgramsCache = new Dictionary<ShaderProgramAST, Tuple<ShaderProgramAST, Dictionary<ShaderMember, ShaderMember>>>();
                
                private ShaderProgramAST FixShader(out GlobalBindings bindings)
                {
                    if (!_fixedProgramsCache.ContainsKey(_shader))
                    {
                        Builtins builtins = _shader.Builtins;

                        ShaderProgramAST program = new ShaderProgramAST(builtins);

                        Dictionary<ShaderMember, ShaderMember> maps, compilations;
                        program.Include(_shader, out maps, out compilations);


                        program.PushMode(ShaderProgramAST.AddingMode.Append);

                        var oldMain = compilations[_shader.Main] as ShaderMethod;

                        var input = DeclareFixedInput(program);
                        var output = DeclareFixedOutput(program);

                        var newMain = program.DeclareNewMethod("FixedMain", 0);
                        program.SetReturnType(newMain, output);
                        var In = newMain.DeclareNewParameter(input, "In", null, Compilers.Shaders.Info.ParameterModifier.In);

                        var builder = newMain.GetMethodBuilder();

                        /*
                        OUTPUT FixedMain (INPUT In)
                        {
                          OLDINPUT input = new OLDINPUT ();
                          // foreach coincident field semantics
                          input.field = In.field;
                          // foreach non-coincident field semantics
                          input.field = default
                      
                          OLDOUTPUT output = OLDMAIN (input);
                     
                          OUTPUT Out = new OUTPUT ();
                          // foreach coincident field semantics
                          Out.field = output.field;
                          // foreach non-coincident field semantics
                          Out.field = In.field;
                     
                          return Out;
                        }
                        */

                        var OLDINPUT = oldMain.Parameters.First().ParameterType;
                        var OLDOUTPUT = oldMain.ReturnType;

                        var inputLocal = builder.DeclareLocal(OLDINPUT, "input");
                        builder.AddInitialization(inputLocal);

                        foreach (var field in OLDINPUT.Members.OfType<ShaderField>())
                        {
                            ShaderField InField = input.Members.OfType<ShaderField>().FirstOrDefault(f => f.Semantic.Equals(field.Semantic));
                            if (InField != null)
                                builder.AddAdapt(builder.Field(builder.Local(inputLocal), field), builder.Field(builder.Parameter(In), InField));
                            else
                            {
                                var defaultValue = ((DataSemantic)field.Semantic).GetDefaultValue();
                                builder.AddAdapt(builder.Field(builder.Local(inputLocal), field), builder.Constant(builtins.Resolve(defaultValue.GetType()), defaultValue));
                            }
                        }

                        var outputLocal = builder.DeclareLocal(OLDOUTPUT, "output");
                        builder.AddAssignament(outputLocal, builder.Call(null, oldMain, builder.Local(inputLocal)));

                        var Out = builder.DeclareLocal(output, "Out");
                        builder.AddInitialization(Out);

                        foreach (var field in output.Members.OfType<ShaderField>())
                        {
                            ShaderField outputField = OLDOUTPUT.Members.OfType<ShaderField>().FirstOrDefault(f => f.Semantic.Equals(field.Semantic));

                            if (outputField != null)
                                builder.AddAdapt(builder.Field(builder.Local(Out), field), builder.Field(builder.Local(outputLocal), outputField));
                            else
                            {
                                var InField = input.Members.OfType<ShaderField>().FirstOrDefault(f => f.Semantic.Equals(field.Semantic));
                                if (InField != null)
                                    builder.AddAdapt(builder.Field(builder.Local(Out), field), builder.Field(builder.Parameter(In), InField));
                                else
                                {
                                }
                            }
                        }

                        builder.AddReturn(builder.Local(Out));

                        program.PopMode();

                        program.Main = newMain.Method;

                        _fixedProgramsCache.Add(_shader, Tuple.Create(program, maps));
                    }

                    var mappings = _fixedProgramsCache[_shader].Item2;

                    bindings = _shaderBindings.Map(mappings);

                    return _fixedProgramsCache[_shader].Item1;
                }
                #endregion
            }

        }

        class SequenceKey<T>
        {
            IEnumerable<T> s;
            public SequenceKey(IEnumerable<T> s)
            {
                this.s = s;
            }

            public override bool Equals(object obj)
            {
                return this.s.SequenceEqual(((SequenceKey<T>)obj).s);
            }

            public override int GetHashCode()
            {
                return s.Aggregate(0, (a, i) => a + i.GetHashCode() * 3 + 1);
            }
        }

    }
    public struct CompiledStage 
    {
        public ShaderStage Stage {get; private set;}
        public string Code {get; private set;}
        public string Main {get; private set;}

        public static CompiledStage From(ShaderStage stage, string code, string main)
        {
            return new CompiledStage { Stage = stage, Code = code, Main = main };
        }
    }
}
