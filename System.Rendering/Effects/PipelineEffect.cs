using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering;
using System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Effects
{
    public abstract class PipelineEffect<VSIn, VSOut, PSIn, PSOut> : Effect
        where VSIn : struct
        where VSOut : struct
        where PSIn : struct
        where PSOut : struct
    {
        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            World = manager.GetState<WorldState>().Matrix;
            View = manager.GetState<ViewState>().Matrix;
            Projection = manager.GetState<ProjectionState>().Matrix;

            return new PipelineTechnique(this, manager);
        }

        public Matrix4x4 World;
        public Matrix4x4 View;
        public Matrix4x4 Projection;

        VertexShaderState vsState;
        PixelShaderState psState;

        public PipelineEffect()
        {
            vsState = new VertexShaderState((Func<VSIn, VSOut>)this.VertexShader);
            psState = new PixelShaderState((Func<PSIn, PSOut>)this.PixelShader);
        }

        protected abstract VSOut VertexShader(VSIn In);

        protected abstract PSOut PixelShader(PSIn In);

        class PipelineTechnique : Technique
        {
            PipelineEffect<VSIn, VSOut, PSIn, PSOut> pipeline;

            public PipelineTechnique(PipelineEffect<VSIn, VSOut, PSIn, PSOut> pipeline, IRenderStatesManager man)
                : base(man)
            {
                this.pipeline = pipeline;
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    RenderStates.SetState<VertexShaderState>(pipeline.vsState);

                    RenderStates.SetState<PixelShaderState>(pipeline.psState);

                    yield return NewPass();
                }
            }

            protected override void SaveStates()
            {
                RenderStates.Save<VertexShaderState>();
                RenderStates.Save<PixelShaderState>();
                RenderStates.Save<SamplersState>();
            }

            protected override void RestoreStates()
            {
                RenderStates.Restore<SamplersState>();
                RenderStates.Restore<PixelShaderState>();
                RenderStates.Restore<VertexShaderState>();
            }
        }

        protected override Location OnClone(System.Rendering.Resourcing.AllocateableBase toFill, IRenderDevice render)
        {
            return Location.Device;
        }
        protected override void OnDispose()
        {
        }
    }
}
