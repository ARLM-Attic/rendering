using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders;
using System.Compilers.Shaders.Info;
using System.Compilers.Shaders.ShaderAST;
using System.Rendering.Effects.Shaders;
using System.Maths;

namespace System.Compilers.Generators
{
    public class GLSL400CodeGenerator : ShaderCodeGenerator
    {
        public ShaderStage ShaderStage { get; private set; }
        
        static GLSL400CodeGenerator()
        {
            resolveVI.Add(ShaderSemantics.Position(0), "gl_Vertex");
            resolveVI.Add(ShaderSemantics.Normal(0), "gl_Normal");
            resolveVI.Add(ShaderSemantics.Color(0), "gl_Color");
            resolveVI.Add(ShaderSemantics.Color(1), "gl_SecondaryColor");
            resolveVI.Add(ShaderSemantics.Fog(), "gl_Fog");
            for (int i = 0; i < 16; i++)
                resolveVI.Add(ShaderSemantics.Coordinates(i), "gl_MultiTexCoord" + i);

            resolveVO.Add(ShaderSemantics.Projected(), "gl_Position");
            resolveVO.Add(ShaderSemantics.Color(0), "gl_FrontColor");
            resolveVO.Add(ShaderSemantics.Color(1), "gl_FrontSecondaryColor");
            for (int i = 0; i < 16; i++)
                resolveVO.Add(ShaderSemantics.Coordinates(i), "gl_TexCoord[" + i + "]");

            resolvePI.Add(ShaderSemantics.Color(0), "gl_Color");
            resolvePI.Add(ShaderSemantics.Color(1), "gl_SecondaryColor");
            for (int i = 0; i < 16; i++)
                resolvePI.Add(new CoordinatesSemantic() { Index = i }, "gl_TexCoord[" + i + "]");

            resolvePO.Add(ShaderSemantics.Color(0), "gl_FragData[0]");
            resolvePO.Add(ShaderSemantics.Color(1), "gl_FragData[1]");
        }

        public GLSL400CodeGenerator(ShaderStage stage, Builtins builtins)
        {
            this._Builtins = builtins;
            this.ShaderStage = stage;
        }

        Builtins _Builtins;
        public override Builtins Builtins { get { return _Builtins; } }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderTypeDeclarationAST ast)
        {
            if (ast.IsStruct)
            {
                yield return "struct " + ResolveNameFor(ast.Member);
                yield return "{";
                foreach (var m in ast.Members)
                    yield return Indent(GetCode(m));
                yield return "};";
            }
            else
                throw new NotSupportedException();
        }

        string Fix(string name)
        {
            string originalName = name.Replace("<", "").Replace(">", "").Replace(" ", "").Replace("@", "").Replace(".", "");
            name = originalName;
            int index = 0;
            while (Builtins.Keywords.Contains(name) || members.Contains(name))
                name = originalName + (index++);
            return name;
        }

        static Dictionary<Semantic, string> resolveVI = new Dictionary<Semantic, string>();
        static Dictionary<Semantic, string> resolveVO = new Dictionary<Semantic, string>();
        static Dictionary<Semantic, string> resolvePI = new Dictionary<Semantic, string>();
        static Dictionary<Semantic, string> resolvePO = new Dictionary<Semantic, string>();

        int fragDataOut = 2;

        private void ResolveSemantic(Semantic s, bool isIn, out string name, out bool isAttribute, out bool isGlobal, out bool isVarying)
        {
            isAttribute = false;
            isGlobal = false;
            isVarying = false;
            name = "noname";

            if (isIn)
            {
                switch (ShaderStage)
                {
                    case ShaderStage.Vertex:
                        if (resolveVI.ContainsKey(s))
                        {
                            name = resolveVI[s];
                            if (name.StartsWith("attribute_"))
                                isAttribute = true;
                            else
                                isGlobal = true;
                        }
                        else
                        {
                            name = "attribute_" + s.ToString();
                            isAttribute = true;
                            resolveVI[s] = name;
                        }
                        break;
                    case ShaderStage.Pixel:
                        if (resolvePI.ContainsKey(s))
                        {
                            name = resolvePI[s];
                            if (name.StartsWith("varying_"))
                                isVarying = true;
                            else
                                isGlobal = true;
                        }
                        else
                        {
                            name = "varying_" + s.ToString();
                            isVarying = true;
                            resolvePI[s] = name;
                        }
                        break;
                }
            }
            else
            {
                switch (ShaderStage)
                {
                    case ShaderStage.Vertex:
                        if (resolveVO.ContainsKey(s))
                        {
                            name = resolveVO[s];
                            if (name.StartsWith("varying_"))
                                isVarying = true;
                            else
                                isGlobal = true;
                        }
                        else
                        {
                            name = "varying_" + s.ToString();
                            isVarying = true;
                            resolveVO[s] = name;
                        }
                        break;
                    case ShaderStage.Pixel:
                        if (!resolvePO.ContainsKey(s))
                        {
                            resolvePO[s] = "gl_FragData[" + fragDataOut + "]";
                            fragDataOut++;
                        }
                        name = resolvePO[s];
                        isGlobal = true;
                        break;
                }
            }
        }

