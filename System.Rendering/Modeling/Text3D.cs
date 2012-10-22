using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.Modeling
{
    public struct Text3D : IGraphicPrimitive
    {
        public static Text3D From(string text, string fontFamily, FontStyle style)
        {
            Text3D t = new Text3D();
            t.text = text;
            t.fontFamily = fontFamily;
            t.style = style;
            return t;
        }

        public static Text3D From(string text)
        {
            Text3D t = new Text3D();
            t.text = text;
            t.fontFamily = "Default";
            t.style =  FontStyle.Regular;
            return t;
        }

        string text;

        string fontFamily;

        FontStyle style;

        public string Text
        {
            get { return text; }
        }

        public string FontFamily
        {
            get { return fontFamily; }
        }

        public FontStyle Style
        {
            get { return style; }
        }
    }

    /// <summary>
    /// Specifies style information applied to text.
    /// </summary>
    [Flags]
    public enum FontStyle
    {
        /// <summary>
        /// Normal text.
        /// </summary>
        Regular = 0,
        /// <summary>
        /// Bold text.
        /// </summary>
        Bold = 1,
        /// <summary>
        /// Italic text.
        /// </summary>
        Italic = 2,
        /// <summary>
        /// Underlined text.
        /// </summary>
        Underline = 4,
        /// <summary>
        /// Text with a line through the middle.
        /// </summary>
        Strikeout = 8,
    }
}
