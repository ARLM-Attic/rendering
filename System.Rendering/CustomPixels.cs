using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Maths;

namespace System.Rendering
{
    public static class CustomPixels
    {
        [Alpha(24), Red(16), Green(8), Blue(0)]
        public struct ARGB
        {
            public byte B;
            public byte G;
            public byte R;
            public byte A;

            public ARGB(byte r, byte g, byte b, byte a)
            {
                this.A = a;
                this.R = r;
                this.G = g;
                this.B = b;
            }

            private static byte ScaleToComp(float x)
            {
                return (byte)(Math.Max(0, Math.Min(x, 1)) * 255);
            }

            public ARGB(float sr, float sg, float sb, float sa)
                : this(ScaleToComp(sr), ScaleToComp(sg), ScaleToComp(sb), ScaleToComp(sa))
            {
            }

            public ARGB(uint argb)
            {
                A = (byte)((argb >> 24) & 0x000000FF);
                R = (byte)((argb >> 16) & 0x000000FF);
                G = (byte)((argb >> 8) & 0x000000FF);
                B = (byte)(argb & 0x000000FF);
            }

            public uint Argb
            {
                get { return (uint)(A << 24 | R << 16 | G << 8 | B << 0); }
                set
                {
                    A = (byte)((value >> 24) & 0x000000FF);
                    R = (byte)((value >> 16) & 0x000000FF);
                    G = (byte)((value >> 8) & 0x000000FF);
                    B = (byte)(value & 0x000000FF);
                }
            }
        }

        [Alpha(24), Red(16), Green(8), Blue(0)]
        public struct ARGBtest
        {
            public byte A;
            public byte G;
            public byte B;
            public byte R;
        }

        [Red(24), Green(16), Blue(8), Alpha(0)]
        public struct RGBA
        {

            public byte A;
            public byte B;
            public byte G;
            public byte R;

            public RGBA(byte r, byte g, byte b, byte a)
            {
                this.R = r;
                this.G = g;
                this.B = b;
                this.A = a;
            }

            private static byte ScaleToComp(float x)
            {
                return (byte)(Math.Max(0, Math.Min(x, 1)) * 255);
            }

            public RGBA(float sr, float sg, float sb, float sa)
                : this(ScaleToComp(sr), ScaleToComp(sg), ScaleToComp(sb), ScaleToComp(sa))
            {
            }

            public RGBA(uint rgba)
            {
                R = (byte)((rgba >> 24) & 0x000000FF);
                G = (byte)((rgba >> 16) & 0x000000FF);
                B = (byte)((rgba >> 8) & 0x000000FF);
                A = (byte)(rgba & 0x000000FF);
            }

            public uint Rgba
            {
                get { return (uint)(R << 24 | G << 16 | B << 8 | A << 0); }
                set
                {
                    R = (byte)((value >> 24) & 0x000000FF);
                    G = (byte)((value >> 16) & 0x000000FF);
                    B = (byte)((value >> 8) & 0x000000FF);
                    A = (byte)(value & 0x000000FF);
                }
            }
        }

        [Red(16), Green(8), Blue(0)]
        public struct RGB
        {
            public byte B;
            public byte G;
            public byte R;

            public RGB(byte r, byte g, byte b)
            {
                this.R = r;
                this.G = g;
                this.B = b;
            }
        }

        public struct SRGBA
        {
            [Red]
            public float R;
            [Green]
            public float G;
            [Blue]
            public float B;
            [Alpha]
            public float A;

            public SRGBA(float r, float g, float b, float a)
            {
                this.A = a;
                this.R = r;
                this.G = g;
                this.B = b;
            }
        }

        public struct SRG
        {
            [Red]
            public float R;
            [Green]
            public float G;

            public SRG(float r, float g)
            {
                this.R = r;
                this.G = g;
            }
        }

        public struct ScaledRed
        {
            [Red]
            public float R;

            public ScaledRed(float value)
            {
                this.R = value;
            }
        }

        public struct Depth
        {
            [Red]
            public float Value;

            public Depth(float value)
            {
                this.Value = value;
            }
        }

        public struct BumpTexel
        {
            [X]
            public float Nx;
            [Y]
            public float Ny;
            [Z]
            public float Nz;
            [W]
            public float Height;

            public BumpTexel(Vector3 normal, float height)
            {
                this.Nx = normal.X;
                this.Ny = normal.Y;
                this.Nz = normal.Z;
                this.Height = height;
            }
        }
    }
}
