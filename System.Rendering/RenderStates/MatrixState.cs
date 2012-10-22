#define FLOAT

#if DECIMAL
using FLOATINGTYPE = System.Decimal;
#endif
#if FLOATINGTYPE
using FLOATINGTYPE = System.FLOATINGTYPE;
#endif
#if FLOAT
using FLOATINGTYPE = System.Single;
#endif

using System;
using System.Collections.Generic;
using System.Text;
using System.Maths;

namespace System.Rendering.RenderStates
{
    public struct WorldState
    {
        public static readonly WorldState Default = new WorldState(Matrices.I);

        public Matrix4x4 Matrix;

        public WorldState(Matrix4x4 matrix)
        {
            this.Matrix = matrix;
        }

        public static explicit operator WorldState(Matrix4x4 matrix)
        {
            return new WorldState(matrix);
        }
    }

    public struct ViewState
    {
        public static readonly ViewState Default = new ViewState(Matrices.I);

        public Matrix4x4 Matrix;

        public ViewState(Matrix4x4 matrix)
        {
            this.Matrix = matrix;
        }

        public static explicit operator ViewState(Matrix4x4 matrix)
        {
            return new ViewState(matrix);
        }
    }

    public struct ProjectionState
    {
        public static readonly ProjectionState Default = new ProjectionState(Matrices.I);

        public Matrix4x4 Matrix;

        public ProjectionState(Matrix4x4 matrix)
        {
            this.Matrix = matrix;
        }

        public static explicit operator ProjectionState(Matrix4x4 matrix)
        {
            return new ProjectionState(matrix);
        }
    }
}
