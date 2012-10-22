#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if DOUBLE
using FLOATINGTYPE = System.Double;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Compilers.Shaders;
using System.Maths;

namespace System.Rendering.OpenGL
{
    public class OpenGLGLSLBuiltins : Builtins
    {
        public static readonly OpenGLGLSLBuiltins Instance = new OpenGLGLSLBuiltins();

        public string AssemblyName
        {
            get
            {
                string assemblyName = typeof(Vector4<float>).AssemblyQualifiedName;
                int comaPos = assemblyName.IndexOf(",");
                if (comaPos < 0)
                    return "";
                else
                    return assemblyName.Substring(comaPos + 1);
            }
        }

        private string prefix(Type type)
        {
            if (type == typeof(int))
                return "i";
            if (type == typeof(short))
                return "i";
            if (type == typeof(bool))
                return "b";
            if (type == typeof(float))
                return "";
            if (type == typeof(double))
                return "";

            return "";
        }

        protected OpenGLGLSLBuiltins()
            : base()
        {
            Dictionary<Type, string> baseTypes = new Dictionary<Type, string>();
            baseTypes.Add(typeof(int), "int");
            baseTypes.Add(typeof(float), "float");
            baseTypes.Add(typeof(bool), "bool");
            baseTypes.Add(typeof(short), "int");
            baseTypes.Add(typeof(double), "float");

            foreach (var baseType in baseTypes.Keys)
                Register(baseType, baseTypes[baseType], null, null);

            Func<MemberRegistration, bool> vecAndMatrixMemberRegistrating = m =>
            {
                if (m.IsConstructor) return true;

                if (m.IsField)
                {
                    m.Name = m.Name.ToLowerInvariant();
                    return true;
                }

                return false;
            };

            string assemblyInfo = AssemblyName;

            for (int i=1; i<= 4; i++)
                foreach (var baseType in baseTypes.Keys)
                {
                    Type genericVectorType = Type.GetType("System.Rendering.Vector" + i + "`1, " + assemblyInfo);
                    Type vectorType = genericVectorType.MakeGenericType(baseType);
                    Register (vectorType, prefix (baseType)+ "vec" + i, null, vecAndMatrixMemberRegistrating);
                }

            for (int r = 1; r <= 4; r++)
                for (int c = 1; c <= 4; c++)
                    foreach (var baseType in baseTypes.Keys)
                    {
                        Type genericMatrixType = Type.GetType(string.Format("System.Rendering.Matrix{0}x{1}`1, " + assemblyInfo, r, c));
                        Type matrixType = genericMatrixType.MakeGenericType(baseType);
                        Register(matrixType, prefix(baseType) + "mat" + r, null, vecAndMatrixMemberRegistrating);
                    }

            //Register(typeof(Sampler), "sampler2D");


            #region Register Base Types Members

            foreach (var baseType in baseTypes.Keys)
                this.FullRegisterScalarMembers(baseType);

            #endregion

            #region Register math functions

            Type mathType = typeof(Math);

            this.RegisterAvailableOverloads(mathType, "Max", "max");
            this.RegisterAvailableOverloads(mathType, "Min", "min");
            this.RegisterAvailableOverloads(mathType, "Pow", "pow");
            this.RegisterAvailableOverloads(mathType, "Sin", "sin");
            this.RegisterAvailableOverloads(mathType, "Cos", "cos");
            this.RegisterAvailableOverloads(mathType, "Tan", "tan");

            #endregion

            #region Register Sample Members

            //RegisterFunction(typeof(Sampler2D).GetMethod("Sample", new Type[] { typeof(Vector2<FLOATINGTYPE>) }), "texture2D");

            #endregion

            #region Register Vector Members

            string[] components = new string[] { "x", "y", "z", "w"};

            //for (int i=1; i<= 4; i++)
            //    foreach (var baseType in baseTypes.Keys)
            //    {
            //        Type genericVectorType = Type.GetType("System.Rendering.Vector" + i + "`1, "+assemblyInfo);
            //        Type vectorType = genericVectorType.MakeGenericType(baseType);
            //        this.FullRegisterScalarMembers(vectorType, components.Take(i).ToArray());

            //        this.Register(vectorType.GetMethod("get_Normalized"), "normalize", null);
            //        this.Register(vectorType.GetMethod("Dot"), "dot", null);

            //        if (i >= 3)
            //            this.Register(vectorType.GetMethod("Cross"), "cross", null);
            //    }

            #endregion

            #region Register all matrices members

            //for (int r = 1; r <= 4; r++)
            //    for (int c = 1; c <= 4; c++)
            //        foreach (var baseType in baseTypes.Keys)
            //        {
            //            Type genericMatrixType = Type.GetType(string.Format("System.Rendering.Matrix{0}x{1}`1, "+assemblyInfo, r, c));
            //            Type matrixType = genericMatrixType.MakeGenericType(baseType);
            //            List<string> fieldNames = new List<string>();

            //            for (int row = 1; row <= r; row++)
            //                for (int col = 1; col <= c; col++)
            //                    fieldNames.Add(string.Format("M{0}{1}", row, col));

            //            this.FullRegisterStructMembers(matrixType, fieldNames.ToArray());
            //        }

            #endregion
        }
        
        public override IEnumerable<string> Keywords
        {
            get
            {
                yield return "sampler";
                yield return "main";
            }
        }
    }
}
