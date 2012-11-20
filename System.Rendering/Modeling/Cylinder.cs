using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;

namespace System.Rendering.Modeling
{
    class Cylinder : ModelGroup
    {
        public Cylinder()
            : base(Models.diskBottom32x32, Models.cylinderBorder32x32, Models.diskTop32x32)
        {
        }
    }
}
