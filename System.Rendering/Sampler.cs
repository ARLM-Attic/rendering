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
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Maths;
using System.Compilers.Shaders;

namespace System.Rendering
{
    public interface ISampler
    {
        Addressing3D Addresses { get; }

        Filtering Filters { get; }

        TextureBuffer Texture { get; }

        float MipmapLODBias { get; }

        uint MaxMipLevel { get; }

        uint MaxAnisotropy { get; }

        ISampler Clone(TextureBuffer other);

        int Rank { get; }
    }

    [OnlyGlobalDeclaration]
    public struct Sampler1D : ISampler
    {
        Addressing1D _Addresses;
        public Addressing1D Addresses { get { return _Addresses; } set { _Addresses = value; } }

        Addressing3D ISampler.Addresses { get { return new Addressing3D { U = _Addresses.U, BorderColor = _Addresses.BorderColor }; } }

        Filtering _Filters;
        public Filtering Filters { get { return _Filters; } set { _Filters = value; } }

        TextureBuffer _Texture;
        public TextureBuffer Texture { get { return _Texture; } private set { _Texture = value; } }

        float _MipmapLODBiad;
        public float MipmapLODBias { get { return _MipmapLODBiad; } set { _MipmapLODBiad = value; } }

        uint _MaxMipLevel;
        public uint MaxMipLevel { get { return _MaxMipLevel; } set { _MaxMipLevel = value; } }

        uint _MaxAnisotropy;
        public uint MaxAnisotropy { get { return _MaxAnisotropy; } set { _MaxAnisotropy = value; } }

        public Sampler1D(TextureBuffer texture)
        {
            if (texture == null)
                throw new ArgumentNullException("Can not sample over a null texture");
            
            if (texture.Rank != 1)
                throw new ArgumentException ("A 1D Sampler can only be used for sample 1D textures");

            _Addresses = new Addressing1D()
            {
                U = TextureAddress.Wrap
            };
            _Filters = new Filtering()
            {
                Magnification = TextureFilterType.Point,
                Minification = TextureFilterType.Point,
                MipMapping = TextureFilterType.Point
            };
            _Texture = texture;

            _MaxAnisotropy = 0;
            _MaxMipLevel = 0;
            _MipmapLODBiad = 0;
        }

        public int Rank { get { return 1; } }

        public Sampler1D Clone(TextureBuffer texture)
        {
            Sampler1D s = this;
            s._Texture = texture;
            return s;
        }

        ISampler ISampler.Clone(TextureBuffer texture)
        {
            return Clone(texture);
        }

        public Vector4 Sample(Vector1 texCoord)
        {
            throw new NotImplementedException ();
        }

        public Vector4 Sample(Vector1 texCoord, float ddx, float ddy)
        {
            throw new NotImplementedException();
        }
    }

    [OnlyGlobalDeclaration]
    public struct Sampler2D : ISampler
    {
        Addressing2D _Addresses;
        public Addressing2D Addresses { get { return _Addresses; } set { _Addresses = value; } }

        Addressing3D ISampler.Addresses { get { return new Addressing3D { U = this._Addresses.U, V = this._Addresses.V, BorderColor = _Addresses.BorderColor }; } }

        Filtering _Filters;
        public Filtering Filters { get { return _Filters; } set { _Filters = value; } }

        TextureBuffer _Texture;
        public TextureBuffer Texture { get { return _Texture; } private set { _Texture = value; } }

        float _MipmapLODBiad;
        public float MipmapLODBias { get { return _MipmapLODBiad; } set { _MipmapLODBiad = value; } }

        uint _MaxMipLevel;
        public uint MaxMipLevel { get { return _MaxMipLevel; } set { _MaxMipLevel = value; } }

        uint _MaxAnisotropy;
        public uint MaxAnisotropy { get { return _MaxAnisotropy; } set { _MaxAnisotropy = value; } }

        public Sampler2D(TextureBuffer texture)
        {
            if (texture == null)
                throw new ArgumentNullException("Can not sample over a null texture");

            if (texture.Rank != 2)
                throw new ArgumentException ("A 2D Sampler can only be used for sample 2D textures");

            _Addresses = new Addressing2D()
            {
                U = TextureAddress.Wrap,
                V = TextureAddress.Wrap
            };
            _Filters = new Filtering()
            {
                Magnification = TextureFilterType.Point,
                Minification = TextureFilterType.Point,
                MipMapping = TextureFilterType.Point
            };
            _Texture = texture;

            _MaxAnisotropy = 0;
            _MaxMipLevel = 0;
            _MipmapLODBiad = 0;
        }

        public Vector4 Sample(Vector2 texCoord)
        {
            throw new NotImplementedException ();
        }

        public Vector4 SampleLOD(Vector4 texAndLOD)
        {
            throw new NotImplementedException();
        }

        public Vector4 Sample(Vector2 texCoord, float ddx, float ddy)
        {
            throw new NotImplementedException ();
        }

        public Sampler2D Clone(TextureBuffer texture)
        {
            Sampler2D s = this;
            s._Texture = texture;
            return s;
        }

        ISampler ISampler.Clone(TextureBuffer texture)
        {
            return Clone(texture);
        }

