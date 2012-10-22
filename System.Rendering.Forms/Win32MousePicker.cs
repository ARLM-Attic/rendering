using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Maths;

namespace System.Rendering.Forms
{
    public class Win32MousePicker : IRayPicker
    {
        RenderedControl control;

        public Win32MousePicker(RenderedControl control)
        {
            this.control = control;
        }

        #region IRayPicker Members

        public Ray Ray
        {
            get
            {
                IControlRenderDevice render = control.Render;

                if (render == null)
                    return null;

                return render.FromScreenToWorld(ConvertToVec2(control.PointToClient (Control.MousePosition)));
            }
        }

        static Vector2<float> ConvertToVec2(Point point)
        {
            return new Vector2<float>(point.X, point.Y);
        }

        #endregion
    }
}
