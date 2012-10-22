using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders;
using System.Compilers.Shaders.Info;
using System.Rendering;
using System.Compilers.Shaders.ShaderAST;
using System.Rendering.Effects.Shaders;

namespace System.Compilers.Generators
{
    public class HLSLCodeGenerator : ShaderCodeGenerator
    {
        int lastUsedTC = 7;
        int samplers = 0;
        static Dictionary<ShaderMember, string> fixedNames = new Dictionary<ShaderMember, string>();
        static HashSet<string> members = new HashSet<string>();

        static Dictionary<Semantic, string> resolveVInSemantic = new Dictionary<Semantic, string>();
        static Dictionary<Semantic, string> resolveVOutSemantic = new Dictionary<Semantic, string>();
        static Dictionary<Semantic, string> resolvePInSemantic = new Dictionary<Semantic, string>();
        static Dictionary<Semantic, string> resolvePOutSemantic = new Dictionary<Semantic, string>();

        Dictionary<ShaderMember, string> currentNames;

        public ShaderStage ShaderStage { get; private set; }

        public HLSLCodeGenerator(ShaderStage stage, Builtins builtins)
        {
            this._Builtins = builtins;
            this.ShaderStage = stage;

            for (int i = 0; i < 8; i++)
            {
                resolveVInSemantic.AddIfNotPresent(ShaderSemantics.Position(i), "POSITION" + i);
                resolveVInSemantic.AddIfNotPresent(ShaderSemantics.Normal(i), "NORMAL" + i);
            }

            resolveVOutSemantic.AddIfNotPresent(ShaderSemantics.Projected(), "POSITION");

            for (int i = 0; i < 2; i++)
            {
                resolveVInSemantic.AddIfNotPresent(ShaderSemantics.Color(i), "COLOR" + i);
                resolveVOutSemantic.AddIfNotPresent(ShaderSemantics.Color(i), "COLOR" + i);
                resolvePInSemantic.AddIfNotPresent(ShaderSemantics.Color(i), "COLOR" + i);
                resolvePOutSemantic.AddIfNotPresent(ShaderSemantics.Color(i), "COLOR" + i);
            }

            resolveVOutSemantic.AddIfNotPresent(ShaderSemantics.Fog(), "FOG");
            resolvePInSemantic.AddIfNotPresent(ShaderSemantics.Fog(), "FOG");

            for (int i = 0; i < 8; i++)
            {
                resolveVInSemantic.AddIfNotPresent(ShaderSemantics.Coordinates(i), "TEXCOORD" + i);
                resolveVOutSemantic.AddIfNotPresent(ShaderSemantics.Coordinates(i), "TEXCOORD" + i);
                resolvePInSemantic.AddIfNotPresent(ShaderSemantics.Coordinates(i), "TEXCOORD" + i);
            }
        }

        private string Fix(string name)
        {
            string originalName = name.Replace("<", "").Replace(">", "").Replace(" ", "").Replace("@", "").Replace(".", "");
            name = originalName;
            int index = 0;
            while (Builtins.Keywords.Contains(name) || members.Contains(name))
                name = originalName + (index++);
            return name;
        }

        private string GetParameterDeclaration(ShaderParameter parameterDec)
        {
            string mod = parameterDec.Modifier == ParameterModifier.In ? "in " : parameterDec.Modifier == ParameterModifier.Out ? "out " :
                parameterDec.Modifier == ParameterModifier.InOut ? "inout " : "";
            return mod + ResolveName(parameterDec.ParameterType) + " " + parameterDec.Name;
        }

        int colors = 2;

        private string GetNameForSemantic(Semantic s, bool isIn)
        {
            Action<Semantic> register = (sem) =>
            {
                if (lastUsedTC > 0)
                    lastUsedTC--;
                if (!resolveVInSemantic.ContainsKey(s))
                    resolveVInSemantic[s] = "TEXCOORD" + lastUsedTC;
                if (!resolveVOutSemantic.ContainsKey(s))
                    resolveVOutSemantic[s] = "TEXCOORD" + lastUsedTC;
                if (!resolvePInSemantic.ContainsKey(s))
                    resolvePInSemantic[s] = "TEXCOORD" + lastUsedTC;
                if (!resolvePOutSemantic.ContainsKey(s))
                {
                    resolvePOutSemantic[s] = "COLOR" + colors;
                    colors++;
                }
            };
            if (isIn)
            {
                switch (ShaderStage)
                {
                    case ShaderStage.Vertex:
                        if (!resolveVInSemantic.ContainsKey(s))
                            register(s);
                        return resolveVInSemantic[s];
                    case ShaderStage.Geometry:
                        if (!resolveVOutSemantic.ContainsKey(s))
                            register(s);
                        return resolveVOutSemantic[s];
                    case ShaderStage.Pixel:
                        if (!resolvePInSemantic.ContainsKey(s))
                            register(s);
                        return resolvePInSemantic[s];
                }
            }
            else
            {
                switch (ShaderStage)
                {
                    case ShaderStage.Vertex:
                        if (!resolveVOutSemantic.ContainsKey(s))
                            register(s);
                        return resolveVOutSemantic[s];
                    case ShaderStage.Geometry:
                        if (!resolvePInSemantic.ContainsKey(s))
                            register(s);
                        return resolvePInSemantic[s];
                    case ShaderStage.Pixel:
                        if (!resolvePOutSemantic.ContainsKey(s))
                            register(s);
                        return resolvePOutSemantic[s];
                }
            }
            throw new ArgumentException();
        }

