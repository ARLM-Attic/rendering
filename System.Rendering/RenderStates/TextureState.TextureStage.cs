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
using System.Reflection;
using System.Globalization;
using System.Maths;

namespace System.Rendering.RenderStates
{
    public struct TextureStage
    {
        public TextureCoordinatesIndex Generation { get; set; }

        public bool LockToCamera
        {
            get
            {
                return Generation == TextureCoordinatesIndex.OrthographicMap || Generation == TextureCoordinatesIndex.NormalMap;
            }
        }

        public Matrix4x4 Transform;

        public ColorArgument AlphaArgument0 { get; set; }

        public ColorArgument AlphaArgument1 { get; set; }

        public ColorArgument AlphaArgument2 { get; set; }

        public ColorOperation AlphaOperation { get; set; }

        public ColorArgument Argument0 { get; set; }

        public ColorArgument Argument1 { get; set; }

        public ColorArgument Argument2 { get; set; }

        public ColorArgument Result { get; set; }

        public ColorOperation Operation { get; set; }

        public Vector4 Constant { get; set; }

    }

    public enum ColorArgument
    {
        Diffuse = 0,
        Current = 1,
        TextureColor = 2,
        TFactor = 3,
        Specular = 4,
        Temp = 5,
        Constant = 6,
        Complement = 16,
        AlphaReplicate = 32,
    }

    public enum ColorOperation
    {
        Disable = 1,
        SelectArg1 = 2,
        SelectArg2 = 3,
        Modulate = 4,
        Modulate2X = 5,
        Modulate4X = 6,
        Add = 7,
        AddSigned = 8,
        AddSigned2X = 9,
        Subtract = 10,
        AddSmooth = 11,
        BlendDiffuseAlpha = 12,
        BlendTextureAlpha = 13,
        BlendFactorAlpha = 14,
        BlendTextureAlphaPM = 15,
        BlendCurrentAlpha = 16,
        PreModulate = 17,
        ModulateAlphaAddColor = 18,
        ModulateColorAddAlpha = 19,
        ModulateInvAlphaAddColor = 20,
        ModulateInvColorAddAlpha = 21,
        BumpEnvironmentMap = 22,
        BumpEnvironmentMapLuminance = 23,
        DotProduct3 = 24,
        MultiplyAdd = 25,
        Lerp = 26,
    }

    public enum TextureCoordinatesIndex
    {
        Mapping = 0,
        MappingIndex0 = Mapping,
        MappingIndex1 = Mapping + 1,
        MappingIndex2 = Mapping + 2,
        MappingIndex3 = Mapping + 3,
        MappingIndex4 = Mapping + 4,
        MappingIndex5 = Mapping + 5,
        MappingIndex6 = Mapping + 6,
        MappingIndex7 = Mapping + 7,
        OrthographicMap = 65536,
        OrthographicEyeMap = 65537,
        NormalMap = 65538,
        NormalEyeMap = 65539,
        ReflectionMap = 65540,
        SphereMap = 65541
    }
}
