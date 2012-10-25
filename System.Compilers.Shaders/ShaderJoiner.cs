using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders.ShaderAST;
using System.Compilers.Shaders.Info;
using System.Maths;

namespace System.Compilers.Shaders
{
    public static class ShaderJoiner
    {
        static int ID = 0;
        class BySemanticEqComparer : IEqualityComparer<ShaderFieldDeclarationAST>
        {
            public bool Equals(ShaderFieldDeclarationAST x, ShaderFieldDeclarationAST y)
            {
                return x.Semantic.Equals(y.Semantic);
            }

            public int GetHashCode(ShaderFieldDeclarationAST obj)
            {
                return obj.Semantic.GetHashCode();
            }
        }

        static ShaderFieldDeclarationAST SelectBest(ShaderFieldDeclarationAST d1, ShaderFieldDeclarationAST d2)
        {
            if (d1.FieldType.Members.Count() > d2.FieldType.Members.Count())
                return d1;
            return d2;
        }

        static IEnumerable<ShaderFieldDeclarationAST> Merge(IEnumerable<ShaderFieldDeclarationAST> e1, IEnumerable<ShaderFieldDeclarationAST> e2)
        {
            foreach (var d1 in e1)
                foreach (var d2 in e2)
                    if (d1.Semantic.Equals(d2.Semantic))
                        yield return SelectBest(d1, d2);
        }

        static ShaderTypeDeclarationAST JoinSimpleType(ShaderProgramAST join, ShaderTypeDeclarationAST s1, ShaderTypeDeclarationAST s2)
        {
            HashSet<string> names = new HashSet<string>();
            Func<string, string> fixName = (n) =>
            {
                if (!names.Contains(n))
                {
                    names.Add(n);
                    return n;
                }
                else
                {
                    int index = 0;
                    while (names.Contains(n + index))
                        index++;
                    names.Add(n + index);
                    return n + index;
                }
            };
            ShaderTypeDeclarationAST newDeclaration = join.DeclareNewStruct("DATA_" + (ID++), 0);

            var same = Merge(s1.Members.OfType<ShaderFieldDeclarationAST>(), s2.Members.OfType<ShaderFieldDeclarationAST>());
            var in1not2 = s1.Members.OfType<ShaderFieldDeclarationAST>().Except(s2.Members.OfType<ShaderFieldDeclarationAST>(), new BySemanticEqComparer());
            var in2not1 = s2.Members.OfType<ShaderFieldDeclarationAST>().Except(s1.Members.OfType<ShaderFieldDeclarationAST>(), new BySemanticEqComparer());

            foreach (var m in same.Concat(in1not2).Concat(in2not1))
                newDeclaration.DeclareNewField(m.FieldType, fixName(m.Name), m.Semantic);

            return newDeclaration;
        }

        static void AddAsignations(ShaderMethodBuilder builder, ShaderExpressionAST toExp, ShaderExpressionAST fromExp)
        {
            var program = builder.Program;

            var toType = toExp.Type;
            var fromType = fromExp.Type;

            foreach (var fieldTo in toType.Members.OfType<ShaderField>())
            {
                var fieldFrom = fromType.Members.OfType<ShaderField>().FirstOrDefault(f => f != null && object.Equals(f.Semantic, fieldTo.Semantic));

                if (fieldFrom != null)
                    builder.AddAdapt(program.CreateInvoke(fieldTo, toExp), program.CreateInvoke(fieldFrom, fromExp));
            }
        }

        struct JoinResult
        {
            public ShaderProgramAST Program;
            public Dictionary<ShaderMember, ShaderMember> Maps;
        }

