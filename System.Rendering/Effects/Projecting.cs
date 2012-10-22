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
using System.Rendering.RenderStates;
using System.Maths;

namespace System.Rendering.Effects
{
    public class Projecting : Effect<ProjectionState>
    {
        protected Projecting() : base() { }

        protected Projecting(ProjectionState state) : base(state) { }

        public static Projecting PerspectiveFovLH(FLOATINGTYPE fov, FLOATINGTYPE aspectRatio, FLOATINGTYPE znear, FLOATINGTYPE zfar)
        {
            return new Projecting(new ProjectionState(Matrices.PerspectiveFovLH(fov, aspectRatio, znear, zfar)));
        }

        public static Projecting OrthoLH(FLOATINGTYPE width, FLOATINGTYPE height, FLOATINGTYPE znear, FLOATINGTYPE zfar)
        {
            return new Projecting(new ProjectionState(Matrices.OrthoLH(width, height, znear, zfar)));
        }

        public static Projecting PerspectiveFovRH(FLOATINGTYPE fov, FLOATINGTYPE aspectRatio, FLOATINGTYPE znear, FLOATINGTYPE zfar)
        {
            return new Projecting(new ProjectionState(Matrices.PerspectiveFovRH(fov, aspectRatio, znear, zfar)));
        }

        public static Projecting OrthoRH(FLOATINGTYPE width, FLOATINGTYPE height, FLOATINGTYPE znear, FLOATINGTYPE zfar)
        {
            return new Projecting(new ProjectionState(Matrices.OrthoRH(width, height, znear, zfar)));
        }

        public static explicit operator Projecting (Matrix4x4 projectionMatrix)
        {
            return new Projecting(new ProjectionState(projectionMatrix));
        }
    }
}
