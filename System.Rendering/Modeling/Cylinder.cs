using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;

namespace System.Rendering.Modeling
{
    public class Cylinder : AllocateableBase, IModel
    {
        IModel top, bottom, border;

        public Cylinder()
        {
            bottom = Models.diskBottom32x32;
            border = Models.cylinderBorder32x32;
            top = Models.diskTop32x32;
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            Cylinder c = (Cylinder)toFill;

            c.top = this.top.Allocate(render);
            c.border = this.border.Allocate(render);
            c.bottom = this.bottom.Allocate(render);

            return render == null ? Location.User : Location.Device;
        }

        protected override void OnDispose()
        {
            this.top.Dispose();
            this.border.Dispose();
            this.bottom.Dispose();
        }

        public bool IsSupported(IRenderDevice device)
        {
            return device.TessellatorInfo.IsSupported<Basic>();
        }

        public void Tesselate(ITessellator tessellator)
        {
            this.top.Tesselate(tessellator);
            this.border.Tesselate(tessellator);
            this.bottom.Tesselate(tessellator);
        }
    }
}
