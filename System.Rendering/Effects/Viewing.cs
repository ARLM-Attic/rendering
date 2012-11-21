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
using System.Rendering.Effects;

namespace System.Rendering.Effects
{
    public class Viewing : Effect<ViewState>
    {
        protected Viewing() : base() { }

        protected Viewing(ViewState state) : base(state) { }

        public static explicit operator Viewing(Matrix4x4 viewMatrix)
        {
            return new Viewing(new ViewState(viewMatrix));
        }
    }
}

namespace System.Rendering
{
    public enum CoordinatesSystemHandRule
    {
        RightHandled,
        LeftHandled
    }

    public class Cameras
    {
        public static Viewing LookAt(Vector3 from, Vector3 target, Vector3 up)
        {
            return LookAt(from, target, up, CoordinatesSystemHandRule.LeftHandled);
        }

        public static Viewing LookAt(Vector3 from, Vector3 target, Vector3 up, CoordinatesSystemHandRule handRule)
        {
            switch (handRule){
                case CoordinatesSystemHandRule.LeftHandled: return (Viewing)Matrices.LookAtLH(from, target, up);
                case CoordinatesSystemHandRule.RightHandled: return (Viewing)Matrices.LookAtLH(from, target, up);
                default: throw new ArgumentOutOfRangeException("handRule");
            }
        }

        public static Projecting Perspective (float fieldOfView, float aspectRatio, float nearPlane, float farPlane, CoordinatesSystemHandRule handRule)
        {
            switch (handRule)
            {
                case CoordinatesSystemHandRule.LeftHandled: return (Projecting)Matrices.PerspectiveFovLH(fieldOfView, aspectRatio, nearPlane, farPlane);
                case CoordinatesSystemHandRule.RightHandled: return (Projecting)Matrices.PerspectiveFovRH(fieldOfView, aspectRatio, nearPlane, farPlane);
                default: throw new ArgumentOutOfRangeException("handRule");
            }
        }

        public static Projecting Perspective(float fieldOfView, float aspectRatio, float nearPlane, float farPlane)
        {
            return Perspective(fieldOfView, aspectRatio, nearPlane, farPlane, CoordinatesSystemHandRule.LeftHandled);
        }

        public static Projecting Perspective(float fieldOfView, float aspectRatio)
        {
            return Perspective(fieldOfView, aspectRatio, 0.1f, 1000);
        }

        public static Projecting Perspective(float aspectRatio)
        {
            return Perspective(GMath.PiOver4, aspectRatio);
        }

        public static Projecting Orthographic(float width, float height,float nearPlane, float farPlane,  CoordinatesSystemHandRule handRule )
        {
            switch (handRule)
            {
                case CoordinatesSystemHandRule.LeftHandled: return Projecting.OrthoLH(width, height, nearPlane, farPlane);
                case CoordinatesSystemHandRule.RightHandled: return Projecting.OrthoRH(width, height, nearPlane, farPlane);
                default: throw new ArgumentOutOfRangeException("handRule");
            }
        }
    }
}