        Dictionary<ShaderMember, string> fixedNames = new Dictionary<ShaderMember, string>();
        HashSet<string> members = new HashSet<string>();

        Dictionary<ShaderMember, string> currentNames;

        private string ResolveNameFor(ShaderMember member)
        {
            if (member.Name.StartsWith("@"))
                return member.Name.Substring(1);

            if (Builtins.Contains(member))
                return Builtins.GetName(member);

            if (!fixedNames.ContainsKey(member))
            {
                string fix = Fix(member.Name);
                fixedNames.Add(member, fix);
                members.Add(fix);

                currentNames.Add(member, fix);
            }
            return fixedNames[member];
        }

				Dictionary<string, string> fixedLocals = new Dictionary<string, string>();
				private string ResolveNameForLocal(string name)
				{
					if (!fixedLocals.ContainsKey(name))
						fixedLocals.Add(name, Fix(name));

					return fixedLocals[name];
				}

        string GetParameterDeclaration(ShaderParameter parameterDec)
        {
            string mod = parameterDec.Modifier == ParameterModifier.In ? "in " : parameterDec.Modifier == ParameterModifier.Out ? "out " :
                parameterDec.Modifier == ParameterModifier.InOut ? "inout " : "";
            return mod + ResolveNameFor(parameterDec.ParameterType) + " " + parameterDec.Name;
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodDeclarationAST ast)
        {
            string args = string.Join(", ", ast.Parameters.Select(p => GetParameterDeclaration(p)).ToArray());
            yield return ResolveNameFor(ast.ReturnType) + " " + ResolveNameFor(ast.Method) + "(" + args + ")";
            yield return Join(GetCode(ast.Body));
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstructorDeclarationAST ast)
        {
            string args = string.Join(", ", ast.Parameters.Select(p => GetParameterDeclaration(p)).ToArray());
            yield return ast.DeclaringType.Name + "(" + args + ")";
            yield return Join(GetCode(ast.Body))+";";
        }

