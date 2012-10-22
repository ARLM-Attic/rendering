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
using System.Drawing;
using System.Rendering.Modeling;
using System.Rendering.Resourcing;
using System.Maths;
using OpenTK.Graphics.OpenGL;

namespace System.Rendering.OpenGL
{
    public class OpenGLTools
    {
        public static float[] VectorToFv(Vector4<float> vec)
        {
            return new float[] { (float)vec.X, (float)vec.Y, (float)vec.Z, (float)vec.W };
        }

        public static float[] VectorToFv(Vector3 vec)
        {
            return new float[] { (float)vec.X, (float)vec.Y, (float)vec.Z, 1 };
        }

        public static Color FromVector(Vector4 vec)
        {
            return Color.FromArgb((int)(vec.W * 255), (int)(vec.X * 255), (int)(vec.Y * 255), (int)(vec.Z * 255));
        }
        public static Vector4 FromColor(Color color)
        {
            return new Vector4(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }

        public static BeginMode Convert(BasicPrimitiveType type)
        {
            switch (type)
            {
                case BasicPrimitiveType.Line_Loop: return BeginMode.LineLoop;
                case BasicPrimitiveType.Line_Strip: return BeginMode.LineStrip;
                case BasicPrimitiveType.Lines: return BeginMode.Lines;
                case BasicPrimitiveType.Points: return BeginMode.Points;
                case BasicPrimitiveType.Triangle_Fan: return BeginMode.TriangleFan;
                case BasicPrimitiveType.Triangle_Strip: return BeginMode.TriangleStrip;
                case BasicPrimitiveType.Triangles: return BeginMode.Triangles;
            }
            return BeginMode.Triangles;
        }

        public static DataType GetBaseType(Type type)
        {
            return GetBaseType(Type.GetTypeCode(type));
        }

        public static DataType GetBaseType(TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean: return DataType.Byte;
                case TypeCode.Byte: return DataType.UnsignedByte;
                case TypeCode.Double: return DataType.Double;
                case TypeCode.Int16: return DataType.Short;
                case TypeCode.Int32: return DataType.Int;
                case TypeCode.Single: return DataType.Float;
                case TypeCode.UInt16: return DataType.UnsignedShort;
                case TypeCode.UInt32: return DataType.UnsignedInt;
                default: throw new NotSupportedException();
            }
        }

        public static int PrimitiveCount(int numberOfVertex, BasicPrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case BasicPrimitiveType.Line_Loop: return numberOfVertex - 1;
                case BasicPrimitiveType.Line_Strip: return numberOfVertex - 1;
                case BasicPrimitiveType.Lines: return numberOfVertex / 2;
                case BasicPrimitiveType.Points: return numberOfVertex;
                case BasicPrimitiveType.Triangle_Fan: return numberOfVertex - 2;
                case BasicPrimitiveType.Triangle_Strip: return numberOfVertex - 2;
                case BasicPrimitiveType.Triangles: return numberOfVertex / 3;
                default: return 0;
            }
        }

        struct Format{
            IEnumerable<DataComponentAttribute> declaration;
            public Format(IEnumerable<DataComponentAttribute> declaration)
            {
                this.declaration = declaration;
            }
            public Format(params DataComponentAttribute[] declaration) : this((IEnumerable<DataComponentAttribute>)declaration) { }

