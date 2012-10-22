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
using System.Rendering.Resourcing;
using System.Maths;
using System.Rendering.Effects;

namespace System.Rendering.RenderStates
{
    public struct TextureState
    {
        public static TextureState Default
        {
            get
            {
                return new TextureState(new TextureStage[] { });

                //return new TextureState(new TextureStage[] 
                //{ 
                //    new TextureStage { Argument0 = ColorArgument.Specular, Argument1 = ColorArgument.Diffuse, Argument2 = ColorArgument.Current, Operation = ColorOperation.MultiplyAdd, Result = ColorArgument.Current }
                //});
            }
        }

        TextureStage[] stages;

        public TextureStage this[int stage]
        {
            get { return stages[stage]; }
        }

        public int NumberOfStages { get { return stages == null?0: stages.Length; } }

        public static TextureState Concat(TextureState first, TextureState second)
        {
            TextureStage[] stages = new TextureStage[first.NumberOfStages + second.NumberOfStages];
            for (int i = 0; i < first.NumberOfStages; i++)
                stages[i] = first[i];
            for (int i = 0; i < second.NumberOfStages; i++)
                stages[i + first.NumberOfStages] = second[i];
            return new TextureState(stages);
        }

        public TextureState(TextureStage[] stages)
        {
            this.stages = stages.Clone() as TextureStage[];
        }
    }

    public struct SamplersState
    {
        public static readonly SamplersState Default = new SamplersState(new ISampler[0]);

        public SamplersState(ISampler[] samplers)
        {
            this.samplers = samplers.Clone() as ISampler[];
        }

        private ISampler[] samplers;

        public int NumberOfSamplers { get { return samplers == null ? 0 : samplers.Length; } }

        public ISampler this[int index] { get { return samplers[index]; } }

        public SamplersState Clone(IRenderDevice render)
        {
            ISampler[] samplers = this.samplers.Select(s => s.Clone(s.Texture.Render == render ? s.Texture.Reference<TextureBuffer>() : (TextureBuffer)s.Texture.Clone(render))).ToArray();

            return new SamplersState(samplers);
        }

        public static SamplersState Concat(SamplersState oldSamplings, SamplersState sampling)
        {
            ISampler[] samplers = new ISampler[oldSamplings.NumberOfSamplers + sampling.NumberOfSamplers];

            for (int i = 0; i < oldSamplings.NumberOfSamplers; i++)
                samplers[i] = oldSamplings[i];
            for (int i = 0; i < sampling.NumberOfSamplers; i++)
                samplers[i + oldSamplings.NumberOfSamplers] = sampling[i];

            return new SamplersState(samplers);
        }

        public void Dispose()
        {
            foreach (var s in samplers)
                s.Texture.Dispose();
        }
    }

    public struct TextureFactorState
    {
        public static TextureFactorState Default { get { return new TextureFactorState(); } }

        public Vector4 Factor { get; set; }
    }

    public static class SamplerStatesExtensors
    {
        public static IEnumerable<ISampler> GetSamplers(this SamplersState state)
        {
            for (int i = 0; i < state.NumberOfSamplers; i++)
                yield return state[i];
        }
    }
}
