using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.Shaders.Info
{
    public class ShaderParameter
    {
        internal ShaderParameter(ShaderType parameterType, string name, Semantic semantic, ParameterModifier modifier)
        {
            this.ParameterType = parameterType;
            this.Name = name;
            this.Semantic = semantic;
            this.Modifier = modifier;
        }

        public ShaderType ParameterType
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            private set;
        }

        public Semantic Semantic
        {
            get;
            private set;
        }

        public ParameterModifier Modifier
        {
            get;
            private set;
        }
    }
}
