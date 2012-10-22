using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Maths;
using System.Reflection;

namespace System.Compilers.Shaders.GLSL
{
    public class GLSLBasic : Builtins
    {
        static GLSLBasic()
        {
            GLSL = new GLSLBasic();
        }

        public static Builtins GLSL { get; private set; }

        protected GLSLBasic()
        {
            Register(typeof(int), "int", null);
            Register(typeof(bool), "bool", null);
            Register(typeof(float), "float", null);
            Register(typeof(double), "float", null);
            Register(typeof(byte), "byte", null);
            Register(typeof(void), "void", null);

            RegisterMembers(typeof(int), null);
            RegisterMembers(typeof(bool), null);
            RegisterMembers(typeof(float), null);
            RegisterMembers(typeof(byte), null);


            this.FullRegisterScalarMembers<int>();
            this.FullRegisterScalarMembers<bool>();
            this.FullRegisterScalarMembers<float>();
            this.FullRegisterScalarMembers<byte>();

            Dictionary<Type, string> scalars = new Dictionary<Type, string>();
            scalars[typeof(int)] = "int";
            scalars[typeof(byte)] = "byte";
            scalars[typeof(float)] = "float";
            scalars[typeof(double)] = "float";
            scalars[typeof(bool)] = "bool";

            Register(typeof(Sampler1D), "sampler1D", null);
            Register(typeof(Sampler2D), "sampler2D", null);
            Register(typeof(Sampler3D), "sampler3D", null);
            Register(typeof(SamplerCube), "samplerCube", null);

            Func<MemberRegistration, bool> vecAndMatrixMemberRegistrating = m =>
            {
                if (m.IsConstructor) return true;

                if (m.IsField)
                {
                    m.Name = m.Name.ToLowerInvariant();
                    return true;
                }

                if (m.IsOperator)
                {
                    if (m.Name == "op_Multiply"
                        &&
                        m.Member.DeclaringType.Name.StartsWith("Matr")
                        &&
                        ((MethodInfo)m.Member).GetParameters()[0].ParameterType == ((MethodInfo)m.Member).GetParameters()[1].ParameterType)
                    {
                        m.Name = "matrixCompMult";
                    }
                    return true;
                }

                return false;
            };

            foreach (var t in scalars.Keys)
            {
                string vecprefix = t == typeof(float) ? "" : t == typeof(int) ? "i" : t == typeof(bool) ? "b" : "";

                Register(typeof(Vector1<>).MakeGenericType(t), scalars[t], null);
                Register(typeof(Vector2<>).MakeGenericType(t), vecprefix + "vec2", null);
                Register(typeof(Vector3<>).MakeGenericType(t), vecprefix + "vec3", null);
                Register(typeof(Vector4<>).MakeGenericType(t), vecprefix + "vec4", null);

                Register(typeof(Matrix1x1<>).MakeGenericType(t), vecprefix + "mat1", null);
                Register(typeof(Matrix1x2<>).MakeGenericType(t), vecprefix + "mat1x2", null);
                Register(typeof(Matrix1x3<>).MakeGenericType(t), vecprefix + "mat1x3", null);
                Register(typeof(Matrix1x4<>).MakeGenericType(t), vecprefix + "mat1x4", null);

                Register(typeof(Matrix2x1<>).MakeGenericType(t), vecprefix + "mat2x1", null);
                Register(typeof(Matrix2x2<>).MakeGenericType(t), vecprefix + "mat2", null);
                Register(typeof(Matrix2x3<>).MakeGenericType(t), vecprefix + "mat2x3", null);
                Register(typeof(Matrix2x4<>).MakeGenericType(t), vecprefix + "mat2x4", null);

                Register(typeof(Matrix3x1<>).MakeGenericType(t), vecprefix + "mat3x1", null);
                Register(typeof(Matrix3x2<>).MakeGenericType(t), vecprefix + "mat3x2", null);
                Register(typeof(Matrix3x3<>).MakeGenericType(t), vecprefix + "mat3", null);
                Register(typeof(Matrix3x4<>).MakeGenericType(t), vecprefix + "mat3x4", null);

                Register(typeof(Matrix4x1<>).MakeGenericType(t), vecprefix + "mat4x1", null);
                Register(typeof(Matrix4x2<>).MakeGenericType(t), vecprefix + "mat4x2", null);
                Register(typeof(Matrix4x3<>).MakeGenericType(t), vecprefix + "mat4x3", null);
                Register(typeof(Matrix4x4<>).MakeGenericType(t), vecprefix + "mat4", null);
            }

            Register(typeof(Vector1), "float", null);
            Register(typeof(Vector2), "vec2", null);
            Register(typeof(Vector3), "vec3", null);
            Register(typeof(Vector4), "vec4", null);

            Register(typeof(Matrix1x1), "mat1", null);
            Register(typeof(Matrix1x2), "mat1x2", null);
            Register(typeof(Matrix1x3), "mat1x3", null);
            Register(typeof(Matrix1x4), "mat1x4", null);

            Register(typeof(Matrix2x1), "mat2x1", null);
            Register(typeof(Matrix2x2), "mat2", null);
            Register(typeof(Matrix2x3), "mat2x3", null);
            Register(typeof(Matrix2x4), "mat2x4", null);

            Register(typeof(Matrix3x1), "mat3x1", null);
            Register(typeof(Matrix3x2), "mat3x2", null);
            Register(typeof(Matrix3x3), "mat3", null);
            Register(typeof(Matrix3x4), "mat3x4", null);

            Register(typeof(Matrix4x1), "mat4x1", null);
            Register(typeof(Matrix4x2), "mat4x2", null);
            Register(typeof(Matrix4x3), "mat4x3", null);
            Register(typeof(Matrix4x4), "mat4", null);

            foreach (var t in scalars.Keys)
            {
                RegisterMembers(typeof(Vector1<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Vector2<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Vector3<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Vector4<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);

                RegisterMembers(typeof(Matrix1x1<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix1x2<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix1x3<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix1x4<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);

                RegisterMembers(typeof(Matrix2x1<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix2x2<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix2x3<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix2x4<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);

                RegisterMembers(typeof(Matrix3x1<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix3x2<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix3x3<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix3x4<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);

                RegisterMembers(typeof(Matrix4x1<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix4x2<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix4x3<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
                RegisterMembers(typeof(Matrix4x4<>).MakeGenericType(t), vecAndMatrixMemberRegistrating);
            }

            RegisterMembers(typeof(Vector1), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Vector2), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Vector3), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Vector4), vecAndMatrixMemberRegistrating);

            RegisterMembers(typeof(Matrix1x1), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix2x1), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix3x1), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix4x1), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix1x2), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix2x2), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix3x2), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix4x2), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix1x3), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix2x3), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix3x3), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix4x3), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix1x4), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix2x4), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix3x4), vecAndMatrixMemberRegistrating);
            RegisterMembers(typeof(Matrix4x4), vecAndMatrixMemberRegistrating);

            #region Register math functions

            Type mathType = typeof(GMath);

            this.RegisterAvailableOverloads(mathType, "abs", "abs");
            this.RegisterAvailableOverloads(mathType, "acos", "acos");
            this.RegisterAvailableOverloads(mathType, "all", "all");
            this.RegisterAvailableOverloads(mathType, "any", "any");
            this.RegisterAvailableOverloads(mathType, "asin", "asin");
            this.RegisterAvailableOverloads(mathType, "atan", "atan");
            this.RegisterAvailableOverloads(mathType, "atan2", "atan2");
            this.RegisterAvailableOverloads(mathType, "ceil", "ceil");
            this.RegisterAvailableOverloads(mathType, "clamp", "clamp");
            this.RegisterAvailableOverloads(mathType, "cos", "cos");
            this.RegisterAvailableOverloads(mathType, "cosh", "cosh");
            this.RegisterAvailableOverloads(mathType, "cross", "cross");
            this.RegisterAvailableOverloads(mathType, "degrees", "degrees");
            this.RegisterAvailableOverloads(mathType, "determinant", "determinant");
            this.RegisterAvailableOverloads(mathType, "distance", "distance");
            this.RegisterAvailableOverloads(mathType, "dot", "dot");
            this.RegisterAvailableOverloads(mathType, "exp", "exp");
            this.RegisterAvailableOverloads(mathType, "exp2", "exp2");
            this.RegisterAvailableOverloads(mathType, "floor", "floor");
            this.RegisterAvailableOverloads(mathType, "fmod", "mod");
            this.RegisterAvailableOverloads(mathType, "frac", "fract");
            this.RegisterAvailableOverloads(mathType, "isfinite", "isfinite");
            this.RegisterAvailableOverloads(mathType, "isinf", "isinf");
            this.RegisterAvailableOverloads(mathType, "isnan", "isnan");

            this.RegisterAvailableOverloads(mathType, "ldexp", "ldexp");
            this.RegisterAvailableOverloads(mathType, "length", "length");
            this.RegisterAvailableOverloads(mathType, "lerp", "mix");
            this.RegisterAvailableOverloads(mathType, "lit", "lit");

            this.RegisterAvailableOverloads(mathType, "log", "log");
            this.RegisterAvailableOverloads(mathType, "log10", "log10");
            this.RegisterAvailableOverloads(mathType, "log2", "log2");
            this.RegisterAvailableOverloads(mathType, "max", "max");
            this.RegisterAvailableOverloads(mathType, "min", "min");
            this.RegisterAvailableOverloads(mathType, "mul", "op_Multiply");
            this.RegisterAvailableOverloads(mathType, "normalize", "normalize");
            this.RegisterAvailableOverloads(mathType, "pow", "pow");
            this.RegisterAvailableOverloads(mathType, "radians", "radians");
            this.RegisterAvailableOverloads(mathType, "reflect", "reflect");
            this.RegisterAvailableOverloads(mathType, "refract", "refract");
            this.RegisterAvailableOverloads(mathType, "round", "round");
            this.RegisterAvailableOverloads(mathType, "rsqrt", "inversesqrt");
            this.RegisterAvailableOverloads(mathType, "saturate", "saturate");
            this.RegisterAvailableOverloads(mathType, "sign", "sign");
            this.RegisterAvailableOverloads(mathType, "sin", "sin");
            this.RegisterAvailableOverloads(mathType, "sinh", "sinh");
            this.RegisterAvailableOverloads(mathType, "smoothstep", "smoothstep");
            this.RegisterAvailableOverloads(mathType, "sqrt", "sqrt");
            this.RegisterAvailableOverloads(mathType, "step", "step");
            this.RegisterAvailableOverloads(mathType, "tanh", "tan");
            this.RegisterAvailableOverloads(mathType, "transpose", "transpose");

            #endregion

            #region Register sampler functions
            this.RegisterAvailableOverloads(typeof(Sampler1D), "Sample", "texture1D");
            this.RegisterAvailableOverloads(typeof(Sampler2D), "Sample", "texture2D");
            this.RegisterAvailableOverloads(typeof(Sampler2D), "SampleLOD", "texture2DLod");
            this.RegisterAvailableOverloads(typeof(Sampler3D), "Sample", "texture3D");
            this.RegisterAvailableOverloads(typeof(SamplerCube), "Sample", "textureCube");
            #endregion

            EndInitialization();
        }

        public override IEnumerable<string> Keywords
        {
            get
            {
                yield return "sampler";
                yield return "Sampler1D";
                yield return "Sampler2D";
                yield return "SamplerCube";
                yield return "Sampler3D";
                yield return "struct";
                yield return "for";
                yield return "if";
                yield return "while";
                yield return "return";
                yield return "else";
                yield return "in";
                yield return "out";
                yield return "inout";
								yield return "input";
								yield return "output";
								yield return "INPUT";
								yield return "OUTPUT";
            }
        }
    }
}
