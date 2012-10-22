using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;
using System.Rendering.RenderStates;
using System.Rendering.Effects;
using System.Rendering.Effects.Shaders;
using System.Compilers.Shaders;

namespace System.Rendering.Effects
{
    public class SkinnedEffect : VertexShaderEffect<SkinnedEffect.SkinnedVertex, SkinnedEffect.VertexOut>
    {
        [ShaderType]
        public struct SkinnedVertex
        {
            [Position]
            public Vector3 Position;
            [Normal]
            public Vector3 Normal;

            [Weight(0)]
            public float Weight0;
            [Weight(1)]
            public float Weight1;
            [Weight(2)]
            public float Weight2;
            [Weight(3)]
            public float Weight3;

            [Index(0)]
            public float Index0;
            [Index(1)]
            public float Index1;
            [Index(2)]
            public float Index2;
            [Index(3)]
            public float Index3;
            //[Weight(4)]
            //public float Weight4;
            //[Weight(5)]
            //public float Weight5;
            //[Weight(6)]
            //public float Weight6;
            //[Weight(7)]
            //public float Weight7;
            //[Weight(8)]
            //public float Weight8;
            //[Weight(9)]
            //public float Weight9;
            //[Weight(10)]
            //public float Weight10;
            //[Weight(11)]
            //public float Weight11;
            //[Weight(12)]
            //public float Weight12;
            //[Weight(13)]
            //public float Weight13;
            //[Weight(14)]
            //public float Weight14;
            //[Weight(15)]
            //public float Weight15;
            //[Weight(16)]
            //public float Weight16;
            //[Weight(17)]
            //public float Weight17;
            //[Weight(18)]
            //public float Weight18;

            public static SkinnedVertex Create(Vector3 position, Vector3 normal, params Tuple<float, int>[] weights)
            {
                SkinnedVertex v = new SkinnedVertex();
                v.Position = position;
                v.Normal = normal;

                v.Weight0 = (weights.Length > 0) ? weights[0].Item1 : 0;
                v.Weight1 = (weights.Length > 1) ? weights[1].Item1 : 0;
                v.Weight2 = (weights.Length > 2) ? weights[2].Item1 : 0;
                v.Weight3 = (weights.Length > 3) ? weights[3].Item1 : 0;

                v.Index0 = (weights.Length > 0) ? weights[0].Item2 : 0;
                v.Index1 = (weights.Length > 1) ? weights[1].Item2 : 0;
                v.Index2 = (weights.Length > 2) ? weights[2].Item2 : 0;
                v.Index3 = (weights.Length > 3) ? weights[3].Item2 : 0;

                //v.Weight4 = (weights.Length > 4) ? weights[4] : 0;
                //v.Weight5 = (weights.Length > 5) ? weights[5] : 0;
                //v.Weight6 = (weights.Length > 6) ? weights[6] : 0;
                //v.Weight7 = (weights.Length > 7) ? weights[7] : 0;
                //v.Weight8 = (weights.Length > 8) ? weights[8] : 0;
                //v.Weight9 = (weights.Length > 9) ? weights[9] : 0;
                //v.Weight10 = (weights.Length > 10) ? weights[10] : 0;
                //v.Weight11 = (weights.Length > 11) ? weights[11] : 0;
                //v.Weight12 = (weights.Length > 12) ? weights[12] : 0;
                //v.Weight13 = (weights.Length > 13) ? weights[13] : 0;
                //v.Weight14 = (weights.Length > 14) ? weights[14] : 0;
                //v.Weight15 = (weights.Length > 15) ? weights[15] : 0;
                //v.Weight16 = (weights.Length > 16) ? weights[16] : 0;
                //v.Weight17 = (weights.Length > 17) ? weights[17] : 0;
                //v.Weight18 = (weights.Length > 18) ? weights[18] : 0;

                return v;
            }
        }

        [ShaderType]
        public struct VertexOut
        {
            [Position]
            public Vector3 Position;
            [Normal]
            public Vector3 Normal;
        }

        public SkinnedEffect()
            : base(null)
        {
            this.state = new VertexShaderState(ShaderSource.From(new Func<SkinnedVertex, VertexOut>(VertexShader)));
        }

        public void SetMatrices(params Matrix4x4[] transforms)
        {
            this.Worlds = transforms;
        }

        [ArrayLength(19)]
        public Matrix4x4[] Worlds;

        private VertexOut VertexShader(SkinnedVertex In)
        {
            var world0 = Worlds[(int)In.Index0];
            var world1 = Worlds[(int)In.Index1];
            var world2 = Worlds[(int)In.Index2];
            var world3 = Worlds[(int)In.Index3];

            Matrix4x4 transform =
                    In.Weight0 * world0
                + In.Weight1 * world1
                + In.Weight2 * world2
                + In.Weight3 * world3;

            return new VertexOut
            {
                Position = (Vector3)GMath.mul(new Vector4(In.Position, 1), transform),
                Normal = GMath.normalize((Vector3)GMath.mul(new Vector4(In.Normal, 0), transform))
            };
        }
    }
}
