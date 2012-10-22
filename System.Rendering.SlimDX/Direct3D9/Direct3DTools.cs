using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3D = global::SlimDX.Direct3D9;
using SlimDX.Direct3D9;
using System.Rendering.Modeling;
using System.Rendering.Resourcing;
using System.Rendering.RenderStates;
using SRE = System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Direct3D9
{
    public static class Direct3D9Tools
    {
        public static PrimitiveType Convert(System.Rendering.Modeling.BasicPrimitiveType primitiveType)
        {
            switch (primitiveType)
            {
                case BasicPrimitiveType.Line_Loop: return PrimitiveType.LineStrip;
                case BasicPrimitiveType.Line_Strip: return PrimitiveType.LineStrip;
                case BasicPrimitiveType.Lines: return PrimitiveType.LineList;
                case BasicPrimitiveType.Points: return PrimitiveType.PointList;
                case BasicPrimitiveType.Triangle_Fan: return PrimitiveType.TriangleFan;
                case BasicPrimitiveType.Triangle_Strip: return PrimitiveType.TriangleStrip;
                case BasicPrimitiveType.Triangles: return PrimitiveType.TriangleList;
                default: throw new NotSupportedException();
            }
        }

        public static SlimDX.Vector2 Convert(Vector2 v)
        {
            return new SlimDX.Vector2(v.X, v.Y);
        }
        public static SlimDX.Vector3 Convert(Vector3 v)
        {
            return new SlimDX.Vector3(v.X, v.Y, v.Z);
        }
        public static SlimDX.Vector4 Convert(Vector4 v)
        {
            return new SlimDX.Vector4(v.X, v.Y, v.Z, v.W);
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

        struct FormatKey
        {
            IEnumerable<DataComponentAttribute> declaration;
            public FormatKey(IEnumerable<DataComponentAttribute> declaration)
            {
                this.declaration = declaration;
            }
            public FormatKey(params DataComponentAttribute[] declaration) : this((IEnumerable<DataComponentAttribute>)declaration) { }

            public override int GetHashCode()
            {
                return declaration.First().GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (!(obj is FormatKey)) return false;
                FormatKey o = (FormatKey)obj;
                return o.declaration.SequenceEqual(this.declaration);
            }
        }

        abstract class FormatAnalizer
        {
            public abstract bool Analize(DataDescription descriptor, out Format internalFormat);

            public static readonly FormatAnalizer RGBA = new RGBAFormat();
            public static readonly FormatAnalizer BGRA = new BGRAFormat();
            public static readonly FormatAnalizer BGR = new BGRFormat();
            public static readonly FormatAnalizer R = new RFormat();
            public static readonly FormatAnalizer RG = new RGFormat();

            class RGBAFormat : FormatAnalizer
            {
                static Dictionary<Tuple<int, int, int, int>, Format> bitformats = new Dictionary<Tuple<int, int, int, int>, Format>
                {
                    {Tuple.Create (8,8,8,8), Format.A8B8G8R8},
                    {Tuple.Create(16,16,16,16), Format.A16B16G16R16},
                    {Tuple.Create(32,32,32,32), Format.A32B32G32R32F},
                    {Tuple.Create(10,10,10,2), Format.A2B10G10R10}
                };

                public override bool Analize(DataDescription descriptor, out Format internalFormat)
                {
                    internalFormat = (Format)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = Format.A32B32G32R32F;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = Format.A8B8G8R8;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = Format.A16B16G16R16;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = Format.A32B32G32R32F;
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
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class BGRAFormat : FormatAnalizer
            {
                static Dictionary<Tuple<int, int, int, int>, Format> bitformats = new Dictionary<Tuple<int, int, int, int>, Format>
                {
                    {Tuple.Create (8,8,8,8), Format.A8R8G8B8},
                    {Tuple.Create (2,2,2,2), Format.Multi2Argb8},
                    {Tuple.Create(4,4,4,4), Format.A4R4G4B4 },
                    {Tuple.Create(10,10,10,2), Format.A2R10G10B10},
                    {Tuple.Create(5,5,5,1), Format.A1R5G5B5}
                };

                public override bool Analize(DataDescription descriptor, out Format internalFormat)
                {
                    internalFormat = (Format)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = Format.A32B32G32R32F;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = Format.A8B8G8R8;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = Format.A16B16G16R16;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = Format.A32B32G32R32F;
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
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class ABGRFormat : FormatAnalizer
            {
                static Dictionary<Tuple<int, int, int, int>, Format> bitformats = new Dictionary<Tuple<int, int, int, int>, Format>
                {
                    {Tuple.Create (8,8,8,8), Format.A8R8G8B8},
                    {Tuple.Create (2,2,2,2), Format.Multi2Argb8},
                    {Tuple.Create(4,4,4,4), Format.A4R4G4B4 },
                    {Tuple.Create(16,16,16,16), Format.A16B16G16R16},
                    {Tuple.Create(32,32,32,32), Format.A32B32G32R32F},
                    {Tuple.Create(10,10,10,2), Format.A2R10G10B10},
                    {Tuple.Create(5,5,5,1), Format.A1R5G5B5}
                };

                public override bool Analize(DataDescription descriptor, out Format internalFormat)
                {
                    internalFormat = (Format)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = Format.A32B32G32R32F;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = Format.A8B8G8R8;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = Format.A16B16G16R16;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = Format.A32B32G32R32F;
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
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class BGRFormat : FormatAnalizer
            {
                static Dictionary<Tuple<int, int, int>, Format> bitformats = new Dictionary<Tuple<int, int, int>, Format>
                {
                    {Tuple.Create (8,8,8), Format.R8G8B8},
                    {Tuple.Create (2,2,2), Format.Multi2Argb8},
                    {Tuple.Create(4,4,4), Format.X4R4G4B4},
                    {Tuple.Create(16,16,16), Format.A16B16G16R16},
                    {Tuple.Create(32,32,32), Format.A32B32G32R32F},
                    {Tuple.Create(10,10,10), Format.A2B10G10R10},
                    {Tuple.Create(5,5,5), Format.X1R5G5B5}
                };

                public override bool Analize(DataDescription descriptor, out Format internalFormat)
                {
                    internalFormat = (Format)0;

                    if (descriptor.IsByteOrder) // is byte order
                    {
                        var firstType = descriptor.Declaration.First().BasicType;

                        if (!descriptor.Declaration.All(d => d.BasicType == firstType))
                            return false;

                        switch (descriptor.Declaration.First().BasicType)
                        {
                            case TypeCode.Single:
                                internalFormat = Format.A32B32G32R32F;
                                return true;
                            case TypeCode.Byte:
                            case TypeCode.SByte:
                                internalFormat = Format.R8G8B8;
                                return true;
                            case TypeCode.Int16:
                            case TypeCode.UInt16:
                                internalFormat = Format.A16B16G16R16;
                                return true;
                            case TypeCode.Int32:
                            case TypeCode.UInt32:
                                internalFormat = Format.A32B32G32R32F;
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
                            return true;
                        }
                        else
                            return false;
                    }
                }
            }

            class RFormat : FormatAnalizer
            {
                public override bool Analize(DataDescription descriptor, out Format internalFormat)
                {
                    switch (descriptor.TypeSize)
                    {
                        case 16: internalFormat = Format.R16F;
                            return true;
                        case 32: internalFormat = Format.R32F;
                            return true;
                        default:
                            internalFormat = (Format)0;
                            return false;
                    }
                }
            }

            class RGFormat : FormatAnalizer
            {
                public override bool Analize(DataDescription descriptor, out Format internalFormat)
                {
                    internalFormat = (Format)0;

                    var rd = descriptor.Declaration.First();
                    var gd = descriptor.Declaration.Last();

                    switch (descriptor.TypeSize)
                    {
                        case 16:
                            if ((rd.SizeInBits == 8 || rd.Size == 1) && (gd.SizeInBits == 8 || gd.Size == 1))
                            {
                                internalFormat = Format.V8U8;
                                return true;
                            }
                            else
                                return false;
                        case 32:
                            if ((rd.SizeInBits == 16 || rd.Size == 2) && (gd.SizeInBits == 16 || gd.Size == 2))
                            {
                                internalFormat = Format.G16R16;
                                return true;
                            }
                            else
                                return false;
                        case 64:
                            if ((rd.SizeInBits == 32 || rd.Size == 4) && (gd.SizeInBits == 32 || gd.Size == 4))
                            {
                                internalFormat = Format.G32R32F;
                                return true;
                            }
                            else
                                return false;
                        default:
                            return false;
                    }
                }
            }
        }

        static Dictionary<FormatKey, FormatAnalizer> texelFormatAnalizers = new Dictionary<FormatKey, FormatAnalizer>()
        {
            { 
                new FormatKey(new RedAttribute(), new GreenAttribute(), new BlueAttribute(), new AlphaAttribute()),
                FormatAnalizer.RGBA 
            },
            { 
                new FormatKey(new BlueAttribute(), new GreenAttribute(), new RedAttribute(), new AlphaAttribute()),
                FormatAnalizer.BGRA 
            },
            {
                new FormatKey(new BlueAttribute(), new GreenAttribute(), new RedAttribute()),
                FormatAnalizer.BGR
            },
            {
                new FormatKey(new RedAttribute()),
                FormatAnalizer.R
            },
            {
                new FormatKey(new RedAttribute(),new GreenAttribute()),
                FormatAnalizer.RG
            }
        };

        public static Format GetPixelFormat(Type pixelType)
        {
            var descriptor = DataComponentExtensors.GetDescriptor(pixelType);
            var dec = descriptor.Declaration.Select(d => d.Semantic).ToArray();
            var formatKey = new FormatKey(dec);

            if (texelFormatAnalizers.ContainsKey(formatKey))
            {
                var f = (Format)0;
                if (texelFormatAnalizers[formatKey].Analize(descriptor, out f))
                    return f;
            }

            throw new NotSupportedException("Type " + pixelType + " can not be used as texel type.");
        }

        static Dictionary<int, VertexElement[]> declarations = new Dictionary<int, VertexElement[]>();
        static Dictionary<int, int> strides = new Dictionary<int, int>();
        static Dictionary<int, DataDescription> descriptors = new Dictionary<int, DataDescription>();

        static DeclarationType GetDeclarationType(DataComponentDescription des)
        {
            switch (des.Components)
            {
                case 1:
                    switch (des.BasicType)
                    {
                        case TypeCode.UInt32:
                        case TypeCode.Int32:
                            return DeclarationType.Color;
                    }
                    break;
                case 2:
                    switch (des.BasicType)
                    {
                        case TypeCode.Int16:
                            return DeclarationType.Short2;
                        case TypeCode.Single:
                            return DeclarationType.Float2;
                    }
                    break;
                case 3:
                    switch (des.BasicType)
                    {
                        case TypeCode.Single:
                            return DeclarationType.Float3;
                        case TypeCode.Int32:
                            return DeclarationType.Dec3N;
                        case TypeCode.Byte:
                            return DeclarationType.UDec3;
                    }
                    break;
                case 4:
                    switch (des.BasicType)
                    {
                        case TypeCode.Byte:
                            return DeclarationType.Ubyte4;
                        case TypeCode.Int16:
                            return DeclarationType.Short4;
                        case TypeCode.Int32:
                            return DeclarationType.Color;
                        case TypeCode.Single:
                            return DeclarationType.Float4;
                    }
                    break;
            }

            throw new NotSupportedException("Specific type of vertex element is not supported " + des.BasicType + " " + des.Components);
        }
        static DeclarationUsage GetDeclarationUsage(DataComponentDescription des, out int index)
        {
            index = des.Semantic is IndexedComponentAttribute ?
                ((IndexedComponentAttribute)des.Semantic).Index :0;
            
            if (des.Semantic is PositionAttribute || des.Semantic is ProjectedAttribute)
                return DeclarationUsage.Position;
            if (des.Semantic is NormalAttribute)
                return DeclarationUsage.Normal;
            if (des.Semantic is CoordinatesAttribute)
                return DeclarationUsage.TextureCoordinate;
            if (des.Semantic is ColorAttribute)
                return DeclarationUsage.Color;

            throw new NotSupportedException("Usage " + des.Semantic + " is not supported");
        }

        public static VertexElement[] GetVertexDeclaration(Type vertexType, out int stride, out DataDescription description)
        {
            int token = vertexType.MetadataToken;
            if (!declarations.ContainsKey(token))
            {
                DataDescription descriptor = DataComponentExtensors.GetDescriptor(vertexType);

                List<VertexElement> elements = new List<VertexElement>();

                int s = 0;

                foreach (var des in descriptor.Declaration)
                {
                    int start = des.Offset, size = des.Size;
                    s = Math.Max(s, start + size);
                    var sem = des.Semantic;
                    DeclarationType decType = GetDeclarationType(des);
                    int index;
                    DeclarationUsage usage = GetDeclarationUsage(des, out index);
                    elements.Add(new VertexElement(0, (short)start, decType, DeclarationMethod.Default, usage, (byte)index));
                }

                elements.Add(VertexElement.VertexDeclarationEnd);

                declarations.Add(token, elements.ToArray());
                descriptors.Add(token, descriptor);
                strides.Add(token, s);
            }

            stride = strides[token];
            description = descriptors[token];
            return declarations[token];
        }

        public static D3D.Compare FromCompare(SRE.Compare compare)
        {
            switch (compare)
            {
                case SRE.Compare.Always: return D3D.Compare.Always;
                case SRE.Compare.Equal: return D3D.Compare.Equal;
                case SRE.Compare.Greater: return D3D.Compare.Greater;
                case SRE.Compare.GreaterEqual: return D3D.Compare.GreaterEqual;
                case SRE.Compare.Less: return D3D.Compare.Less;
                case SRE.Compare.LessEqual: return D3D.Compare.LessEqual;
                case SRE.Compare.Never: return D3D.Compare.Never;
                case SRE.Compare.NotEqual: return D3D.Compare.NotEqual;
            }
            return D3D.Compare.Always;
        }

        public static SRE.Compare ToCompare(D3D.Compare compare)
        {
            switch (compare)
            {
                case D3D.Compare.Always: return SRE.Compare.Always;
                case D3D.Compare.Equal: return SRE.Compare.Equal;
                case D3D.Compare.Greater: return SRE.Compare.Greater;
                case D3D.Compare.GreaterEqual: return SRE.Compare.GreaterEqual;
                case D3D.Compare.Less: return SRE.Compare.Less;
                case D3D.Compare.LessEqual: return SRE.Compare.LessEqual;
                case D3D.Compare.Never: return SRE.Compare.Never;
                case D3D.Compare.NotEqual: return SRE.Compare.NotEqual;
            }
            return SRE.Compare.Always;
        }

        public static D3D.TextureFilter SwitchTextureFilterType(TextureFilterType type)
        {
            switch (type)
            {
                case TextureFilterType.Linear: return D3D.TextureFilter.Linear;
                case TextureFilterType.None: return D3D.TextureFilter.None;
                case TextureFilterType.Point: return D3D.TextureFilter.Point;
                case TextureFilterType.Anisotropic: return D3D.TextureFilter.Anisotropic;
                case TextureFilterType.GaussianQuad: return D3D.TextureFilter.GaussianQuad;
                case TextureFilterType.PyramidalQuad: return D3D.TextureFilter.PyramidalQuad;
                default: return D3D.TextureFilter.Linear;
            }
        }

        public static D3D.TextureAddress SwitchTextureAddress(System.Rendering.TextureAddress address)
        {
            switch (address)
            {
                case System.Rendering.TextureAddress.Border: return D3D.TextureAddress.Border;
                case System.Rendering.TextureAddress.Clamp: return D3D.TextureAddress.Clamp;
                case System.Rendering.TextureAddress.Mirror: return D3D.TextureAddress.Mirror;
                case System.Rendering.TextureAddress.MirrorOnce: return D3D.TextureAddress.MirrorOnce;
                case System.Rendering.TextureAddress.Wrap: return D3D.TextureAddress.Wrap;

                default: return D3D.TextureAddress.Wrap;
            }
        }

        public static void SetTextureSampler(Device device, ISampler sampler, int textureIndex)
        {
            var filter = sampler.Filters;
            var addresses = sampler.Addresses;

            device.SetSamplerState(textureIndex, D3D.SamplerState.BorderColor, addresses.BorderColor.ToARGB());

            device.SetSamplerState(textureIndex, D3D.SamplerState.MagFilter, Direct3D9Tools.SwitchTextureFilterType(filter.Magnification));
            //device.SamplerState[textureIndex].MaxAnisotropy = (int)sampler.MaxAnisotropy;
            device.SetSamplerState(textureIndex, D3D.SamplerState.MinFilter, Direct3D9Tools.SwitchTextureFilterType(filter.Minification));
            device.SetSamplerState(textureIndex, D3D.SamplerState.MipFilter, Direct3D9Tools.SwitchTextureFilterType(filter.MipMapping));

            device.SetSamplerState(textureIndex, D3D.SamplerState.MipMapLodBias, sampler.MipmapLODBias);

            device.SetSamplerState(textureIndex, D3D.SamplerState.AddressU, Direct3D9Tools.SwitchTextureAddress(addresses.U));
            device.SetSamplerState(textureIndex, D3D.SamplerState.AddressV, Direct3D9Tools.SwitchTextureAddress(addresses.V));
            device.SetSamplerState(textureIndex, D3D.SamplerState.AddressW, Direct3D9Tools.SwitchTextureAddress(addresses.W));
        }

        public static R GetInternalResource<R, GR>(this Direct3DRender render, GR resource) where R : SlimDX.Direct3D9.Resource where GR : GraphicResource
        {
            var man = ((Direct3DRender.Direct3DResourcesManager)render.ResourcesManager).GetManagerFor<GR>(resource);
            return (R)((Direct3DRender.ID3D9ResourceManager)man).Resource;
        }
    }
}