        static ShaderMethod CreatePromotionOf(ShaderProgramAST program, ShaderType type)
        {
            string methodName = "promoteVec";
            var method = program.Members.OfType<ShaderMethod>().FirstOrDefault(m => m.Name.StartsWith(methodName) && m.Parameters.First().ParameterType.Equals(type));
            if (method != null)
                return method;

            program.PushMode(ShaderProgramAST.AddingMode.Prepend);
            var methodDeclaration = program.DeclareNewMethod(methodName, 0);
            program.PopMode();

            var parameter = methodDeclaration.DeclareNewParameter(type, "v", null, ParameterModifier.In);

            var vec4Type = program.Builtins.Resolve(typeof(Vector4));
            methodDeclaration.ReturnType = vec4Type;

            var builder = methodDeclaration.GetMethodBuilder();

            ShaderExpressionAST[] arguments = new ShaderExpressionAST[4];

            int count = 0;
            foreach (var m in type.Members.OfType<ShaderField>())
            {
                arguments[count] = program.CreateInvoke(m, program.CreateInvoke(parameter));
                count++;
            }
            while (count < 4)
                arguments[count++] = program.CreateConstant<float>(0.0f);

            builder.AddReturn(program.CreateInvoke(program.Builtins.GetBestOverload(vec4Type, new ShaderType[] { program.Builtins.Float, program.Builtins.Float, program.Builtins.Float, program.Builtins.Float }), arguments));

            return methodDeclaration.Method;
        }

        static SecuenceKeyDictionary<JoinResult> __JoinCache = new SecuenceKeyDictionary<JoinResult>();
        static SecuenceKeyDictionary<JoinResult> __ImplementCache = new SecuenceKeyDictionary<JoinResult>();

        static ShaderProgramAST JoinBySemantics(ShaderProgramAST p1, ShaderProgramAST p2, out Dictionary<ShaderMember, ShaderMember> maps)
        {
            if (!__JoinCache.ContainsKey(p1, p2))
            {
                if (p1.Builtins != p2.Builtins)
                    throw new InvalidOperationException("Builtins should match");

                maps = new Dictionary<ShaderMember, ShaderMember>();

                var InputType1 = p1.Members.OfType<ShaderTypeDeclarationAST>().First(t => t.Type.Equals(p1.Main.Parameters.First().ParameterType));
                var OutputType1 = p1.Members.OfType<ShaderTypeDeclarationAST>().First(t => t.Type.Equals(p1.Main.ReturnType));

                var InputType2 = p2.Members.OfType<ShaderTypeDeclarationAST>().First(t => t.Type.Equals(p2.Main.Parameters.First().ParameterType));
                var OutputType2 = p2.Members.OfType<ShaderTypeDeclarationAST>().First(t => t.Type.Equals(p2.Main.ReturnType));

                ShaderProgramAST join = new ShaderProgramAST(p1.Builtins);
                join.PushMode (ShaderProgramAST.AddingMode.Append);

                Dictionary<ShaderMember, ShaderMember> mapsP1;
                Dictionary<ShaderMember, ShaderMember> compilationsP1;
                join.Include(p1, out mapsP1, out compilationsP1); // Program 1
                foreach (var pair in mapsP1)
                    maps.Add(pair.Key, pair.Value);

                Dictionary<ShaderMember, ShaderMember> mapsP2;
                Dictionary<ShaderMember, ShaderMember> compilationsP2;
                join.Include(p2, out mapsP2, out compilationsP2); // Program 2
                foreach (var pair in mapsP2)
                    maps.Add(pair.Key, pair.Value);

                var i1 = compilationsP1[InputType1.Type] as ShaderType;
                var o1 = compilationsP1[OutputType1.Type] as ShaderType;
                var i2 = compilationsP2[InputType2.Type] as ShaderType;
                var o2 = compilationsP2[OutputType2.Type] as ShaderType;
                
                var newInput = JoinSimpleType(join, InputType1, InputType2); // union of Inputs

                var newOutput = JoinSimpleType(join, OutputType1, OutputType2); // union of Outputs
                
                var newMain = join.DeclareNewMethod("MAIN_" + (ID++), 0); // new Main

                join.Main = newMain.Method;
                
                newMain.ReturnType = newOutput.Type; // return type

                var InParameter = newMain.DeclareNewParameter(newInput.Type, "In", null, Info.ParameterModifier.In); // In parameter

                ShaderMethodBuilder builder = newMain.GetMethodBuilder();

                var OutLocal = builder.DeclareLocal(newOutput.Type, "Out"); // Out local

                var Input1Local = builder.DeclareLocal(i1, "Input1"); // Input1 local

                var Output1Local = builder.DeclareLocal(o1, "Output1"); // Output1 local

                var Input2Local = builder.DeclareLocal(i2, "Input2"); // Input2 local

                var Output2Local = builder.DeclareLocal(o2, "Output2"); // Output2 local

                builder.AddInitialization(Input1Local);
                builder.AddInitialization(Input2Local);

                // Input1 <- In
                AddAsignations(builder, join.CreateInvoke(Input1Local), join.CreateInvoke(InParameter));
                // Input2 <- In
                AddAsignations(builder, join.CreateInvoke(Input2Local), join.CreateInvoke(InParameter));

                // Output1 = Main1 (Input1);
                builder.AddAssignament(Output1Local, join.CreateInvoke(compilationsP1[p1.Main] as ShaderMethod, null, join.CreateInvoke(Input1Local)));

                // Out <- Output1
                AddAsignations(builder, join.CreateInvoke(OutLocal), join.CreateInvoke(Output1Local));

                // Input2 <- Output1
                AddAsignations(builder, join.CreateInvoke(Input2Local), join.CreateInvoke(Output1Local));

                // Output2 = Main2 (Input2);
                builder.AddAssignament(Output2Local, join.CreateInvoke(compilationsP2[p2.Main] as ShaderMethod, null, join.CreateInvoke(Input2Local)));

                // Out <- Output2
                AddAsignations(builder, join.CreateInvoke(OutLocal), join.CreateInvoke(Output2Local));

                builder.AddReturn(join.CreateInvoke(OutLocal));

                __JoinCache[p1, p2] = new JoinResult { Maps = maps, Program = join };

                join.PopMode();
            }
            var result = __JoinCache[p1, p2];
            maps = result.Maps;
            return result.Program;
        }

