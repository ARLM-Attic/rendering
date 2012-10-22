using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Compilers.FlowAnalysis
{
    public class ControlFlowEdge<T>
    {
        public readonly ControlFlowNode<T> Source;
        public readonly ControlFlowNode<T> Target;

        public ControlFlowEdge(ControlFlowNode<T> source, ControlFlowNode<T> target)
        {
            this.Source = source;
            this.Target = target;
        }
    }
}
