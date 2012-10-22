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
    public class Viewing : Effect<ViewState>
    {
        protected Viewing() : base() { }

        protected Viewing(ViewState state) : base(state) { }

        public static explicit operator Viewing(Matrix4x4 viewMatrix)
        {
            return new Viewing(new ViewState(viewMatrix));
        }

        public static Viewing LookAtLH(Vector3 from, Vector3 target, Vector3 up)
        {
            return (Viewing)Matrices.LookAtLH(from, target, up);
        }

        public static Viewing LookAtRH(Vector3 from, Vector3 target, Vector3 up)
        {
            return (Viewing)Matrices.LookAtRH(from, target, up);
        }
    }

    public class Camera : Viewing
    {
        public Camera() : this ((Vector3)Vectors.Front, (Vector3)Vectors.O, (Vector3)Vectors.Up)
        {
        }

        public static new Camera LookAt(Vector3 from, Vector3 target, Vector3 up)
        {
            Camera camera = new Camera(from, target, up);
            return camera;
        }

        Vector3 from, target, up;

        public Vector3 Normal
        {
            get { return up; }
            set { up = value; UpdateState(); }
        }

        private void UpdateState()
        {
            this.state = new ViewState(Matrices.LookAtLH(from, target, up));
        }

        public Vector3 Direction
        {
            get { return Target - Position; }
            set { Target = Position + value; }
        }

        public void Move(Vector3 direction)
        {
            from = from + direction;
            target = target + direction;
            up = (Vector3)Vectors.Up;
            UpdateState();
        }

        public Vector3 Target
        {
            get { return target; }
            set { target = value; UpdateState(); }
        }

        public Vector3 Position
        {
            get { return from; }
            set { from = value; UpdateState(); }
        }

        public void LookAt(float x, float y, float z)
        {
            Target = new Vector3(x, y, z);
        }

        public Camera(Vector3 from, Vector3 target, Vector3 up)
            :base (new ViewState (Matrices.LookAtLH (from, target, up)))
        {
            this.from = from;
            this.target = target;
            this.up = up;
        }

        public void RotateAroundTarget(float p, Vector3 vector)
        {
            this.Position = (Vector3)GMath.mul(new Vector4((this.Position - this.Target), 1), Matrices.Rotate(p, vector)) + this.target;
        }
    }
}
