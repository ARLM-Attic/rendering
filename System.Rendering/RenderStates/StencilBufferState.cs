using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    public struct StencilBufferState
    {
        public static readonly StencilBufferState Disable = new StencilBufferState(false, 0, 0, StencilOp.Zero, StencilOp.Zero, StencilOp.Zero, 0, Compare.Always);

        public static readonly StencilBufferState Enable = new StencilBufferState(true, 0, StencilBufferState.FullMask, StencilOp.Zero, StencilOp.Zero, StencilOp.Zero, 0, Compare.Always);

        public readonly StencilOp 
            StencilFails , 
            DepthFails ,
            Pass ;

        public const uint FullMask = uint.MaxValue;

        public readonly int Reference ;
        public readonly uint CompareMask ;
        public readonly uint WriteMask ;
        public readonly bool TestEnable ;

        public StencilBufferState SetEnable(bool enable)
        {
            return new StencilBufferState(enable, CompareMask, WriteMask, StencilFails, DepthFails, Pass, Reference, Function);
        }
        public StencilBufferState SetOperation(StencilOp stencil_fails, StencilOp depth_fails, StencilOp pass)
        {
            return new StencilBufferState(TestEnable, CompareMask, WriteMask, stencil_fails, depth_fails, pass, Reference, Function);
        }
        public StencilBufferState SetFunction(Compare function, int reference, uint mask)
        {
            return new StencilBufferState(TestEnable, mask, WriteMask, StencilFails, DepthFails, Pass, reference, function);
        }
        public StencilBufferState SetWriteMask(uint mask)
        {
            return new StencilBufferState(TestEnable, CompareMask, mask, StencilFails, DepthFails, Pass, Reference, Function);
        }

        public readonly Compare Function ;

        public static readonly StencilBufferState Default = new StencilBufferState(false, StencilBufferState.FullMask, StencilBufferState.FullMask, StencilOp.Keep, StencilOp.Keep, StencilOp.Keep, 0, Compare.Always).Clear();

        public StencilBufferState(bool enable, uint comparemask, uint writemask, StencilOp stencil_fails, StencilOp depth_fails, StencilOp pass, int reference, Compare function)
        {
            this.TestEnable = enable;
            this.StencilFails = stencil_fails;
            this.DepthFails = depth_fails;
            this.Pass = pass;
            this.Reference = reference;
            this.Function = function;
            this.CompareMask = comparemask;
            this.WriteMask = writemask;
            this._ClearOnSet = true;
        }

        bool _ClearOnSet;

        public bool ClearOnSet
        {
            get { return _ClearOnSet; }
        }

        public StencilBufferState Clear()
        {
            StencilBufferState state = new StencilBufferState(TestEnable, CompareMask, WriteMask, StencilFails, DepthFails, Pass, Reference, Function);
            state._ClearOnSet = true;
            return state;
        }

        public StencilBufferState DoNotClear()
        {
            StencilBufferState state = new StencilBufferState(TestEnable, CompareMask, WriteMask, StencilFails, DepthFails, Pass, Reference, Function);
            state._ClearOnSet = false;
            return state;
        }
    }

    public enum StencilOp
    {
        Zero,
        Keep,
        Replace,
        Increment,
        Decrement,
        Invert
    }
}
