using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Maths;

namespace System.Compilers.Shaders.HLSL
{
    public class HLSLBasic : Builtins
    {
        public static Builtins HLSL { get; private set; }

        static HLSLBasic()
        {
            HLSL = new HLSLBasic();
        }

        protected HLSLBasic()
        {
            Register(typeof(int), "int", null);
            Register(typeof(bool), "bool", null);
            Register(typeof(float), "float", null);
            Register(typeof(double), "double", null);
            Register(typeof(byte), "byte", null);
            Register(typeof(void), "void", null);

            RegisterMembers(typeof(int), null);
            RegisterMembers(typeof(bool), null);
            RegisterMembers(typeof(float), null);
            RegisterMembers(typeof(double), null);
            RegisterMembers(typeof(byte), null);


            this.FullRegisterScalarMembers<int>();
            this.FullRegisterScalarMembers<bool>();
            this.FullRegisterScalarMembers<float>();
            this.FullRegisterScalarMembers<double>();
            this.FullRegisterScalarMembers<byte>();

            Dictionary<Type, string> scalars = new Dictionary<Type, string>();
            scalars[typeof(int)] = "int";
            scalars[typeof(byte)] = "byte";
            scalars[typeof(float)] = "float";
            scalars[typeof(double)] = "double";
            scalars[typeof(bool)] = "bool";

            Func<MemberRegistration, bool> vecAndMatrixMemberRegistrating = m =>
            {
                if (m.IsConstructor) return true;

                if (m.IsField)
                {
                    m.Name = m.Name.ToLowerInvariant();
                    return true;
                }

                if (m.IsOperator)
                    return true;

                return false;
            };

            Register(typeof(Sampler1D), "sampler", null);
            Register(typeof(Sampler2D), "sampler", null);
            Register(typeof(Sampler3D), "sampler", null);
            Register(typeof(SamplerCube), "sampler", null);

            foreach (var t in scalars.Keys)
            {
                Register(typeof(Vector1<>).MakeGenericType(t), scalars[t] + "1", null);
                Register(typeof(Vector2<>).MakeGenericType(t), scalars[t] + "2", null);
                Register(typeof(Vector3<>).MakeGenericType(t), scalars[t] + "3", null);
                Register(typeof(Vector4<>).MakeGenericType(t), scalars[t] + "4", null);

                Register(typeof(Matrix1x1<>).MakeGenericType(t), scalars[t] + "1x1", null);
                Register(typeof(Matrix1x2<>).MakeGenericType(t), scalars[t] + "1x2", null);
                Register(typeof(Matrix1x3<>).MakeGenericType(t), scalars[t] + "1x3", null);
                Register(typeof(Matrix1x4<>).MakeGenericType(t), scalars[t] + "1x4", null);

                Register(typeof(Matrix2x1<>).MakeGenericType(t), scalars[t] + "2x1", null);
                Register(typeof(Matrix2x2<>).MakeGenericType(t), scalars[t] + "2x2", null);
                Register(typeof(Matrix2x3<>).MakeGenericType(t), scalars[t] + "2x3", null);
                Register(typeof(Matrix2x4<>).MakeGenericType(t), scalars[t] + "2x4", null);

                Register(typeof(Matrix3x1<>).MakeGenericType(t), scalars[t] + "3x1", null);
                Register(typeof(Matrix3x2<>).MakeGenericType(t), scalars[t] + "3x2", null);
                Register(typeof(Matrix3x3<>).MakeGenericType(t), scalars[t] + "3x3", null);
                Register(typeof(Matrix3x4<>).MakeGenericType(t), scalars[t] + "3x4", null);

                Register(typeof(Matrix4x1<>).MakeGenericType(t), scalars[t] + "4x1", null);
                Register(typeof(Matrix4x2<>).MakeGenericType(t), scalars[t] + "4x2", null);
                Register(typeof(Matrix4x3<>).MakeGenericType(t), scalars[t] + "4x3", null);
                Register(typeof(Matrix4x4<>).MakeGenericType(t), scalars[t] + "4x4", null);
            }

            Register(typeof(Vector1), "float1", null);
            Register(typeof(Vector2), "float2", null);
            Register(typeof(Vector3), "float3", null);
            Register(typeof(Vector4), "float4", null);

            Register(typeof(Matrix1x1), "float1x1", null);
            Register(typeof(Matrix1x2), "float1x2", null);
            Register(typeof(Matrix1x3), "float1x3", null);
            Register(typeof(Matrix1x4), "float1x4", null);

            Register(typeof(Matrix2x1), "float2x1", null);
            Register(typeof(Matrix2x2), "float2x2", null);
            Register(typeof(Matrix2x3), "float2x3", null);
            Register(typeof(Matrix2x4), "float2x4", null);

            Register(typeof(Matrix3x1), "float3x1", null);
            Register(typeof(Matrix3x2), "float3x2", null);
            Register(typeof(Matrix3x3), "float3x3", null);
            Register(typeof(Matrix3x4), "float3x4", null);

            Register(typeof(Matrix4x1), "float4x1", null);
            Register(typeof(Matrix4x2), "float4x2", null);
            Register(typeof(Matrix4x3), "float4x3", null);
            Register(typeof(Matrix4x4), "float4x4", null);
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
            this.RegisterAvailableOverloads(mathType, "degrees", "degrees");
            this.RegisterAvailableOverloads(mathType, "determinant", "determinant");
            this.RegisterAvailableOverloads(mathType, "distance", "distance");
            this.RegisterAvailableOverloads(mathType, "dot", "dot");
            this.RegisterAvailableOverloads(mathType, "exp", "exp");
            this.RegisterAvailableOverloads(mathType, "exp2", "exp2");
            this.RegisterAvailableOverloads(mathType, "floor", "floor");
            this.RegisterAvailableOverloads(mathType, "fmod", "fmod");
            this.RegisterAvailableOverloads(mathType, "frac", "frac");
            this.RegisterAvailableOverloads(mathType, "isfinite", "isfinite");
            this.RegisterAvailableOverloads(mathType, "isinf", "isinf");
            this.RegisterAvailableOverloads(mathType, "isnan", "isnan");

            this.RegisterAvailableOverloads(mathType, "ldexp", "ldexp");
            this.RegisterAvailableOverloads(mathType, "length", "length");
            this.RegisterAvailableOverloads(mathType, "lerp", "lerp");
            this.RegisterAvailableOverloads(mathType, "lit", "lit");

            this.RegisterAvailableOverloads(mathType, "log", "log");
            this.RegisterAvailableOverloads(mathType, "log10", "log10");
            this.RegisterAvailableOverloads(mathType, "log2", "log2");
            this.RegisterAvailableOverloads(mathType, "max", "max");
            this.RegisterAvailableOverloads(mathType, "min", "min");
            this.RegisterAvailableOverloads(mathType, "mul", "mul");
            this.RegisterAvailableOverloads(mathType, "normalize", "normalize");
            this.RegisterAvailableOverloads(mathType, "pow", "pow");
            this.RegisterAvailableOverloads(mathType, "radians", "radians");
            this.RegisterAvailableOverloads(mathType, "reflect", "reflect");
            this.RegisterAvailableOverloads(mathType, "refract", "refract");
            this.RegisterAvailableOverloads(mathType, "round", "round");
            this.RegisterAvailableOverloads(mathType, "rsqrt", "rsqrt");
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
            this.RegisterAvailableOverloads(typeof(Sampler1D), "Sample", "tex1D");
            this.RegisterAvailableOverloads(typeof(Sampler2D), "Sample", "tex2D");
            this.RegisterAvailableOverloads(typeof(Sampler2D), "SampleLOD", "tex2Dlod");
            this.RegisterAvailableOverloads(typeof(Sampler3D), "Sample", "tex3D");
            this.RegisterAvailableOverloads(typeof(SamplerCube), "Sample", "texCUBE");
            #endregion

            EndInitialization();
        }

        public override IEnumerable<string> Keywords
        {
            get
            {
                yield return "main";
                yield return "sampler";
                yield return "struct";
                yield return "for";
                yield return "if";
                yield return "while";
                yield return "return";
                yield return "else";
                yield return "in";
                yield return "out";
                yield return "inout";
                yield return "Sampler";
                yield return "Sampler";
                yield return "Sampler";
                yield return "sampler_state";
                yield return "Sampler";
                yield return "PixelShader";
                yield return "VertexShader";
                yield return "GeometryShader";
                yield return "HullShader";
                yield return "DomainShader";
                yield return "ComputeShader";

            }
        }
    }
}