        private string GetInitializationCodeFor(ShaderType type)
        {
            return "(" + ResolveName(type) + ")0";
        }

        private string ResolveName(ShaderMember member)
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

        #region Get Codes

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderTypeDeclarationAST ast)
        {
            if (ast.IsStruct)
            {
                yield return "struct " + ResolveName(ast.Type);
                yield return "{";
                foreach (var m in ast.Members)
                    yield return Indent(GetCode(m));
                yield return "}; \n";
            }
            else
                throw new NotSupportedException();
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodDeclarationAST ast)
        {
            string args = string.Join(", ", ast.Parameters.Select(p => GetParameterDeclaration(p)).ToArray());

            yield return ResolveName(ast.ReturnType) + " " + ResolveName(ast.Method) + "(" + args + ")";
            yield return Join(GetCode(ast.Body)) + "; \n";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstructorDeclarationAST ast)
        {
            string args = string.Join(", ", ast.Parameters.Select(p => GetParameterDeclaration(p)).ToArray());
            yield return ast.DeclaringType.Name + "(" + args + ")";
            yield return Join(GetCode(ast.Body)) + ";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderFieldDeclarationAST ast)
        {
            if (ast.Field == null)
                throw new Exception("field is null");

            if (ast.FieldType == null)
                throw new Exception("field type is null");

            if (ast.FieldType.Name.StartsWith("sampler"))
            {
                yield return ResolveName(ast.FieldType) + " " + ResolveName(ast.Field) + " : register (s" + (samplers++) + ");";
            }
            else
            {
                string sem = "";
                if (ast.Semantic != null)
                {
                    bool isOut = ast.DeclaringType.Type.Equals(this.Program.Main.ReturnType);
                    sem = ":" + GetNameForSemantic(ast.Semantic, !isOut);
                }
                yield return ResolveName(ast.FieldType) + " " + ResolveName(ast.Field) + sem + ";";
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
                yield return ResolveName(ast.Field);
            else
                yield return GetCode(ast.LeftSide) + "." + ResolveName(ast.Field);
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderMethodInvokeAST ast)
        {
            string args = string.Join(", ", ast.Arguments.Select(a => GetCode(a)).ToArray());
            if (ast.LeftSide == null)
                yield return ResolveName(ast.Method) + "(" + args + ")";
            else
                yield return ResolveName(ast.Method) + "(" + GetCode(ast.LeftSide) + (args.Length > 0 ? "," : "") + args + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConstructorInvokeAST ast)
        {
            string args = string.Join(", ", ast.Arguments.Select(a => GetCode(a)).ToArray());
            yield return ResolveName(ast.Type) + "(" + args + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderOperationAST ast)
        {
            switch (ast.Operands.Length)
            {
                case 1:
                    yield return ast.Operator.Token() + GetCode(ast.Operands[0]);
                    yield break;
                case 2:
                    yield return "("+GetCode(ast.Operands[0]) + ast.Operator.Token() + GetCode(ast.Operands[1])+")";
                    yield break;
                case 3:
                    yield return "(" + GetCode(ast.Operands[0]) + "?" + GetCode(ast.Operands[1]) + ":" + GetCode(ast.Operands[2]) + ")";
                    yield break;
            }
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderConversionAST ast)
        {
            yield return "((" + ResolveName(ast.Target) + ")" + "(" + GetCode(ast.Expression) + ")" + ")";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderExpressionStatementAST ast)
        {
            if (ast.Expression != null)
                yield return GetCode(ast.Expression) + ";";
        }

        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderLocalDeclarationAST ast)
        {
            yield return ResolveName(ast.Type) + " " + ast.Local.Name + ";";
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


        public override IEnumerable<string> GetCode(Shaders.ShaderAST.ShaderInitializationInvokeAST ast)
        {
            ShaderType type = ast.TargetType;
            yield return GetInitializationCodeFor(type);
        }
        public override string GenerateCode(Shaders.ShaderAST.ShaderProgramAST ast,  out Dictionary<ShaderMember, string> names)
        {
            this.Program = ast;
            this.currentNames = new Dictionary<ShaderMember, string>();
            
            samplers = 0;

            var fullCode = string.Concat (ast.Members.Select (m=> GetCode(m)));

            this.Program = null;

            names = currentNames;
            
            return string.Concat(fullCode.Split('\n').Select((line, i) => line + "//" + i+"\n"));
        }

        #endregion

        public bool IsGenerating { get { return Program != null; } }

        Builtins _Builtins;
        public override Builtins Builtins { get { return _Builtins; } }

        public ShaderProgramAST Program { get; private set; }

    }

    public static class SomeX
    {
        public static void AddIfNotPresent<K, V>(this Dictionary<K, V> d, K key, V value)
        {
            if (!d.ContainsKey(key))
                d.Add(key, value);
        }
    }
}
