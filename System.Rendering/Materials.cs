using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Effects;
using System.Maths;

namespace System.Rendering
{
    public class Materials
    {
        public static Material Diffuse(Vector3 color)
        {
            return new Material { Diffuse = color , Specular = new Vector3 (1,1,1), BlendMode = StateBlendMode.Multiply };
        }

        public static Material Diffuse(Vector4 color)
        {
            return new Material { Diffuse = (Vector3)color, Opacity = color.W, Specular = new Vector3(1, 1, 1), BlendMode = StateBlendMode.Multiply };
        }

        public static Material Diffuse(Vector3 color, float opacity)
        {
            return new Material { Diffuse = color, Opacity = opacity, Specular = new Vector3(1, 1, 1), BlendMode = StateBlendMode.Multiply };
        }

        #region Diffuse Materials


        public static Material Transparent { get { return Material.From(new Vector3(1f, 1f, 1f)); } }

        public static Material AliceBlue { get { return Material.From(new Vector3(0.9411765f, 0.972549f, 1f)); } }

        public static Material AntiqueWhite { get { return Material.From(new Vector3(0.9803922f, 0.9215686f, 0.8431373f)); } }

        public static Material Aqua { get { return Material.From(new Vector3(0f, 1f, 1f)); } }

        public static Material Aquamarine { get { return Material.From(new Vector3(0.4980392f, 1f, 0.8313726f)); } }

        public static Material Azure { get { return Material.From(new Vector3(0.9411765f, 1f, 1f)); } }

        public static Material Beige { get { return Material.From(new Vector3(0.9607843f, 0.9607843f, 0.8627451f)); } }

        public static Material Bisque { get { return Material.From(new Vector3(1f, 0.8941177f, 0.7686275f)); } }

        public static Material Black { get { return Material.From(new Vector3(0f, 0f, 0f)); } }

        public static Material BlanchedAlmond { get { return Material.From(new Vector3(1f, 0.9215686f, 0.8039216f)); } }

        public static Material Blue { get { return Material.From(new Vector3(0f, 0f, 1f)); } }

        public static Material BlueViolet { get { return Material.From(new Vector3(0.5411765f, 0.1686275f, 0.8862745f)); } }

        public static Material Brown { get { return Material.From(new Vector3(0.6470588f, 0.1647059f, 0.1647059f)); } }

        public static Material BurlyWood { get { return Material.From(new Vector3(0.8705882f, 0.7215686f, 0.5294118f)); } }

        public static Material CadetBlue { get { return Material.From(new Vector3(0.372549f, 0.6196079f, 0.627451f)); } }

        public static Material Chartreuse { get { return Material.From(new Vector3(0.4980392f, 1f, 0f)); } }

        public static Material Chocolate { get { return Material.From(new Vector3(0.8235294f, 0.4117647f, 0.1176471f)); } }

        public static Material Coral { get { return Material.From(new Vector3(1f, 0.4980392f, 0.3137255f)); } }

        public static Material CornflowerBlue { get { return Material.From(new Vector3(0.3921569f, 0.5843138f, 0.9294118f)); } }

        public static Material Cornsilk { get { return Material.From(new Vector3(1f, 0.972549f, 0.8627451f)); } }

        public static Material Crimson { get { return Material.From(new Vector3(0.8627451f, 0.07843138f, 0.2352941f)); } }

        public static Material Cyan { get { return Material.From(new Vector3(0f, 1f, 1f)); } }

        public static Material DarkBlue { get { return Material.From(new Vector3(0f, 0f, 0.5450981f)); } }

        public static Material DarkCyan { get { return Material.From(new Vector3(0f, 0.5450981f, 0.5450981f)); } }

        public static Material DarkGoldenrod { get { return Material.From(new Vector3(0.7215686f, 0.5254902f, 0.04313726f)); } }

        public static Material DarkGray { get { return Material.From(new Vector3(0.6627451f, 0.6627451f, 0.6627451f)); } }

        public static Material DarkGreen { get { return Material.From(new Vector3(0f, 0.3921569f, 0f)); } }

        public static Material DarkKhaki { get { return Material.From(new Vector3(0.7411765f, 0.7176471f, 0.4196078f)); } }

        public static Material DarkMagenta { get { return Material.From(new Vector3(0.5450981f, 0f, 0.5450981f)); } }

        public static Material DarkOliveGreen { get { return Material.From(new Vector3(0.3333333f, 0.4196078f, 0.1843137f)); } }

        public static Material DarkOrange { get { return Material.From(new Vector3(1f, 0.5490196f, 0f)); } }

