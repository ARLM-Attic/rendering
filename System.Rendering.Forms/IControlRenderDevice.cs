using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

namespace System.Rendering.Forms
{
    public interface IControlRenderDevice : IInteractiveRenderDevice, IComponent
    {
        bool IsCreated { get; }

        void CreateDevice(Control hWnd);

        event EventHandler Created;
    }

    public interface IFullScreenableDeviceRender : IControlRenderDevice
    {
        bool FullScreen { get; set; }

        IEnumerable<DisplaySettings> AvailableDisplaySettings { get; }

        DisplaySettings FullScreenSetting { get; set; }
    }

    public struct DisplaySettings
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public IEnumerable<VertexComponentAttribute> PixelFormatDescription { get; set; }
    }

}
