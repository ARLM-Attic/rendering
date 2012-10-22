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

namespace System.Maths
{
    public struct Sampler     
    {
        #region Instances
        private TextureAddress addressU, addressV, addressW;
        private TextureFilterType
            magFilter,
            minFilter,
            mipFilter;
        private uint mipmapLODBias;
        private uint maxMipLevel;
        private uint maxAnisotropy;
        private Vector4<FLOATINGTYPE> borderColor;
        #endregion

        public Sampler(TextureAddress addU, TextureAddress addV, TextureAddress addW, TextureFilterType min, TextureFilterType mag, TextureFilterType mip, Vector4<FLOATINGTYPE> border, uint mipmapLODBias, uint maxmipLevel, uint maxanisotropy)
        {
            this.addressU = addU;
            this.addressV = addV;
            this.addressW = addW;
            this.borderColor = border;
            this.magFilter = mag;
            this.minFilter = min;
            this.mipFilter = mip;
            this.mipmapLODBias = mipmapLODBias;
            this.maxMipLevel = maxmipLevel;
            this.maxAnisotropy = maxanisotropy;
            Indexing = null;
        }

        public TextureAddress AddressV
        {
            get { return addressV; }
            set { addressV = value; }
        }

        public TextureAddress AddressU
        {
            get { return addressU; }
            set { addressU = value; }
        }

        public TextureAddress AddressW
        {
            get { return addressW; }
            set { addressW = value; }
        }

        public Vector4<FLOATINGTYPE> BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

        public TextureFilterType MipFilter
        {
            get { return mipFilter; }
            set { mipFilter = value; }
        }

        public TextureFilterType MinFilter
        {
            get { return minFilter; }
            set { minFilter = value; }
        }

        public TextureFilterType MagFilter
        {
            get { return magFilter; }
            set { magFilter = value; }
        }

        public uint MipmapLODBias
        {
            get { return mipmapLODBias; }
            set { mipmapLODBias = value; }
        }

        public uint MaxMipLevel
        {
            get { return maxMipLevel; }
            set { maxMipLevel = value; }
        }

        public uint MaxAnisotropy
        {
            get { return maxAnisotropy; }
            set { maxAnisotropy = value; }
        }

        public event Func<Vector3<FLOATINGTYPE>, Vector4<FLOATINGTYPE>> Indexing;

        public Vector4<FLOATINGTYPE> Sample(Vector3<FLOATINGTYPE> texCoord)
        {
            var indexing = Indexing;
            if (indexing != null)
            {
                // wrap support only
                texCoord = texCoord.Clamped;
                // Point Sampling support only.
                return indexing(texCoord);
            }

            return Vectors.Transparent;
        }

        public Vector4<FLOATINGTYPE> Sample(Vector2<FLOATINGTYPE> texCoord)
        {
            return Sample(new Vector3<FLOATINGTYPE>(texCoord.X, texCoord.Y, 0));
        }

        public Vector4<FLOATINGTYPE> Sample(Vector3<FLOATINGTYPE> texCoord, float ddx, float ddy, float ddz)
        {
            return Sample(texCoord);
        }

        public Vector4<FLOATINGTYPE> Sample(Vector2<FLOATINGTYPE> texCoord, float ddx, float ddy)
        {
            return Sample(new Vector3<FLOATINGTYPE>(texCoord.X, texCoord.Y, 0), ddx, ddy, 0);
        }
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