        public static Material DarkOrchid { get { return Material.From(new Vector3(0.6f, 0.1960784f, 0.8f)); } }

        public static Material DarkRed { get { return Material.From(new Vector3(0.5450981f, 0f, 0f)); } }

        public static Material DarkSalmon { get { return Material.From(new Vector3(0.9137255f, 0.5882353f, 0.4784314f)); } }

        public static Material DarkSeaGreen { get { return Material.From(new Vector3(0.5607843f, 0.7372549f, 0.5450981f)); } }

        public static Material DarkSlateBlue { get { return Material.From(new Vector3(0.282353f, 0.2392157f, 0.5450981f)); } }

        public static Material DarkSlateGray { get { return Material.From(new Vector3(0.1843137f, 0.3098039f, 0.3098039f)); } }

        public static Material DarkTurquoise { get { return Material.From(new Vector3(0f, 0.8078431f, 0.8196079f)); } }

        public static Material DarkViolet { get { return Material.From(new Vector3(0.5803922f, 0f, 0.827451f)); } }

        public static Material DeepPink { get { return Material.From(new Vector3(1f, 0.07843138f, 0.5764706f)); } }

        public static Material DeepSkyBlue { get { return Material.From(new Vector3(0f, 0.7490196f, 1f)); } }

        public static Material DimGray { get { return Material.From(new Vector3(0.4117647f, 0.4117647f, 0.4117647f)); } }

        public static Material DodgerBlue { get { return Material.From(new Vector3(0.1176471f, 0.5647059f, 1f)); } }

        public static Material Firebrick { get { return Material.From(new Vector3(0.6980392f, 0.1333333f, 0.1333333f)); } }

        public static Material FloralWhite { get { return Material.From(new Vector3(1f, 0.9803922f, 0.9411765f)); } }

        public static Material ForestGreen { get { return Material.From(new Vector3(0.1333333f, 0.5450981f, 0.1333333f)); } }

        public static Material Fuchsia { get { return Material.From(new Vector3(1f, 0f, 1f)); } }

        public static Material Gainsboro { get { return Material.From(new Vector3(0.8627451f, 0.8627451f, 0.8627451f)); } }

        public static Material GhostWhite { get { return Material.From(new Vector3(0.972549f, 0.972549f, 1f)); } }

        public static Material Gold { get { return Material.From(new Vector3(1f, 0.8431373f, 0f)); } }

        public static Material Goldenrod { get { return Material.From(new Vector3(0.854902f, 0.6470588f, 0.1254902f)); } }

        public static Material Gray { get { return Material.From(new Vector3(0.5019608f, 0.5019608f, 0.5019608f)); } }

        public static Material Green { get { return Material.From(new Vector3(0f, 0.5019608f, 0f)); } }

        public static Material GreenYellow { get { return Material.From(new Vector3(0.6784314f, 1f, 0.1843137f)); } }

        public static Material Honeydew { get { return Material.From(new Vector3(0.9411765f, 1f, 0.9411765f)); } }

        public static Material HotPink { get { return Material.From(new Vector3(1f, 0.4117647f, 0.7058824f)); } }

        public static Material IndianRed { get { return Material.From(new Vector3(0.8039216f, 0.3607843f, 0.3607843f)); } }

        public static Material Indigo { get { return Material.From(new Vector3(0.2941177f, 0f, 0.509804f)); } }

        public static Material Ivory { get { return Material.From(new Vector3(1f, 1f, 0.9411765f)); } }

        public static Material Khaki { get { return Material.From(new Vector3(0.9411765f, 0.9019608f, 0.5490196f)); } }

        public static Material Lavender { get { return Material.From(new Vector3(0.9019608f, 0.9019608f, 0.9803922f)); } }

        public static Material LavenderBlush { get { return Material.From(new Vector3(1f, 0.9411765f, 0.9607843f)); } }

        public static Material LawnGreen { get { return Material.From(new Vector3(0.4862745f, 0.9882353f, 0f)); } }

        public static Material LemonChiffon { get { return Material.From(new Vector3(1f, 0.9803922f, 0.8039216f)); } }

        public static Material LightBlue { get { return Material.From(new Vector3(0.6784314f, 0.8470588f, 0.9019608f)); } }

        public static Material LightCoral { get { return Material.From(new Vector3(0.9411765f, 0.5019608f, 0.5019608f)); } }

        public static Material LightCyan { get { return Material.From(new Vector3(0.8784314f, 1f, 1f)); } }

        public static Material LightGoldenrodYellow { get { return Material.From(new Vector3(0.9803922f, 0.9803922f, 0.8235294f)); } }

