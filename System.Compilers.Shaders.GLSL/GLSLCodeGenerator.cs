using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders;
using System.Compilers.Shaders.Info;
using System.Compilers.Shaders.ShaderAST;

namespace System.Compilers.Generators
{
    public class GLSLCodeGenerator : ShaderCodeGenerator
    {
        public GLSLCodeGenerator(ShaderStage stage, Builtins builtins)
        {
            this.ShaderStage = stage;
            this.Builtins = builtins;

            resolveVI.Add(ShaderSemantics.Position(0), "gl_Vertex");
            resolveVI.Add(ShaderSemantics.Normal(0), "gl_Normal");
            resolveVI.Add(ShaderSemantics.Diffuse(), "gl_Color");
            resolveVI.Add(ShaderSemantics.Specular(), "gl_SecondaryColor");
            resolveVI.Add(ShaderSemantics.Fog(), "gl_Fog");
            for (int i = 0; i < 16; i++)
                resolveVI.Add(ShaderSemantics.TextureCoordinates(i), "gl_MultiTexCoord" + i);

            resolveVO.Add(ShaderSemantics.Projected(), "gl_Position");
            resolveVO.Add(ShaderSemantics.Diffuse(), "gl_FrontColor");
            resolveVO.Add(ShaderSemantics.Specular(), "gl_FrontSecondaryColor");
            for (int i = 0; i < 16; i++)
                resolveVO.Add(ShaderSemantics.TextureCoordinates(i), "gl_TexCoord[" + i + "]");

            resolvePI.Add(ShaderSemantics.Diffuse(), "gl_Color");
            resolvePI.Add(ShaderSemantics.Specular(), "gl_SecondaryColor");
            for (int i = 0; i < 16; i++)
                resolvePI.Add(new TextureCoordinatesSemantic() { Index = i }, "gl_TexCoord[" + i + "]");

            resolvePO.Add(ShaderSemantics.Diffuse(), "gl_FragColor");
            resolvePO.Add(ShaderSemantics.Specular(), "gl_SecondaryFragColor");
        }

        public Builtins Builtins { get; private set; }

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

        Dictionary<ShaderMember, string> fixedNames = new Dictionary<ShaderMember, string>();
        HashSet<string> members = new HashSet<string>();

        int samplers = 0;

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
            //yield return "#version 120";
            //yield return "";
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

        int lastUsedTCVI = 4, lastUsedTCVO = 4, lastUsedTCPI = 4, lastUsedTCPO = 4;

        Dictionary<Semantic, string> resolveVI = new Dictionary<Semantic, string>();
        Dictionary<Semantic, string> resolveVO = new Dictionary<Semantic, string>();
        Dictionary<Semantic, string> resolvePI = new Dictionary<Semantic, string>();
        Dictionary<Semantic, string> resolvePO = new Dictionary<Semantic, string>();

        private string GetGlobalForSemantic(Semantic s, bool isIn, out int components)
        {
            components = 4;
            if (s is NormalSemantic)
                components = 3;

            if (isIn)
            {
                switch (ShaderStage)
                {
                    case Generators.ShaderStage.Vertex:
                        if (!resolveVI.ContainsKey(s))
                        {
                            if (lastUsedTCVI > 0)
                                lastUsedTCVI--;
                            resolveVI[s] = "gl_MultiTexCoord" + lastUsedTCVI;
                        }
                        return resolveVI[s];
                    case Generators.ShaderStage.Pixel:
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
                    case Generators.ShaderStage.Vertex:
                        if (!resolveVO.ContainsKey(s))
                        {
                            if (lastUsedTCVO > 0)
                                lastUsedTCVO--;
                            resolveVO[s]="gl_TexCoord[" + lastUsedTCVO + "]";
                        }
                        return resolveVO[s];
                    case Generators.ShaderStage.Pixel:
                        if (!resolvePO.ContainsKey(s))
                        {
                            return null;
                        }
                        return resolvePO[s];
                }
            }

            return null;
        }

        private System.Compilers.Shaders.ShaderAST.ShaderAssignamentAST FixVectorAssignament(Shaders.ShaderAST.ShaderProgramAST ast, System.Compilers.Shaders.ShaderAST.ShaderExpressionAST leftValue,
            System.Compilers.Shaders.ShaderAST.ShaderExpressionAST rightValue)
        {
            int leftLength = leftValue.Type.Members.Count();
            int rightLength = rightValue.Type.Members.Count();

            if (leftLength == rightLength)
                return ast.CreateAssignament(leftValue, rightValue);

            if (leftLength < rightLength) // demotion
                return ast.CreateAssignament(leftValue, ast.CreateInvoke(leftValue.Type.Members.OfType<ShaderConstructor>().First(c => c.Parameters.Count() == leftLength),
                    rightValue.Type.Members.OfType<ShaderField>().Take(leftLength).Select(f => (ShaderExpressionAST)ast.CreateInvoke(f, rightValue)).ToArray()));
            else
                // promotion
                return ast.CreateAssignament(leftValue, ast.CreateInvoke(leftValue.Type.Members.OfType<ShaderConstructor>().First(c => c.Parameters.Count() == leftLength),
                    rightValue.Type.Members.OfType<ShaderField>().Select(f => (ShaderExpressionAST)ast.CreateInvoke(f, rightValue)).Concat(Enumerable.Range(0, leftLength - rightLength).Select(i => (ShaderExpressionAST)ast.CreateConstant(0.0f))).ToArray()));
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

        private IEnumerable<string> GenerateMainFor(Shaders.ShaderAST.ShaderProgramAST ast)
        {
            yield return "void main ()";
            yield return "{";
            // ensures position element is always set
            if (this.ShaderStage == Generators.ShaderStage.Vertex)
                yield return Indent("gl_Position = gl_Vertex;");
            
            // VSIn In = VSIn (...);
            ShaderType VSInType = ast.Main.Parameters.First().ParameterType;
            ShaderType VSOutType = ast.Main.ReturnType;
            var InLocalDeclaration = ast.CreateLocalDeclaration(VSInType, "In");
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

            yield return "}";
        }

    }

    public enum ShaderStage
    {
        Vertex,
        Geometry,
        Pixel
    }
}