        string GetTypeOfField(ShaderField field)
        {
            if (currentAttributes.Contains(field))
                return "attribute";
            if (currentVaryings.Contains(field))
                return "varying";
            return "uniform";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderFieldDeclarationAST ast)
        {
            if (currentGlobals.Contains(ast.Field)) /// Builtins fields in ast should not be coded.
                yield break;

            var fieldName = ResolveNameFor(ast.Field);
            var fieldPrefix = ast.DeclaringType == null ? GetTypeOfField(ast.Field)+" " : "";

            if (ast.FieldType.IsArray)
            {
                if (ast.FieldType.Ranks == null)
                    yield return fieldPrefix + ResolveNameFor(ast.FieldType.ElementType) + "[] " + fieldName + ";";
                else
                    yield return fieldPrefix + ResolveNameFor(ast.FieldType.ElementType) + " " + fieldName + "[" + ast.FieldType.Ranks[0] + "];";
            }
            else
            {
                yield return fieldPrefix + ResolveNameFor(ast.FieldType) + " " + fieldName + ";";
            }
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodBodyAST ast)
        {
            yield return "{";
            foreach (var inst in ast.Statements)
                yield return Indent(GetCode(inst));
            yield return "}";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstantExpressionAST ast)
        {
            if (ast.Value == null)
                yield return "null";
            else
                if (ast.Value is bool)
                    yield return (bool)ast.Value ? "true" : "false";
                else
                    if (ast.Value is float)
                        yield return ((float)ast.Value).ToString("F6");
                    else
                        if (ast.Value is double)
                            yield return ((double)ast.Value).ToString("F6");
                        else
                            yield return ast.Value.ToString();
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderLocalInvokeAST ast)
        {
            yield return ResolveNameForLocal(ast.Local.Name);
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderParameterInvokeAST ast)
        {
            yield return ast.Parameter.Name;
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderFieldInvokeAST ast)
        {
            if (ast.LeftSide == null)
                yield return ResolveNameFor(ast.Field);
            else
                yield return GetCode(ast.LeftSide) + "." + ResolveNameFor(ast.Field);
        }
    
        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodInvokeAST ast)
        {
            string args = string.Join(", ", ast.Arguments.Select(a => GetCode(a)).ToArray());
            if (ast.LeftSide == null)
                yield return ResolveNameFor(ast.Method) + "(" + args + ")";
            else
                yield return ResolveNameFor(ast.Method) + "(" + GetCode(ast.LeftSide) + (args.Length > 0 ? "," : "") + args + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstructorInvokeAST ast)
        {
            string args = string.Join (", ", ast.Arguments.Select(a=>GetCode(a)).ToArray());
            yield return ResolveNameFor(ast.Type) + "(" + args + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderOperationAST ast)
        {
            if (ast.Operator == Operators.Indexer)
            {
                string arrayExpression = GetCode(ast.Operands[0]);
                if (ast.Operands[0] is ShaderOperationAST)
                    arrayExpression = "(" + arrayExpression + ")";
                yield return arrayExpression + "[" + GetCode(ast.Operands[1]) + "]";
                yield break;
            }
            switch (ast.Operands.Length)
            {
                case 1:
                    yield return ast.Operator.Token() +"("+ GetCode(ast.Operands[0])+")";
                    yield break;
                case 2:
                    yield return "("+GetCode(ast.Operands[0])+")" + ast.Operator.Token() + "("+GetCode(ast.Operands[1])+")";
                    yield break;
                case 3:
                    yield return "(" + GetCode(ast.Operands[0]) + "?" + GetCode(ast.Operands[1]) + ":" + GetCode(ast.Operands[2]) + ")";
                    yield break;
            }
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConversionAST ast)
        {
            yield return ResolveNameFor(ast.Target) + "(" + GetCode(ast.Expression) + ")";

        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderExpressionStatementAST ast)
        {
            yield return GetCode(ast.Expression) + ";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderLocalDeclarationAST ast)
        {
            yield return ResolveNameFor(ast.Type) + " " + ResolveNameForLocal(ast.Local.Name) + ";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConditionalStatement ast)
        {
            yield return "if (" + GetCode(ast.Conditional) + ")";
            yield return Join(GetCode(ast.TrueBlock));
            if (ast.FalseBlock != null)
            {
                yield return "else";
                yield return Join(GetCode(ast.FalseBlock));
            }
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderWhileStatementAST ast)
        {
            yield return "while (" + GetCode(ast.Conditional) + ")";
            yield return Join(GetCode(ast.Body));
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderDoWhileStatementAST ast)
        {
            yield return "do {";
            yield return Join(GetCode(ast.Body));
            yield return "} while (" + GetCode(ast.Conditional) + ");";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderForStatementAST ast)
        {
            yield return "for (" + GetCode(ast.Initialization) + GetCode(ast.Conditional) + ";" + GetCode(ast.Increment).TrimEnd(';') + ")";
            yield return Join(GetCode(ast.Body));
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderReturnStatementAST ast)
        {
            if (ast.Expression == null)
                yield return "return;";
            else
                yield return "return " + GetCode(ast.Expression) + ";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderAssignamentAST ast)
        {
            yield return GetCode(ast.LeftValue) + " = " + GetCode(ast.RightValue);
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderBlockStatementAST ast)
        {
            yield return "{";
            foreach (var i in ast.Statements)
                yield return Indent(GetCode(i));
            yield return "}";
        }

        string GetInitializationCodeFor(ShaderType type)
        {
            if (type.Equals(Builtins.Int)) return "0";
            if (type.Equals(Builtins.Float)) return "0.0";
            if (type.Equals(Builtins.Boolean)) return "false";
            if (type.Equals(Builtins.Byte)) return "0";

            string argumentList = string.Join(", ", type.Members.OfType<ShaderField>().Select(f => GetInitializationCodeFor(f.Type)).ToArray());
            return ResolveNameFor(type) + "(" + argumentList + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderInitializationInvokeAST ast)
        {
            ShaderType type = ast.TargetType;
            yield return GetInitializationCodeFor(type);
        }

        Dictionary<ShaderProgramAST, FixedProgramData> fixedPrograms = new Dictionary<ShaderProgramAST, FixedProgramData>();

        class FixedProgramData
        {
            public ShaderProgramAST Program;
            public HashSet<ShaderField> Varyings;
            public HashSet<ShaderField> Attributes;
            public HashSet<ShaderField> Globals;

            public Dictionary<ShaderMember, ShaderMember> Compilations;
        }

        public string Version { get; set; }

        private HashSet<ShaderField> currentVaryings, currentAttributes, currentGlobals;

        private Dictionary<ShaderMember, ShaderMember> currentCompilations;

        public Dictionary<ShaderMember, ShaderMember> Compilations { get { return currentCompilations; } }

        public override string GenerateCode(System.Compilers.Shaders.ShaderAST.ShaderProgramAST ast, out Dictionary<ShaderMember, string> names)
        {
            currentNames = new Dictionary<ShaderMember,string> ();

            if (!fixedPrograms.ContainsKey(ast))
                fixedPrograms.Add(ast, FixProgram(ast));

            var data = fixedPrograms[ast];

            currentVaryings = data.Varyings;
            currentGlobals = data.Globals;
            currentAttributes = data.Attributes;
            currentCompilations = data.Compilations;

            var fullCode = Version + "\n" + string.Join("\n", data.Program.Members.Select(m => GetCode(m)));

            names = new Dictionary<ShaderMember, string>();
            foreach (var m in currentNames.Keys)
                if (currentCompilations.ContainsKey(m))
                    names.Add(currentCompilations[m], currentNames[m]);

            return fullCode;
        }

        private FixedProgramData FixProgram(ShaderProgramAST ast)
        {
            ShaderProgramAST program = new ShaderProgramAST(ast.Builtins);

            HashSet<ShaderField> attributes = new HashSet<ShaderField>();
            HashSet<ShaderField> varyings = new HashSet<ShaderField>();
            HashSet<ShaderField> globals = new HashSet<ShaderField>();

            Dictionary<ShaderMember, ShaderMember> maps, compilations;
            program.Include(ast, out maps, out compilations);

            var mainFunction = compilations[ast.Main] as ShaderMethod;

            program.PushMode(ShaderProgramAST.AddingMode.Append);

            var main = program.DeclareNewMethod("@main", 0);

            program.PopMode();

            program.SetReturnType(main, Builtins.Void);

            var builder = main.GetMethodBuilder();

            var InLocal = builder.DeclareLocal(mainFunction.Parameters.First().ParameterType, "In");

            builder.AddInitialization(InLocal);

            foreach (var field in mainFunction.Parameters.First().ParameterType.Members.OfType<ShaderField>())
            {
                string name;
                bool isAttribute, isGlobal, isVarying;
                ResolveSemantic(field.Semantic, true, out name, out isAttribute, out isGlobal, out isVarying);
                ShaderType type = isVarying || isAttribute ? field.Type : Builtins.Resolve(typeof(Vector4));

                var fieldDec = program.DeclareNewField("@"+name, type, null);

                if (isAttribute)
                    attributes.Add(fieldDec.Field);
                if (isGlobal)
                    globals.Add(fieldDec.Field);
                if (isVarying)
                    varyings.Add(fieldDec.Field);

                builder.AddAdapt(builder.Field(builder.Local(InLocal), field), builder.Field(null, fieldDec.Field));
            }

            var OutLocal = builder.DeclareLocal(mainFunction.ReturnType, "Out");

            builder.AddAssignament(OutLocal, builder.Call(null, mainFunction, builder.Local(InLocal)));

            foreach (var field in OutLocal.Type.Members.OfType<ShaderField>())
            {
                string name;
                bool isAttribute, isGlobal, isVarying;
                ResolveSemantic(field.Semantic, false, out name, out isAttribute, out isGlobal, out isVarying);
                ShaderType type = isVarying || isAttribute ? field.Type : Builtins.Resolve(typeof(Vector4));

                var fieldDec = program.DeclareNewField("@" + name, type, null);

                if (isAttribute)
                    attributes.Add(fieldDec.Field);
                if (isGlobal)
                    globals.Add(fieldDec.Field);
                if (isVarying)
                    varyings.Add(fieldDec.Field);

                builder.AddAdapt(builder.Field(null, fieldDec.Field), builder.Field(builder.Local(OutLocal), field));
            }


            program.Main = main.Method;

            return new FixedProgramData { Program = program, Attributes = attributes, Globals = globals, Varyings = varyings, Compilations = maps };
        }
    }
}
