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
using System.Rendering.Resourcing;

namespace System.Rendering.Effects
{
    public class Transforming : AppendableEffect<WorldState>
    {
        private Transforming(Matrix4x4 transform)
            : base(new WorldState(transform))
        {
            AppendMode = Effects.AppendMode.Append;
        }

        protected override WorldState Append(WorldState previus)
        {
            switch (AppendMode)
            {
                case Effects.AppendMode.Append:
                    return new WorldState(GMath.mul(state.Matrix, previus.Matrix));
                case Effects.AppendMode.Prepend:
                    return new WorldState(GMath.mul(previus.Matrix, state.Matrix));
                case Effects.AppendMode.None:
                    return previus;
                case Effects.AppendMode.Replace:
                    return state;
                default: throw new ArgumentException();
            }
        }

        public static explicit operator Transforming(Matrix4x4 transform)
        {
            return new Transforming(transform);
        }

        public static Transforming Fixed(Matrix4x4 transform)
        {
            return new Transforming(transform) { AppendMode = AppendMode.Replace };
        }

        public static Transforming Translate(FLOATINGTYPE x, FLOATINGTYPE y, FLOATINGTYPE z)
        {
            return (Transforming)Matrices.Translate(x, y, z);
        }
        public static Transforming Translate(Vector3 offset)
        {
            return (Transforming)Matrices.Translate(offset);
        }
        public static Transforming Rotate(FLOATINGTYPE angle, Vector3 direction)
        {
            return (Transforming)Matrices.Rotate(angle, direction);
        }

        public static Transforming Rotate(FLOATINGTYPE angle, Axis axis)
        {
            return (Transforming)Matrices.Rotate(angle,
                new Vector3(
                1 * Convert.ToInt32((axis & Axis.X) != 0),
                1 * Convert.ToInt32((axis & Axis.Y) != 0),
                1 * Convert.ToInt32((axis & Axis.Z) != 0)));
        }

        public static Transforming Scale(FLOATINGTYPE sx, FLOATINGTYPE sy, FLOATINGTYPE sz)
        {
            return (Transforming)Matrices.Scale(sx, sy, sz);
        }

        public static Transforming Scale(FLOATINGTYPE s)
        {
            return Scale(s, s, s);
        }
    }

    public enum Axis { X = 1, Y = 2, Z = 4}
}