        public static Material LightGreen { get { return Material.From(new Vector3(0.5647059f, 0.9333333f, 0.5647059f)); } }

        public static Material LightGray { get { return Material.From(new Vector3(0.827451f, 0.827451f, 0.827451f)); } }

        public static Material LightPink { get { return Material.From(new Vector3(1f, 0.7137255f, 0.7568628f)); } }

        public static Material LightSalmon { get { return Material.From(new Vector3(1f, 0.627451f, 0.4784314f)); } }

        public static Material LightSeaGreen { get { return Material.From(new Vector3(0.1254902f, 0.6980392f, 0.6666667f)); } }

        public static Material LightSkyBlue { get { return Material.From(new Vector3(0.5294118f, 0.8078431f, 0.9803922f)); } }

        public static Material LightSlateGray { get { return Material.From(new Vector3(0.4666667f, 0.5333334f, 0.6f)); } }

        public static Material LightSteelBlue { get { return Material.From(new Vector3(0.6901961f, 0.7686275f, 0.8705882f)); } }

        public static Material LightYellow { get { return Material.From(new Vector3(1f, 1f, 0.8784314f)); } }

        public static Material Lime { get { return Material.From(new Vector3(0f, 1f, 0f)); } }

        public static Material LimeGreen { get { return Material.From(new Vector3(0.1960784f, 0.8039216f, 0.1960784f)); } }

        public static Material Linen { get { return Material.From(new Vector3(0.9803922f, 0.9411765f, 0.9019608f)); } }

        public static Material Magenta { get { return Material.From(new Vector3(1f, 0f, 1f)); } }

        public static Material Maroon { get { return Material.From(new Vector3(0.5019608f, 0f, 0f)); } }

        public static Material MediumAquamarine { get { return Material.From(new Vector3(0.4f, 0.8039216f, 0.6666667f)); } }

        public static Material MediumBlue { get { return Material.From(new Vector3(0f, 0f, 0.8039216f)); } }

        public static Material MediumOrchid { get { return Material.From(new Vector3(0.7294118f, 0.3333333f, 0.827451f)); } }

        public static Material MediumPurple { get { return Material.From(new Vector3(0.5764706f, 0.4392157f, 0.8588235f)); } }

        public static Material MediumSeaGreen { get { return Material.From(new Vector3(0.2352941f, 0.7019608f, 0.4431373f)); } }

        public static Material MediumSlateBlue { get { return Material.From(new Vector3(0.4823529f, 0.4078431f, 0.9333333f)); } }

        public static Material MediumSpringGreen { get { return Material.From(new Vector3(0f, 0.9803922f, 0.6039216f)); } }

        public static Material MediumTurquoise { get { return Material.From(new Vector3(0.282353f, 0.8196079f, 0.8f)); } }

        public static Material MediumVioletRed { get { return Material.From(new Vector3(0.7803922f, 0.08235294f, 0.5215687f)); } }

        public static Material MidnightBlue { get { return Material.From(new Vector3(0.09803922f, 0.09803922f, 0.4392157f)); } }

        public static Material MintCream { get { return Material.From(new Vector3(0.9607843f, 1f, 0.9803922f)); } }

        public static Material MistyRose { get { return Material.From(new Vector3(1f, 0.8941177f, 0.8823529f)); } }

        public static Material Moccasin { get { return Material.From(new Vector3(1f, 0.8941177f, 0.7098039f)); } }

        public static Material NavajoWhite { get { return Material.From(new Vector3(1f, 0.8705882f, 0.6784314f)); } }

        public static Material Navy { get { return Material.From(new Vector3(0f, 0f, 0.5019608f)); } }

        public static Material OldLace { get { return Material.From(new Vector3(0.9921569f, 0.9607843f, 0.9019608f)); } }

        public static Material Olive { get { return Material.From(new Vector3(0.5019608f, 0.5019608f, 0f)); } }

        public static Material OliveDrab { get { return Material.From(new Vector3(0.4196078f, 0.5568628f, 0.1372549f)); } }

        public static Material Orange { get { return Material.From(new Vector3(1f, 0.6470588f, 0f)); } }

        public static Material OrangeRed { get { return Material.From(new Vector3(1f, 0.2705882f, 0f)); } }

        public static Material Orchid { get { return Material.From(new Vector3(0.854902f, 0.4392157f, 0.8392157f)); } }

        public static Material PaleGoldenrod { get { return Material.From(new Vector3(0.9333333f, 0.9098039f, 0.6666667f)); } }

        public static Material PaleGreen { get { return Material.From(new Vector3(0.5960785f, 0.9843137f, 0.5960785f)); } }

