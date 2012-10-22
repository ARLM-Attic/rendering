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
    public class GLSL150CodeGenerator : ShaderCodeGenerator
    {
        public GLSL150CodeGenerator(ShaderStage stage, Builtins builtins)
        {
            this.ShaderStage = stage;
            this._Builtins = builtins;

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

        Builtins _Builtins;
        public override Builtins Builtins { get { return _Builtins; } }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderTypeDeclarationAST ast)
        {
            if (ast.IsStruct)
            {
                yield return "struct " + GetNameFor(ast.Member);
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
            string originalName = name.Replace("<", "").Replace(">", "").Replace(" ", "").Replace("_", "").Replace("@", "").Replace(".", "");
            name = originalName;
            int index = 0;
            while (Builtins.Keywords.Contains(name) || members.Contains(name))
                name = originalName + (index++);
            return name;
        }

        static Dictionary<ShaderMember, string> fixedNames = new Dictionary<ShaderMember, string>();
        static HashSet<string> members = new HashSet<string>();

        public override string GetNameFor(ShaderMember member)
        {
            if (Builtins.Contains(member))
                return Builtins.GetName(member);
            if (!fixedNames.ContainsKey(member))
            {
                string fix = Fix(member.Name);
                fixedNames.Add(member, fix);
                members.Add(fix);
            }
            return fixedNames[member];
        }

        string GetParameterDeclaration(ShaderParameter parameterDec)
        {
            string mod = parameterDec.Modifier == ParameterModifier.In ? "in " : parameterDec.Modifier == ParameterModifier.Out ? "out " :
                parameterDec.Modifier == ParameterModifier.InOut ? "inout " : "";
            return mod + GetNameFor(parameterDec.ParameterType) + " " + parameterDec.Name;
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodDeclarationAST ast)
        {
            string args = string.Join(", ", ast.Parameters.Select(p => GetParameterDeclaration(p)).ToArray());
            yield return GetNameFor(ast.ReturnType) + " " + GetNameFor(ast.Method) + "(" + args + ")";
            yield return Join(GetCode(ast.Body));
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstructorDeclarationAST ast)
        {
            string args = string.Join(", ", ast.Parameters.Select(p => GetParameterDeclaration(p)).ToArray());
            yield return ast.DeclaringType.Name + "(" + args + ")";
            yield return Join(GetCode(ast.Body))+";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderFieldDeclarationAST ast)
        {
            if (ast.Field.DeclaringType == null) // uniforms
                yield return "uniform " + GetNameFor(ast.FieldType) + " " + GetNameFor(ast.Field) + ";";
            else
                yield return GetNameFor(ast.FieldType) + " " + GetNameFor(ast.Field) + ";";
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
            yield return ast.Local.Name;
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderParameterInvokeAST ast)
        {
            yield return ast.Parameter.Name;
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderFieldInvokeAST ast)
        {
            if (ast.LeftSide == null)
                yield return GetNameFor(ast.Field);
            else
                yield return GetCode(ast.LeftSide) + "." + GetNameFor(ast.Field);
        }
    
        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodInvokeAST ast)
        {
            string args = string.Join(", ", ast.Arguments.Select(a => GetCode(a)).ToArray());
            if (ast.LeftSide == null)
                yield return GetNameFor(ast.Method) + "(" + args + ")";
            else
                yield return GetNameFor(ast.Method) + "(" + GetCode(ast.LeftSide) + (args.Length > 0 ? "," : "") + args + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstructorInvokeAST ast)
        {
            string args = string.Join (", ", ast.Arguments.Select(a=>GetCode(a)).ToArray());
            yield return GetNameFor(ast.Type) + "(" + args + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderOperationAST ast)
        {
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
            yield return GetNameFor(ast.Target) + "(" + GetCode(ast.Expression) + ")";

        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderExpressionStatementAST ast)
        {
            yield return GetCode(ast.Expression) + ";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderLocalDeclarationAST ast)
        {
            yield return GetNameFor(ast.Type) + " " + ast.Local.Name + ";";
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

        public override IEnumerable<string> GetCode(System.Compilers.Shaders.ShaderAST.ShaderProgramAST ast)
        {
            yield return "#version 120";
            yield return "";
            foreach (var d in ast.Members)
                yield return GetCode(d);

            yield return Join(GenerateMainFor(ast));
        }

        string GetInitializationCodeFor(ShaderType type)
        {
            if (type.Equals(Builtins.Int)) return "0";
            if (type.Equals(Builtins.Float)) return "0.0";
            if (type.Equals(Builtins.Boolean)) return "false";
            if (type.Equals(Builtins.Byte)) return "0";

            string argumentList = string.Join(", ", type.Members.OfType<ShaderField>().Select(f => GetInitializationCodeFor(f.Type)).ToArray());
            return GetNameFor(type) + "(" + argumentList + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderInitializationInvokeAST ast)
        {
            ShaderType type = ast.TargetType;
            yield return GetInitializationCodeFor(type);
        }

        public ShaderStage ShaderStage { get; private set; }

        int lastUsedTCVI = 7, lastUsedTCVO = 7, lastUsedTCPI = 7;

        Dictionary<Semantic, string> resolveVI = new Dictionary<Semantic, string>();
        Dictionary<Semantic, string> resolveVO = new Dictionary<Semantic, string>();
        Dictionary<Semantic, string> resolvePI = new Dictionary<Semantic, string>();
        Dictionary<Semantic, string> resolvePO = new Dictionary<Semantic, string>();

        private string GetGlobalForSemantic(Semantic s, bool isIn, out ShaderType type)
        {
            type = Builtins.Resolve (typeof(Vector4));
            if (s is NormalSemantic)
                type = Builtins.Resolve(typeof(Vector3));

            if (isIn)
            {
                switch (ShaderStage)
                {
                    case ShaderStage.Vertex:
                        if (!resolveVI.ContainsKey(s))
                        {
                            if (lastUsedTCVI > 0)
                                lastUsedTCVI--;
                            resolveVI[s] = "gl_MultiTexCoord" + lastUsedTCVI;
                        }
                        return resolveVI[s];
                    case ShaderStage.Pixel:
                        if (!resolvePI.ContainsKey(s))
                        {
                            if (lastUsedTCPI > 0)
                                lastUsedTCPI--;
                            resolvePI[s]= "gl_TexCoord[" + lastUsedTCPI + "]";
                        }
                        return resolvePI[s];
                }
            }
            else
            {
                switch (ShaderStage)
                {
                    case ShaderStage.Vertex:
                        if (!resolveVO.ContainsKey(s))
                        {
                            if (lastUsedTCVO > 0)
                                lastUsedTCVO--;
                            resolveVO[s]="gl_TexCoord[" + lastUsedTCVO + "]";
                        }
                        return resolveVO[s];
                    case ShaderStage.Pixel:
                        if (!resolvePO.ContainsKey(s))
                        {
                            return null;
                        }
                        return resolvePO[s];
                }
            }

            return null;
        }

        string FixConversion(string expression, int leftComponents, int rightComponents)
        {
            if (leftComponents == rightComponents)
                return expression;

            string[] components = new string[] { "x","y","z","w" };

            if (leftComponents < rightComponents) // demotion
                return "vec" + leftComponents + "(" + string.Join(",", Enumerable.Range(0, leftComponents).Select(i => expression + "." + components[i]).ToArray()) + ")";
            else // promotion
                return "vec" + leftComponents + "(" + string.Join(",", Enumerable.Range(0, rightComponents).Select(i => expression + "." + components[i]).Concat(Enumerable.Range(0, leftComponents - rightComponents).Select(i => "0.0")).ToArray()) + ")";
        }

        Dictionary<ShaderProgramAST, ShaderProgramAST> fixedPrograms = new Dictionary<ShaderProgramAST, ShaderProgramAST>();

        private IEnumerable<string> GenerateMainFor(Shaders.ShaderAST.ShaderProgramAST ast)
        {
            if (!fixedPrograms.ContainsKey(ast))
                fixedPrograms.Add(ast, FixProgram(ast));

            var fp = fixedPrograms[ast];

            return GetCode((ShaderMethodDeclarationAST)fp.Members.First(m => m.Member == fp.Main));

            /*yield return "void main ()";
            yield return "{";
            
            // VSIn In = VSIn (...);
            ShaderType VSInType = ast.Main.Parameters.First().ParameterType;
            ShaderType VSOutType = ast.Main.ReturnType;

            var InLocalDeclaration = mainAST.First(VSInType, "In");
            yield return Indent(Join(GetCode(InLocalDeclaration)));
            yield return Indent(Join(GetCode(ast.CreateExpressionStatement(ast.CreateAssignament(ast.CreateInvoke(InLocalDeclaration.Local), ast.CreateInitialization(VSInType))))));

            foreach (var f in InLocalDeclaration.Type.Members.OfType<ShaderField>())
            {
                var semantic = f.Semantic;

                int components;

                string global = GetGlobalForSemantic(semantic, true, out components);

                if (global != null)
                    yield return Indent("In." + GetNameFor(f) + " = " + FixConversion(global, f.Type.Members.OfType<ShaderField>().Count(), 4) + ";");
            }

            // VSOut Out = Main (In);
            var OutLocalDeclaration = ast.CreateLocalDeclaration(VSOutType, "Out");
            yield return Indent(Join(GetCode(OutLocalDeclaration)));
            yield return Indent(Join(GetCode(ast.CreateExpressionStatement(ast.CreateAssignament(ast.CreateInvoke(OutLocalDeclaration.Local),
                ast.CreateInvoke(ast.Main, null, ast.CreateInvoke(InLocalDeclaration.Local)))))));

            foreach (var f in OutLocalDeclaration.Type.Members.OfType<ShaderField>())
            {
                var semantic = f.Semantic;

                int components;

                string global = GetGlobalForSemantic(semantic, false, out components);

                if (global != null)
                    yield return Indent(global + " = " + FixConversion("Out." + GetNameFor(f), 4, f.Type.Members.OfType<ShaderField>().Count()) + ";");
            }

            yield return "}"; */
        }

        private ShaderProgramAST FixProgram(ShaderProgramAST ast)
        {
            ShaderProgramAST program = new ShaderProgramAST(ast.Builtins);

            Dictionary<ShaderMember, ShaderMember> maps, compilations;
            program.Include(ast, out maps, out compilations);

            var mainFunction = maps[ast.Main] as ShaderMethod;
            
            var main = program.DeclareNewMethod("main", 0);

            program.SetReturnType(main, Builtins.Void);

            var builder = main.GetMethodBuilder();

            var InLocal = builder.DeclareLocal(mainFunction.Parameters.First().ParameterType, "In");

            builder.AddInitialization(InLocal);

            foreach (var field in main.Parameters.First().ParameterType.Members.OfType<ShaderField>())
            {
                ShaderType type;
                var fieldDec = program.DeclareNewField(GetGlobalForSemantic(field.Semantic, true, out type), type, null);

                builder.AddAdapt(builder.Field(builder.Local(InLocal), field), builder.Field(null, fieldDec.Field));
            }

            var OutLocal = builder.DeclareLocal(mainFunction.ReturnType, "Out");

            foreach (var field in OutLocal.Type.Members.OfType<ShaderField>())
            {
                ShaderType type;

                var fieldDec = program.DeclareNewField(GetGlobalForSemantic(field.Semantic, false, out type), type, null);

                builder.AddAdapt(builder.Field(null, fieldDec.Field), builder.Field(builder.Local(OutLocal), field));
            }

            return program;
        }

    }
}