        public int Rank { get { return 2; } }
    }

    [OnlyGlobalDeclaration]
    public struct Sampler3D : ISampler
    {
        Addressing3D _Addresses;
        public Addressing3D Addresses { get { return _Addresses; } set { _Addresses = value; } }

        Filtering _Filters;
        public Filtering Filters { get { return _Filters; } set { _Filters = value; } }

        TextureBuffer _Texture;
        public TextureBuffer Texture { get { return _Texture; } private set { _Texture = value; } }

        float _MipmapLODBiad;
        public float MipmapLODBias { get { return _MipmapLODBiad; } set { _MipmapLODBiad = value; } }

        uint _MaxMipLevel;
        public uint MaxMipLevel { get { return _MaxMipLevel; } set { _MaxMipLevel = value; } }

        uint _MaxAnisotropy;
        public uint MaxAnisotropy { get { return _MaxAnisotropy; } set { _MaxAnisotropy = value; } }

        public Sampler3D(TextureBuffer texture)
        {
            if (texture == null)
                throw new ArgumentNullException("Can not sample over a null texture");
            _Addresses = new Addressing3D()
            {
                U = TextureAddress.Wrap,
                V = TextureAddress.Wrap,
                W = TextureAddress.Wrap
            };
            _Filters = new Filtering()
            {
                Magnification = TextureFilterType.Point,
                Minification = TextureFilterType.Point,
                MipMapping= TextureFilterType.Point
            };
            _Texture = texture;

            _MaxAnisotropy = 0;
            _MaxMipLevel = 0;
            _MipmapLODBiad = 0;
        }

        public Vector4 Sample(Vector3 texCoord)
        {
            throw new NotImplementedException();
        }

        public Vector4 Sample(Vector3 texCoord, float ddx, float ddy)
        {
            throw new NotImplementedException();
        }

        public Sampler3D Clone(TextureBuffer texture)
        {
            Sampler3D s = this;
            s._Texture = texture;
            return s;
        }

        ISampler ISampler.Clone(TextureBuffer texture)
        {
            return Clone(texture);
        }

        public int Rank { get { return 3; } }
    }

    [OnlyGlobalDeclaration]
    public struct SamplerCube : ISampler
    {
        Addressing3D _Addresses;
        public Addressing3D Addresses { get { return _Addresses; } set { _Addresses = value; } }

        Filtering _Filters;
        public Filtering Filters { get { return _Filters; } set { _Filters = value; } }

        TextureBuffer _Texture;
        public TextureBuffer Texture { get { return _Texture; } private set { _Texture = value; } }

        float _MipmapLODBiad;
        public float MipmapLODBias { get { return _MipmapLODBiad; } set { _MipmapLODBiad = value; } }

        uint _MaxMipLevel;
        public uint MaxMipLevel { get { return _MaxMipLevel; } set { _MaxMipLevel = value; } }

        uint _MaxAnisotropy;
        public uint MaxAnisotropy { get { return _MaxAnisotropy; } set { _MaxAnisotropy = value; } }

        public SamplerCube(CubeTextureBuffer texture)
        {
            if (texture == null)
                throw new ArgumentNullException("Can not sample over a null texture");
            _Addresses = new Addressing3D()
            {
                U = TextureAddress.Wrap,
                V = TextureAddress.Wrap,
                W = TextureAddress.Wrap
            };
            _Filters = new Filtering()
            {
                Magnification = TextureFilterType.Point,
                Minification = TextureFilterType.Point,
                MipMapping = TextureFilterType.Point
            };
            _Texture = texture;

            _MaxAnisotropy = 0;
            _MaxMipLevel = 0;
            _MipmapLODBiad = 0;
        }

        public Vector4 Sample(Vector3 texCoord)
        {
            throw new NotImplementedException();
        }

        public Vector4 Sample(Vector3 texCoord, float ddx, float ddy)
        {
            throw new NotImplementedException();
        }

        public SamplerCube Clone(TextureBuffer texture)
        {
            SamplerCube s = this;
            s._Texture = texture;
            return s;
        }

        ISampler ISampler.Clone(TextureBuffer texture)
        {
            return Clone(texture);
        }

        public int Rank { get { return 3; } }
    }

    public struct Addressing1D
    {
        public TextureAddress U;

        public Vector4<float> BorderColor;
    }

    public struct Addressing2D
    {
        public TextureAddress U;
        public TextureAddress V;

        public Vector4<float> BorderColor;
    }

    public struct Addressing3D
    {
        public TextureAddress U;
        public TextureAddress V;
        public TextureAddress W;

        public Vector4 BorderColor;
    }

    public struct Filtering
    {
        public TextureFilterType Magnification { get; set; }

        public TextureFilterType Minification { get; set; }

        public TextureFilterType MipMapping { get; set; }
    }

    public enum TextureFilterType
    {
        None,
        Point,
        Linear,
        Anisotropic,
        PyramidalQuad,
        GaussianQuad
    }

    public enum TextureAddress
    {
        Wrap,
        Mirror,
        Clamp,
        Border,
        MirrorOnce,
    }
}