        /// <summary>
        /// Gets the pixel shader result of chaining two pixel shaders.
        /// </summary>
        public static ShaderProgramAST JoinPixelShaderPixelShader(ShaderProgramAST pixelShader1, ShaderProgramAST pixelShader2, out Dictionary<ShaderMember, ShaderMember> maps)
        {
            return JoinBySemantics (pixelShader1, pixelShader2, out maps);
        }

        /// <summary>
        /// Gets the vertex shader result of chaining two vertex shaders.
        /// </summary>
        public static ShaderProgramAST JoinVertexShaderVertexShader(ShaderProgramAST vertexShader1, ShaderProgramAST vertexShader2, out Dictionary<ShaderMember, ShaderMember> maps)
        {
            return JoinBySemantics(vertexShader1, vertexShader2, out maps);
        }

        public static ShaderProgramAST JoinGeometryShaderGeomertyShader(ShaderProgramAST geometryShader1, ShaderProgramAST geometryShader2, out Dictionary<ShaderMember, ShaderMember> maps)
        {
            throw new NotImplementedException();
        }

        public static ShaderProgramAST JoinGeometryShaderVertexShader(ShaderProgramAST geometryShader1, ShaderProgramAST vertexShader2, out Dictionary<ShaderMember, ShaderMember> maps)
        {
            throw new NotImplementedException();
        }

