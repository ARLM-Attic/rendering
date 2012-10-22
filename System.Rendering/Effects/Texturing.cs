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
using System.Linq;
using System.ComponentModel;
using System.Rendering.RenderStates;
using System.Maths;
using System.Rendering.Resourcing;

namespace System.Rendering.Effects
{
    public class Texturing : Effect
    {
        private Texturing(ISampler[] samplers, TextureStage[] stages)
        {
            this.sampling = new SamplersState(samplers);
            this.stages = new TextureState(stages);
        }

        public static Texturing Append(ColorOperation operation, ColorArgument a0, ColorArgument a1, ColorArgument a2, TextureBuffer texture, ColorArgument result)
        {
            ISampler sampler = null;
            switch (texture.Rank)
            {
              case 2:
                sampler = new Sampler2D(texture)
                {
                  Addresses = new Addressing2D { V = TextureAddress.Wrap, U = TextureAddress.Wrap },
                  Filters = new Filtering { MipMapping = TextureFilterType.None, Minification = TextureFilterType.Point, Magnification = TextureFilterType.Point }
                };
                break;
              case 3:
                sampler = new Sampler3D(texture)
                {
                  Addresses = new Addressing3D { V = TextureAddress.Wrap, U = TextureAddress.Wrap, W = TextureAddress.Wrap },
                  Filters = new Filtering { MipMapping = TextureFilterType.None, Minification = TextureFilterType.Point, Magnification = TextureFilterType.Point }
                };
                break;
              default:
                throw new NotSupportedException();
            }
            return new Texturing(sampler, new TextureStage
            {
                Transform = Matrices.I,
                Argument0 = a0,
                Argument1 = a1,
                Argument2 = a2,
                Operation = operation,
                Result = result
            });
        }

        public static Texturing Append(ColorOperation operation, ColorArgument argument0, ColorArgument argument1, TextureBuffer texture)
        {
            return Append(operation, argument0, argument1, ColorArgument.TextureColor, texture, ColorArgument.Current);
        }

        public static Texturing AddDiffuse(TextureBuffer texture)
        {
            return Append(ColorOperation.Add, ColorArgument.Constant, ColorArgument.Current, texture);
        }

        public static Texturing ModulateDiffuse(TextureBuffer texture)
        {
            return Append(ColorOperation.Modulate, ColorArgument.Constant, ColorArgument.Current, texture);
        }

        public static Texturing BlendDiffuse(TextureBuffer texture)
        {
            return Append(ColorOperation.BlendTextureAlpha, ColorArgument.Constant, ColorArgument.Current, texture);
        }

        public static Texturing MultiplyDiffuse(TextureBuffer texture)
        {
            return Append(ColorOperation.MultiplyAdd, ColorArgument.Constant, ColorArgument.Current, texture);
        }

        public static Texturing SetDiffuse(TextureBuffer texture)
        {
            return Append(ColorOperation.MultiplyAdd, ColorArgument.Constant, ColorArgument.Diffuse, texture);
        }

        public static Texturing SetSpecular(TextureBuffer specular)
        {
            return Append(ColorOperation.MultiplyAdd, ColorArgument.Constant, ColorArgument.Specular, ColorArgument.TextureColor, specular, ColorArgument.Temp);
        }

        public static Texturing AddSpecular(TextureBuffer specular)
        {
            return Append(ColorOperation.MultiplyAdd, ColorArgument.Temp, ColorArgument.Specular, ColorArgument.TextureColor, specular, ColorArgument.Temp);
        }

        public static Texturing LitCurrent(TextureBuffer diffuse)
        {
            return Append(ColorOperation.MultiplyAdd, ColorArgument.Specular, ColorArgument.Current, diffuse);
        }

        SamplersState sampling;

        TextureState stages;

        public Texturing(ISampler sampler, TextureStage stage)
            : this(new ISampler[] { sampler }, new TextureStage[] { stage })
        {
        }

        protected class TextureTechnique : Technique
        {
            protected Texturing texturing;

            bool needsToDispose;

            public TextureTechnique(IRenderStatesManager manager, Texturing texturing)
                : base(manager)
            {
                if (((IAllocateable)texturing).Render != manager.Render)
                {
                    this.texturing = texturing.Allocate(manager.Render);
                    needsToDispose = true;
                }
                else
                    this.texturing = texturing;
            }

            protected override void SaveStates()
            {
                RenderStates.Save<TextureState>();
                RenderStates.Save<SamplersState>();
            }

            protected override IEnumerable<Pass> Passes
            {
                get
                {
                    var oldSamplings = RenderStates.GetState<SamplersState>();
                    var oldStages = RenderStates.GetState<TextureState>();

                    RenderStates.SetState(TextureState.Concat(texturing.stages, oldStages));
                    RenderStates.SetState(SamplersState.Concat(texturing.sampling, oldSamplings));
                    yield return NewPass();
                }
            }

            protected override void RestoreStates()
            {
                RenderStates.Restore<SamplersState>();
                RenderStates.Restore<TextureState>();
            }

            public override void Dispose()
            {
                if (needsToDispose)
                    texturing.Dispose();
                base.Dispose();
            }
        }

        protected override Technique GetTechnique(IRenderStatesManager manager)
        {
            return new TextureTechnique(manager, this);
        }

        protected override bool IsSupported(IRenderDevice render)
        {
            return render.RenderStateInfo.IsSupported<TextureState>() && render.RenderStateInfo.IsSupported<SamplersState>();
        }

        protected override Location OnClone(Resourcing.AllocateableBase toFill, IRenderDevice render)
        {
            Texturing texturing = toFill as Texturing;

            texturing.sampling = this.sampling.Clone(render);

            return render == null ? Location.User : Location.Device;
        }

        protected override void OnDispose()
        {
            sampling.Dispose();
        }
    }
}