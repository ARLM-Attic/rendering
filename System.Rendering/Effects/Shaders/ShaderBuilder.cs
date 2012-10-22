using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Linq.Expressions;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders;
using System.Compilers.Shaders.Info;

namespace System.Rendering.Effects.Shaders
{
    public static class ShaderBuilder
    {
        private static Dictionary<int, Type> registeredAgents = new Dictionary<int, Type>();

        static ShaderBuilder()
        {
            Register<ShaderSource.ShaderSourceDelegate, DelegateShaderBuilderAgent>();
            Register<ShaderSource.ShaderSourceLambdaExpression, LambdaExpressionShaderBuilderAgent>();
            Register<HLSLCodeShaderSource, HLSLShaderBuilderAgent>();
        }

        public static void Register<T, A>()
            where T : ShaderSource
            where A : ShaderBuilderAgent
        {
            int token = typeof(T).MetadataToken;
            if (!registeredAgents.ContainsKey(token))
                registeredAgents.Add(token, typeof(A));
            else
                registeredAgents[token] = typeof(A);
        }

        public static ShaderProgramAST Build(ShaderSource source, Builtins builtins, out GlobalBindings bindings)
        {
            int token = source.GetType().MetadataToken;
            if (!registeredAgents.ContainsKey(token))
                throw new NotSupportedException("There is not agent to build the specific shader source");

            ShaderBuilderAgent a = Activator.CreateInstance(registeredAgents[token]) as ShaderBuilderAgent;

            if (!a.CanBuild(source))
                throw new NotSupportedException();

            a.Build(source, builtins);

            bindings = new TargetGlobalBindings(a.Target, a.Program.Constants);

            var program = a.Program;

            while (program.IsAbstract)
            {
                var method = program.Dynamics.Methods.First();
                var del = program.Dynamics.GetMethod(a.Target, method);

                GlobalBindings implementationBindings;
                var overrideProgram = Build(del, builtins, out implementationBindings);

                Dictionary<ShaderMember, ShaderMember> maps;
                program = ShaderJoiner.ImplementMethod(program, method, overrideProgram,
                    out maps);

                bindings = GlobalBindings.Concat(bindings.Map(maps), implementationBindings.Map(maps));
            }

            program.SortMembers();

            return program;
        }

        /// <summary>
        /// Gets the shader tree for the specific lambda expression.
        /// </summary>
        /// <param name="main">Main function of the shader expressed by a lambda expression</param>
        /// <returns>ShaderAST object with the tree.</returns>
        public static ShaderProgramAST Build<In, Out>(Expression<Func<In, Out>> main, Builtins builtins, out GlobalBindings globals)
        {
            return Build(main.Compile(), builtins, out globals);
        }
    }

    public abstract class GlobalBindings
    {
        public abstract IEnumerable<ShaderField> Fields { get; }

        public abstract object GetValue(ShaderField field);

        public abstract bool Contains(ShaderField field);

        public static GlobalBindings Concat(GlobalBindings g1, GlobalBindings g2)
        {
            return new ConcatGlobalBindings { CurrentGlobalBindings = g1, PreviousGlobalBindings = g2 };
        }

        public GlobalBindings Map(Dictionary<ShaderMember, ShaderMember> maps)
        {
            return new MappedGlobalBindings(this, maps);
        }

        class MappedGlobalBindings : GlobalBindings
        {
            public GlobalBindings OriginalGlobals { get; private set; }

            public override IEnumerable<ShaderField> Fields
            {
                get
                {
                    HashSet<ShaderField> originalFields = new HashSet<ShaderField>(OriginalGlobals.Fields);
                    foreach (var f in Maps.Keys.OfType<ShaderField>())
                        if (originalFields.Contains((ShaderField)Maps[f]))
                            yield return f;
                }
            }

            public Dictionary<ShaderMember, ShaderMember> Maps { get; private set; }

            public MappedGlobalBindings(GlobalBindings original, Dictionary<ShaderMember, ShaderMember> maps)
            {
                this.OriginalGlobals = original;
                this.Maps = maps;
            }