        public static Material PaleTurquoise { get { return Material.From(new Vector3(0.6862745f, 0.9333333f, 0.9333333f)); } }

        public static Material PaleVioletRed { get { return Material.From(new Vector3(0.8588235f, 0.4392157f, 0.5764706f)); } }

        public static Material PapayaWhip { get { return Material.From(new Vector3(1f, 0.9372549f, 0.8352941f)); } }

        public static Material PeachPuff { get { return Material.From(new Vector3(1f, 0.854902f, 0.7254902f)); } }

        public static Material Peru { get { return Material.From(new Vector3(0.8039216f, 0.5215687f, 0.2470588f)); } }

        public static Material Pink { get { return Material.From(new Vector3(1f, 0.7529412f, 0.7960784f)); } }

        public static Material Plum { get { return Material.From(new Vector3(0.8666667f, 0.627451f, 0.8666667f)); } }

        public static Material PowderBlue { get { return Material.From(new Vector3(0.6901961f, 0.8784314f, 0.9019608f)); } }

        public static Material Purple { get { return Material.From(new Vector3(0.5019608f, 0f, 0.5019608f)); } }

        public static Material Red { get { return Material.From(new Vector3(1f, 0f, 0f)); } }

        public static Material RosyBrown { get { return Material.From(new Vector3(0.7372549f, 0.5607843f, 0.5607843f)); } }

        public static Material RoyalBlue { get { return Material.From(new Vector3(0.254902f, 0.4117647f, 0.8823529f)); } }

        public static Material SaddleBrown { get { return Material.From(new Vector3(0.5450981f, 0.2705882f, 0.07450981f)); } }

        public static Material Salmon { get { return Material.From(new Vector3(0.9803922f, 0.5019608f, 0.4470588f)); } }

        public static Material SandyBrown { get { return Material.From(new Vector3(0.9568627f, 0.6431373f, 0.3764706f)); } }

        public static Material SeaGreen { get { return Material.From(new Vector3(0.1803922f, 0.5450981f, 0.3411765f)); } }

        public static Material SeaShell { get { return Material.From(new Vector3(1f, 0.9607843f, 0.9333333f)); } }

        public static Material Sienna { get { return Material.From(new Vector3(0.627451f, 0.3215686f, 0.1764706f)); } }

        public static Material Silver { get { return Material.From(new Vector3(0.7529412f, 0.7529412f, 0.7529412f)); } }

        public static Material SkyBlue { get { return Material.From(new Vector3(0.5294118f, 0.8078431f, 0.9215686f)); } }

        public static Material SlateBlue { get { return Material.From(new Vector3(0.4156863f, 0.3529412f, 0.8039216f)); } }

        public static Material SlateGray { get { return Material.From(new Vector3(0.4392157f, 0.5019608f, 0.5647059f)); } }

        public static Material Snow { get { return Material.From(new Vector3(1f, 0.9803922f, 0.9803922f)); } }

        public static Material SpringGreen { get { return Material.From(new Vector3(0f, 1f, 0.4980392f)); } }

        public static Material SteelBlue { get { return Material.From(new Vector3(0.2745098f, 0.509804f, 0.7058824f)); } }

        public static Material Tan { get { return Material.From(new Vector3(0.8235294f, 0.7058824f, 0.5490196f)); } }

        public static Material Teal { get { return Material.From(new Vector3(0f, 0.5019608f, 0.5019608f)); } }

        public static Material Thistle { get { return Material.From(new Vector3(0.8470588f, 0.7490196f, 0.8470588f)); } }

        public static Material Tomato { get { return Material.From(new Vector3(1f, 0.3882353f, 0.2784314f)); } }

        public static Material Turquoise { get { return Material.From(new Vector3(0.2509804f, 0.8784314f, 0.8156863f)); } }

        public static Material Violet { get { return Material.From(new Vector3(0.9333333f, 0.509804f, 0.9333333f)); } }

        public static Material Wheat { get { return Material.From(new Vector3(0.9607843f, 0.8705882f, 0.7019608f)); } }

        public static Material White { get { return Material.From(new Vector3(1f, 1f, 1f)); } }

        public static Material WhiteSmoke { get { return Material.From(new Vector3(0.9607843f, 0.9607843f, 0.9607843f)); } }

        public static Material Yellow { get { return Material.From(new Vector3(1f, 1f, 0f)); } }

        public static Material YellowGreen { get { return Material.From(new Vector3(0.6039216f, 0.8039216f, 0.1960784f)); } }


        #endregion
    }
}
