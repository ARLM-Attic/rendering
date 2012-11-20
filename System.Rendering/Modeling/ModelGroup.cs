using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;

namespace System.Rendering.Modeling
{
    public class ModelGroup : AllocateableBase, IModel
    {
        IModel[] children;

        public ModelGroup(params IModel[] model)
        {
            this.children = model.Clone() as IModel[];
        }

        protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
        {
            ModelGroup group = toFill as ModelGroup;

            IModel[] clone = new IModel[this.children.Length];

            for (int i = 0; i < clone.Length; i++)
                clone[i] = children[i].Allocate(render);

            group.children = clone;

            return group.children.All(m => m.Location == Location.Device) ? Location.Device : Location.User;
        }

        protected override void OnDispose()
        {
            foreach (var m in children)
                m.Dispose();
        }

        public bool IsSupported(IRenderDevice device)
        {
            return children.All(m => m.IsSupported(device));
        }

        public void Tesselate(ITessellator tessellator)
        {
            foreach (var m in children)
                m.Tesselate(tessellator);
        }

        public IModel Transformed(Maths.Matrix4x4 transform)
        {
            IModel[] transformed = new IModel[children.Length];

            for (int i=0; i<transformed.Length; i++)
                transformed[i] = children[i].Transformed (transform);

            return new ModelGroup(transformed);
        }

        public IModel Transformed<FVFIn, FVFOut>(Func<FVFIn, FVFOut> transform)
            where FVFIn : struct
            where FVFOut : struct
        {
            IModel[] transformed = new IModel[children.Length];

            for (int i = 0; i < transformed.Length; i++)
                transformed[i] = children[i].Transformed(transform);

            return new ModelGroup(transformed);
        }
    }
}