            public override int GetHashCode()
            {
                return declaration.First().GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Format)) return false;
                Format o = (Format)obj;
                return o.declaration.SequenceEqual(this.declaration);
            }
        }

        abstract class FormatAnalizer
        {
            public abstract PixelFormat Format { get; }

            public abstract bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType);

            public static readonly FormatAnalizer RGBA = new RGBAFormat();
            public static readonly FormatAnalizer BGRA = new BGRAFormat();
            public static readonly FormatAnalizer BGR = new BGRFormat();
            public static readonly FormatAnalizer GR = new GRFormat();
            public static readonly FormatAnalizer RG = new RGFormat();
            public static readonly FormatAnalizer R = new RFormat();

            class RGBAFormat : FormatAnalizer
            {
                public override PixelFormat Format
                {
                    get { return PixelFormat.Rgba; }
                }

                static Dictionary<Tuple<int, int, int, int>, PixelInternalFormat> bitformats = new Dictionary<Tuple<int, int, int, int>, PixelInternalFormat>
                {
                    {Tuple.Create (8,8,8,8), PixelInternalFormat.Rgba8},
                    {Tuple.Create (2,2,2,2), PixelInternalFormat.Rgba2},
                    {Tuple.Create(4,4,4,4), PixelInternalFormat.Rgba4 },
                    {Tuple.Create(16,16,16,16), PixelInternalFormat.Rgba16},
                    {Tuple.Create(32,32,32,32), PixelInternalFormat.Rgba32ui},
                    {Tuple.Create(10,10,10,2), PixelInternalFormat.Rgb10A2},
                    {Tuple.Create(5,5,5,1), PixelInternalFormat.Rgb5A1},
                    {Tuple.Create(9,9,9,5), PixelInternalFormat.Rgb9E5}
                };

                public override bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType)
                {
                    internalFormat = (PixelInternalFormat)0;
                    dataType = (DataType)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = PixelInternalFormat.Rgba32f;
                                dataType = DataType.Float;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = PixelInternalFormat.Rgba8ui;
                                dataType = DataType.UnsignedByte;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = PixelInternalFormat.Rgba16ui;
                                dataType = DataType.UnsignedShort;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = PixelInternalFormat.Rgba32ui;
                                dataType = DataType.UnsignedInt;
                                return true;
                            default:
                                return false;
                        }
                    }
                    else
                    {
                        var texels = descriptor.Declaration.Select(d => d.SizeInBits).ToArray();
                        var tuple = Tuple.Create(texels[0], texels[1], texels[2], texels[3]);
                        if (bitformats.ContainsKey(tuple))
                        {
                            internalFormat = bitformats[tuple];
                            dataType = DataType.UnsignedByte;
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class BGRAFormat : FormatAnalizer
            {
                public override PixelFormat Format
                {
                    get { return PixelFormat.Bgra; }
                }

                static Dictionary<Tuple<int, int, int, int>, PixelInternalFormat> bitformats = new Dictionary<Tuple<int, int, int, int>, PixelInternalFormat>
                {
                    {Tuple.Create (8,8,8,8), PixelInternalFormat.Rgba8},
                    {Tuple.Create (2,2,2,2), PixelInternalFormat.Rgba2},
                    {Tuple.Create(4,4,4,4), PixelInternalFormat.Rgba4 },
                    {Tuple.Create(16,16,16,16), PixelInternalFormat.Rgba16},
                    {Tuple.Create(32,32,32,32), PixelInternalFormat.Rgba32ui},
                    {Tuple.Create(10,10,10,2), PixelInternalFormat.Rgb10A2},
                    {Tuple.Create(5,5,5,1), PixelInternalFormat.Rgb5A1},
                    {Tuple.Create(9,9,9,5), PixelInternalFormat.Rgb9E5}
                };

                public override bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType)
                {
                    internalFormat = (PixelInternalFormat)0;
                    dataType = (DataType)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = PixelInternalFormat.Rgba32f;
                                dataType = DataType.Float;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = PixelInternalFormat.Rgba8ui;
                                dataType = DataType.UnsignedByte;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = PixelInternalFormat.Rgba16ui;
                                dataType = DataType.UnsignedShort;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = PixelInternalFormat.Rgba32ui;
                                dataType = DataType.UnsignedInt;
                                return true;
                            default:
                                return false;
                        }
                    }
                    else 
                    {
                        var texels = descriptor.Declaration.Select(d => d.SizeInBits).ToArray();
                        var tuple = Tuple.Create(texels[0], texels[1], texels[2], texels[3]);
                        if (bitformats.ContainsKey(tuple))
                        {
                            internalFormat = bitformats[tuple];
                            dataType = DataType.UnsignedByte;
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class BGRFormat : FormatAnalizer
            {
                public override PixelFormat Format
                {
                    get { return PixelFormat.Bgr; }
                }

                static Dictionary<Tuple<int, int, int>, PixelInternalFormat> bitformats = new Dictionary<Tuple<int, int, int>, PixelInternalFormat>
                {
                    {Tuple.Create (8,8,8), PixelInternalFormat.Rgb8},
                    {Tuple.Create (2,2,2), PixelInternalFormat.Rgb2Ext},
                    {Tuple.Create(4,4,4), PixelInternalFormat.Rgb4},
                    {Tuple.Create(16,16,16), PixelInternalFormat.Rgb16},
                    {Tuple.Create(32,32,32), PixelInternalFormat.Rgb32ui},
                    {Tuple.Create(10,10,10), PixelInternalFormat.Rgb10},
                    {Tuple.Create(5,5,5), PixelInternalFormat.Rgb5}
                };

                public override bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType)
                {
                    internalFormat = (PixelInternalFormat)0;
                    dataType = (DataType)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = PixelInternalFormat.Rgb32f;
                                dataType = DataType.Float;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = PixelInternalFormat.Rgb8ui;
                                dataType = DataType.UnsignedByte;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = PixelInternalFormat.Rgb16ui;
                                dataType = DataType.UnsignedShort;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = PixelInternalFormat.Rgb32ui;
                                dataType = DataType.UnsignedInt;
                                return true;
                            default:
                                return false;
                        }
                    }
                    else
                    {
                        var texels = descriptor.Declaration.Select(d => d.SizeInBits).ToArray();
                        var tuple = Tuple.Create(texels[0], texels[1], texels[2]);
                        if (bitformats.ContainsKey(tuple))
                        {
                            internalFormat = bitformats[tuple];
                            dataType = DataType.UnsignedByte;
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class GRFormat : FormatAnalizer
            {
                public override PixelFormat Format
                {
                    get { return PixelFormat.Rg; }
                }

                public override bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType)
                {
                    var rd = descriptor.Declaration.First();
                    var gd = descriptor.Declaration.Last();

                    internalFormat = (PixelInternalFormat)0;
                    dataType = (DataType)0;

                    switch (descriptor.TypeSize)
                    {
                        case 16:
                            if ((rd.SizeInBits == 8 || rd.Size == 1) && (gd.SizeInBits == 8 || gd.Size == 1))
                            {
                                internalFormat = PixelInternalFormat.Rg8;
                                dataType = DataType.UnsignedByte;
                                return true;
                            }
                            else
                                return false;
                        case 32:
                            if ((rd.SizeInBits == 16 || rd.Size == 2) && (gd.SizeInBits == 16 || gd.Size == 2))
                            {
                                internalFormat = PixelInternalFormat.Rg16;
                                dataType = DataType.UnsignedByte;
                                return true;
                            }
                            else
                                return false;
                        case 64:
                            if ((rd.SizeInBits == 32 || rd.Size == 4) && (gd.SizeInBits == 32 || gd.Size == 4))
                            {
                                if (rd.BasicType == TypeCode.Single)
                                {
                                    internalFormat = PixelInternalFormat.Rg32f;
                                    dataType = DataType.Float;
                                }
                                else
                                {
                                    internalFormat = PixelInternalFormat.Rg32ui;
                                    dataType = DataType.UnsignedInt;
                                }
                                return true;
                            }
                            else
                                return false;
                        default:
                            return false;
                    }
                }
            }

            class RGFormat : FormatAnalizer
            {
                public override PixelFormat Format
                {
                    get { return PixelFormat.Rg; }
                }

                public override bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType)
                {
                    var rd = descriptor.Declaration.First();
                    var gd = descriptor.Declaration.Last();

                    internalFormat = (PixelInternalFormat)0;
                    dataType = (DataType)0;

                    switch (descriptor.TypeSize)
                    {
                        case 16:
                            if ((rd.SizeInBits == 8 || rd.Size == 1) && (gd.SizeInBits == 8 || gd.Size == 1))
                            {
                                internalFormat = PixelInternalFormat.Rg8;
                                dataType = DataType.UnsignedByte;
                                return true;
                            }
                            else
                                return false;
                        case 32:
                            if ((rd.SizeInBits == 16 || rd.Size == 2) && (gd.SizeInBits == 16 || gd.Size == 2))
                            {
                                internalFormat = PixelInternalFormat.Rg16f;
                                dataType = DataType.Float;
                                return true;
                            }
                            else
                                return false;
                        case 64:
                            if ((rd.SizeInBits == 32 || rd.Size == 4) && (gd.SizeInBits == 32 || gd.Size == 4))
                            {
                                if (rd.BasicType == TypeCode.Single)
                                {
                                    internalFormat = PixelInternalFormat.Rg32f;
                                    dataType = DataType.Float;
                                }
                                else
                                {
                                    internalFormat = PixelInternalFormat.Rg32ui;
                                    dataType = DataType.UnsignedInt;
                                }
                                return true;
                            }
                            else
                                return false;
                        default:
                            return false;
                    }
                }
            }


            class RFormat : FormatAnalizer
            {
                public override PixelFormat Format
                {
                    get { return PixelFormat.Red; }
                }

                public override bool Analize(DataDescription descriptor, out PixelInternalFormat internalFormat, out DataType dataType)
                {
                    internalFormat = (PixelInternalFormat)0;
                    dataType = (DataType)0;

                    switch (descriptor.TypeSize)
                    {
                        case 8: internalFormat = PixelInternalFormat.R8;
                            dataType = DataType.Byte;
                            return true;
                        case 16:
                            internalFormat = PixelInternalFormat.R16;
                            dataType = DataType.Byte;
                            return true;
                        case 32:
                            if (descriptor.Declaration.First().BasicType == TypeCode.Single)
                            {
                                internalFormat = PixelInternalFormat.R32f;
                                dataType = DataType.Float;
                            }
                            else
                            {
                                internalFormat = PixelInternalFormat.R32ui;
                                dataType = DataType.UnsignedInt;
                            }
                            return true;

                        default:
                            return false;
                    }
                }
            }
        }

        static Dictionary<Format, FormatAnalizer> texelFormatAnalizers = new Dictionary<Format, FormatAnalizer>()
        {
            { 
                new Format(new RedAttribute(), new GreenAttribute(), new BlueAttribute(), new AlphaAttribute()),
                FormatAnalizer.RGBA 
            },
            { 
                new Format(new BlueAttribute(), new GreenAttribute(), new RedAttribute(), new AlphaAttribute()),
                FormatAnalizer.BGRA 
            },
            {
                new Format(new BlueAttribute(), new GreenAttribute(), new RedAttribute()),
                FormatAnalizer.BGR
            },
            {
                new Format(new GreenAttribute(),new RedAttribute()),
                FormatAnalizer.GR
            },
            {
                new Format(new RedAttribute(),new GreenAttribute()),
                FormatAnalizer.RG
            },
            {
                new Format(new RedAttribute()),
                FormatAnalizer.R
                }
        };

        public static bool GetPixelFormat(DataDescription descriptor, out PixelInternalFormat internalFormat, out PixelFormat pixelFormat, out DataType componentType)
        {
            internalFormat = (PixelInternalFormat)0;
            pixelFormat = (PixelFormat)0;
            componentType = (DataType)0;

            Format format = new Format(descriptor.Declaration.Select(d=>d.Semantic));
            if (!texelFormatAnalizers.ContainsKey(format))
                return false;

            FormatAnalizer analizer = texelFormatAnalizers[format];
            if (!analizer.Analize(descriptor, out internalFormat, out componentType))
                return false;
            pixelFormat = analizer.Format;
            return true;
        }

        static TextureWrapMode SwitchTextureAddress(TextureAddress address)
        {
            switch (address)
            {
                case TextureAddress.Border: return TextureWrapMode.ClampToBorder;
                case TextureAddress.Clamp: return TextureWrapMode.Clamp;
                case TextureAddress.Mirror: return TextureWrapMode.MirroredRepeat;
                case TextureAddress.MirrorOnce: return TextureWrapMode.Repeat;
                case TextureAddress.Wrap: return TextureWrapMode.Repeat;

                default: return TextureWrapMode.Repeat;
            }
        }

        static int SwitchTextureFilterType(TextureFilterType mip, TextureFilterType type)
        {
            //if (mip == TextureFilterType.None)
            switch (type)
            {
                case TextureFilterType.Linear: return (int)TextureMinFilter.Linear;
                case TextureFilterType.None: return (int)(TextureMinFilter)0;
                case TextureFilterType.Point: return (int)TextureMinFilter.Nearest;
                default: return (int)TextureMinFilter.Linear;
            }
            //else
            //    switch (type)
            //    {
            //        case TextureFilterType.Linear: return GL._LINEAR | GL._LINEAR_MIPMAP_LINEAR;
            //        case TextureFilterType.None: return GL._NONE;
            //        case TextureFilterType.Point: return GL._NEAREST_MIPMAP_NEAREST;
            //        default: return GL._LINEAR;
            //    }
        }

        internal static void SetTextureSampler(ISampler sampler, int index)
        {
            var textureTarget = TextureTarget.Texture2D;
            switch (sampler.Rank)
            {
                case 1: textureTarget = TextureTarget.Texture1D; break;
                case 2: textureTarget = TextureTarget.Texture2D; break;
                case 3: textureTarget = TextureTarget.Texture3D; break;
            }
            if (sampler is SamplerCube)
                textureTarget = TextureTarget.TextureCubeMap;

            GL.Enable((EnableCap)textureTarget);

            var filter = sampler.Filters;
            GL.TexParameter(textureTarget, TextureParameterName.TextureMagFilter, (int)SwitchTextureFilterType(sampler.Filters.MipMapping, sampler.Filters.Magnification));
            GL.TexParameter(textureTarget, TextureParameterName.TextureMinFilter, (int)SwitchTextureFilterType(sampler.Filters.MipMapping, sampler.Filters.Magnification));

            var addresses = sampler.Addresses;

            switch (sampler.Rank)
            {
                case 1:
                    GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)SwitchTextureAddress(addresses.U));
                    break;
                case 2:
                    GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)SwitchTextureAddress(addresses.U));
                    GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)SwitchTextureAddress(addresses.V));
                    break;
                case 3:
                    GL.TexParameter(textureTarget, TextureParameterName.TextureWrapS, (int)SwitchTextureAddress(addresses.U));
                    GL.TexParameter(textureTarget, TextureParameterName.TextureWrapT, (int)SwitchTextureAddress(addresses.V));
                    GL.TexParameter(textureTarget, TextureParameterName.TextureWrapR, (int)SwitchTextureAddress(addresses.W));
                    break;
            }
        }
    }
}