        public static ShaderProgramAST ImplementMethod(ShaderProgramAST abstractProgram, ShaderMethod abstractMethod, ShaderProgramAST overrideProgram, out Dictionary<ShaderMember, ShaderMember> maps)
        {
            if (!__ImplementCache.ContainsKey(abstractProgram, abstractMethod, overrideProgram))
            {
                var overrideMethod = overrideProgram.Main;
                if (!overrideMethod.ReturnType.Equals(abstractMethod.ReturnType))
                    throw new ArgumentException("override method has not the same return type than abstract method");

                if (!overrideMethod.Parameters.Select(p => p.ParameterType).SequenceEqual(abstractMethod.Parameters.Select(p => p.ParameterType)))
                    throw new ArgumentException("Argument types doesnt match");

                if (abstractProgram.Builtins != overrideProgram.Builtins)
                    throw new InvalidOperationException("Builtins should match");

                maps = new Dictionary<ShaderMember, ShaderMember>();

                ShaderProgramAST join = new ShaderProgramAST(abstractProgram.Builtins);
                join.PushMode (ShaderProgramAST.AddingMode.Append);

                Dictionary<ShaderMember, ShaderMember> mapsP1;
                Dictionary<ShaderMember, ShaderMember> compilationsP1;
                join.Include(overrideProgram, out mapsP1, out compilationsP1); // Program 1
                foreach (var pair in mapsP1)
                    maps.Add(pair.Key, pair.Value);

                Dictionary<ShaderMember, ShaderMember> mapsP2;
                Dictionary<ShaderMember, ShaderMember> compilationsP2;
                join.Include(abstractProgram, out mapsP2, out compilationsP2); // Program 2
                foreach (var pair in mapsP2)
                    maps.Add(pair.Key, pair.Value);

                if (abstractMethod.ReturnType.Equals(abstractProgram.Builtins.Void))
                    join.Implement(compilationsP2[abstractMethod] as ShaderMethod, (builder) =>
                    {
                        builder.AddCall(null, compilationsP1[overrideProgram.Main] as ShaderMethod,
                        abstractMethod.Parameters.Select(p => join.CreateInvoke(p)).ToArray());
                    });
                else
                    join.Implement(compilationsP2[abstractMethod] as ShaderMethod, (builder) =>
                    {
                        builder.AddReturn(builder.Call(null, compilationsP1[overrideProgram.Main] as ShaderMethod,
                        abstractMethod.Parameters.Select(p => join.CreateInvoke(p)).ToArray()));
                    });

                join.Main = compilationsP2[abstractProgram.Main] as ShaderMethod;

                __ImplementCache[abstractProgram, abstractMethod, overrideProgram] = new JoinResult { Maps = maps, Program = join };

                join.PopMode();
            }
            var result = __ImplementCache[abstractProgram, abstractMethod, overrideProgram];
            maps = result.Maps;
            return result.Program;
        }
    }

    public class SecuenceKey
    {
        public object[] Secuence {get; private set;}

        public SecuenceKey (object[] secuence){ Secuence = secuence;}
    }

    public struct SecuenceKeyEqualityComparer : IEqualityComparer<SecuenceKey>
    {
        public bool Equals(SecuenceKey x, SecuenceKey y)
        {
            return x.Secuence.Length == y.Secuence.Length && x.Secuence.SequenceEqual(y.Secuence);
        }

        public int GetHashCode(SecuenceKey obj)
        {
            return obj.Secuence.Aggregate(0, (bef, current) => bef ^ current.GetHashCode());
        }
    }
    
    public class SecuenceKeyDictionary<T> : Dictionary<SecuenceKey, T>
    {
        public SecuenceKeyDictionary()
            : base(new SecuenceKeyEqualityComparer())
        {
        }

        public bool ContainsKey(params object[] secuence)
        {
            return base.ContainsKey(new SecuenceKey(secuence));
        }

        public void Add(T value, params object[] secuence)
        {
            base.Add(new SecuenceKey(secuence), value);
        }

        public T this[params object[] secuence]
        {
            get { return base[new SecuenceKey(secuence)]; }
            set { base[new SecuenceKey(secuence)] = value; }
        }
    }
}
