using System;
using System.Collections.Generic;
using System.Text;

namespace System.Rendering.RenderStates
{
    /// <summary>
    /// Represents a state to manage depth buffer in standar raster pipeline.
    /// </summary>
    public struct DepthBufferState
    {
        /// <summary>
        /// Storages a default value for depth buffers with LessEqual comparison and 1f as default value for clearing.
        /// </summary>
        public static readonly DepthBufferState Default = new DepthBufferState(1, true, true, Compare.LessEqual).DoNotClear();
        /// <summary>
        /// Storages a enable value for depth buffers with LessEqual comparison and clearing the buffer with 1f depth.
        /// </summary>
        public static readonly DepthBufferState Enable = new DepthBufferState(1, true, true, Compare.LessEqual);
        /// <summary>
        /// Storages a disable value for depth buffers than can not update values and always passes the test.
        /// </summary>
        public static readonly DepthBufferState Disable = new DepthBufferState(1, false, false, Compare.Always).DoNotClear();
        /// <summary>
        /// Storages a readonly value for depth buffers than can not update values and pass when less or equal.
        /// </summary>
        public static readonly DepthBufferState ReadOnly = new DepthBufferState(1, true, false, Compare.LessEqual).DoNotClear();   
        
        /// <summary>
        /// Gets the default value used when clearing the buffer.
        /// </summary>
        public readonly double DefaultValue;

        bool _ClearOnSet ;

        /// <summary>
        /// Retrieves if the application of this state will clear the buffer.
        /// </summary>
        public bool ClearOnSet
        {
            get { return _ClearOnSet; }
        }

        /// <summary>
        /// Returns a copy of the current state that not clear when applied.
        /// </summary>
        /// <returns>A DepthBufferState value</returns>
        public DepthBufferState DoNotClear()
        {
            DepthBufferState dbs = new DepthBufferState(DefaultValue, EnableTest, WriteEnable, Function);
            dbs._ClearOnSet = false;
            return dbs;
        }

        /// <summary>
        /// Indicates the buffer can be updated with new values.
        /// </summary>
        public readonly bool WriteEnable;
        /// <summary>
        /// Indicates the buffer is used for testing by depth values.
        /// </summary>
        public readonly bool EnableTest;
        /// <summary>
        /// Indicates the comparison function used by the depth test.
        /// </summary>
        public readonly Compare Function;

        /// <summary>
        /// Initialize a DepthBufferState with necessary values.
        /// </summary>
        /// <param name="_default">Default value used when the buffer is cleared</param>
        /// <param name="enable">Indicates the depth test is enable</param>
        /// <param name="write">Indicates the depth buffer can be updated</param>
        /// <param name="function">Function used by the depth test</param>
        public DepthBufferState(double _default, bool enable, bool write, Compare function)
        {
            this._ClearOnSet = true;
            this.DefaultValue = _default;
            this.EnableTest = enable;
            this.WriteEnable = write;
            this.Function = function;            
        }
        /// <summary>
        /// Creates a copy but changing the default value.
        /// </summary>
        /// <param name="_default">Value used for clearing the buffer</param>
        /// <returns>A DepthBufferState structure with default value changed</returns>
        public DepthBufferState ChangeDefault(double _default)
        {
            return new DepthBufferState(_default, EnableTest, WriteEnable, Function);
        }
        /// <summary>
        /// Creates a copy but changing the enable value
        /// </summary>
        /// <param name="enable">Indicates the depth test is enable</param>
        /// <returns>A DepthBufferState structure with enable value changed</returns>
        public DepthBufferState ChangeEnable(bool enable)
        {
            return new DepthBufferState(DefaultValue, enable, WriteEnable, Function);
        }
        /// <summary>
        /// Creates a copy but changing the write value
        /// </summary>
        /// <param name="write">Indicates the buffer can be updated</param>
        /// <returns>A DepthBufferState structure with write value changed</returns>
        public DepthBufferState ChangeWriteEnable(bool write)
        {
            return new DepthBufferState(DefaultValue, EnableTest, write, Function);
        }
    }
}