            public override bool Contains(ShaderField field)
            {
                return Maps.ContainsKey(field) && OriginalGlobals.Contains((ShaderField)Maps[field]);
            }

            public override object GetValue(ShaderField field)
            {
                if (!Maps.ContainsKey(field))
                    throw new ArgumentException();

                return OriginalGlobals.GetValue((ShaderField)Maps[field]);
            }
        }

        class ConcatGlobalBindings : GlobalBindings
        {
            public GlobalBindings PreviousGlobalBindings { get; internal set; }

            public GlobalBindings CurrentGlobalBindings { get; internal set; }

            public override IEnumerable<ShaderField> Fields
            {
                get
                {
                    foreach (var f in PreviousGlobalBindings.Fields)
                        yield return f;
                    foreach (var f in CurrentGlobalBindings.Fields)
                        yield return f;
                }
            }

            public override object GetValue(ShaderField field)
            {
                if (PreviousGlobalBindings.Contains(field))
                    return PreviousGlobalBindings.GetValue(field);
                return CurrentGlobalBindings.GetValue(field);
            }

            public override bool Contains(ShaderField field)
            {
                return PreviousGlobalBindings.Contains(field) || CurrentGlobalBindings.Contains(field);
            }
        }
    }

    class TargetGlobalBindings : GlobalBindings
    {
        public object Target { get; private set; }
        public IConstantProxy ConstantProxy { get; private set; }

        public override IEnumerable<ShaderField> Fields
        {
            get { return ConstantProxy.Fields; }
        }

        internal TargetGlobalBindings(object target, IConstantProxy programConstantsProxy)
        {
            this.Target = target;
            this.ConstantProxy = programConstantsProxy;
        }

        public override bool Contains(ShaderField field)
        {
            return ConstantProxy.Fields.Contains(field);
        }

        public override object GetValue(ShaderField field)
        {
            return ConstantProxy.GetValueFor(Target, field);
        }
    }

    public abstract class ShaderBuilderAgent
    {
        public abstract bool CanBuild(ShaderSource shaderSource);

        public abstract void Build (ShaderSource shaderSource, Builtins builtins);

        public ShaderProgramAST Program {get; protected set;}

        public object Target { get; protected set; }
    }

    public class HLSLCodeShaderSource : ShaderSource
    {
    }

    public class HLSLShaderBuilderAgent : ShaderBuilderAgent
    {
        public override bool CanBuild(ShaderSource shaderSource)
        {
            return shaderSource is HLSLCodeShaderSource;
        }

        public override void Build(ShaderSource shaderSource, Builtins builtins)
        {
         
        }
    }

    public abstract class ShaderSource
    {
        public static ShaderSource From(Delegate function)
        {
            return new ShaderSourceDelegate { Delegate = function };
        }

        public static ShaderSource From<In, Out>(Expression<Func<In, Out>> function)
            where In : struct
            where Out : struct
        {
            return new ShaderSourceLambdaExpression { Function = function };
        }

        public static implicit operator ShaderSource(Delegate main)
        {
            return ShaderSource.From(main);
        }

        internal class ShaderSourceDelegate : ShaderSource
        {
            public Delegate Delegate { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ShaderSourceDelegate && object.Equals(((ShaderSourceDelegate)obj).Delegate.Method, this.Delegate.Method) && object.Equals(((ShaderSourceDelegate)obj).Delegate.Target, this.Delegate.Target);
            }

            public override int GetHashCode()
            {
                return this.Delegate != null ? this.Delegate.GetHashCode() : 0;
            }
        }

        internal class ShaderSourceLambdaExpression : ShaderSource
        {
            public Expression Function { get; set; }

            public override bool Equals(object obj)
            {
                return obj is ShaderSourceLambdaExpression && object.Equals(((ShaderSourceLambdaExpression)obj).Function, this.Function);
            }

            public override int GetHashCode()
            {
                return Function != null ? Function.GetHashCode() : 0;
            }
        }
    }

    public sealed class BuildingErrorException : Exception
    {
        public IEnumerable<string> Errors { get; set; }
    }

    public enum CompileAsShaderOfType
    {
        Vertex,
        Geometry,
        Pixel
    }
}
