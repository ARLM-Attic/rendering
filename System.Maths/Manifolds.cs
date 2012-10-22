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
using System.Linq;
using System.Text;
using System.Maths;

namespace System.Maths
{
    public interface IManifold
    {
        int Dimension { get; }

        IManifold this[params FLOATINGTYPE[] parameters] { get; }
    }

    public abstract class Manifold : IManifold
    {
        public abstract int Dimension { get; }

        IManifold IManifold.this[params FLOATINGTYPE[] parameters]
        {
            get
            {
                return Slice(parameters);
            }
        }

        protected abstract IManifold Slice(params FLOATINGTYPE[] parameters);

        public static Manifold0 Zero { get { return Point(new Vector3(0, 0, 0)); } }

        public static Manifold0 Point(Func<Vector3> positionFunction)
        {
            return new Manifold0(positionFunction);
        }

        public static Manifold0 Point(Vector3 position)
        {
            return new Manifold0(() => position);
        }

        public static Manifold1 Curve(Func<FLOATINGTYPE, Vector3> positionFunction)
        {
            return new Manifold1(positionFunction);
        }

        public static Manifold2 Surface(Func<FLOATINGTYPE, FLOATINGTYPE, Vector3> positionFunction)
        {
            return new Manifold2(positionFunction);
        }

        public static Manifold3 Surface(Func<FLOATINGTYPE, FLOATINGTYPE, FLOATINGTYPE, Vector3> positionFunction)
        {
            return new Manifold3(positionFunction);
        }
    }

    public delegate Vector1 Demote1();
    public delegate Vector2 Demote2();
    public delegate Vector3 Demote3();
    public delegate Vector3 Promote0(Vector3 position);
    public delegate Vector3 Promote1(Vector3 position, FLOATINGTYPE u);
    public delegate Vector3 Promote2(Vector3 position, FLOATINGTYPE u, FLOATINGTYPE v);
    public delegate Vector3 Promote3(Vector3 position, FLOATINGTYPE u, FLOATINGTYPE v, FLOATINGTYPE w);

    public class Manifold0 : Manifold
    {
        internal Manifold0(Func<Vector3> positionFunction)
        {
            this.positionFunction = positionFunction;
        }

        Func<Vector3> positionFunction;

        public Vector3 Position { get { return positionFunction(); } }

        public sealed override int Dimension
        {
            get { return 0; }
        }

        protected sealed override IManifold Slice(params FLOATINGTYPE[] parameters)
        {
            return this;
        }

        public Manifold0 Promote(Promote0 promotion)
        {
            return new Manifold0(() => promotion(Position));
        }

        public Manifold1 Promote(Promote1 promotion)
        {
            return new Manifold1((u) => promotion(Position, u));
        }

        public Manifold2 Promote(Promote2 promotion)
        {
            return new Manifold2((u, v) => promotion(Position, u, v));
        }

        public Manifold3 Promote(Promote3 promotion)
        {
            return new Manifold3((u, v, w) => promotion(Position, u, v, w));
        }
    }

    public class Manifold1 : Manifold
    {
        internal Manifold1(Func<FLOATINGTYPE, Vector3> positionFunction)
        {
            this.positionFunction = positionFunction;
        }

        Func<FLOATINGTYPE, Vector3> positionFunction;

        public override int Dimension
        {
            get { return 1; }
        }

        public Vector3 GetPositionAt(FLOATINGTYPE u)
        {
            return positionFunction(u);
        }

        protected override IManifold Slice(params FLOATINGTYPE[] parameters)
        {
            switch (parameters.Length)
            {
                case 0: return this;
                default: return Slice(() => new Vector1(parameters[0]));
            }
        }

        public Manifold0 this[FLOATINGTYPE u]
        {
            get { return new Manifold0(() => GetPositionAt(u)); }
        }

        public Manifold0 Slice(Demote1 demote)
        {
            return new Manifold0(() => GetPositionAt(demote().X));
        }

        public Manifold0 Demote(Demote1 demote)
        {
            return new Manifold0(() => GetPositionAt(demote().X));
        }

        public Manifold1 Promote(Promote0 promotion)
        {
            return new Manifold1((u) => promotion(GetPositionAt(u)));
        }

        public Manifold2 Promote(Promote1 promotion)
        {
            return new Manifold2((u, v) => promotion(GetPositionAt(u), v));
        }

        public Manifold3 Promote(Promote2 promotion)
        {
            return new Manifold3((u, v, w) => promotion(GetPositionAt(u), u, v));
        }
    }

    public class Manifold2 : Manifold
    {
        internal Manifold2(Func<FLOATINGTYPE, FLOATINGTYPE, Vector3> positionFunction)
        {
            this.positionFunction = positionFunction;
        }

        Func<FLOATINGTYPE, FLOATINGTYPE, Vector3> positionFunction;

        public override int Dimension
        {
            get { return 2; }
        }

        public Vector3 GetPositionAt(FLOATINGTYPE u, FLOATINGTYPE v)
        {
            return positionFunction(u, v);
        }

        protected override IManifold Slice(params FLOATINGTYPE[] parameters)
        {
            switch (parameters.Length)
            {
                case 0: return this;
                case 1: return Slice(() => new Vector1(parameters[0]));
                default: return Slice(() => new Vector2(parameters[0], parameters[1]));
            }
        }

        public Manifold0 this[FLOATINGTYPE u, FLOATINGTYPE v]
        {
            get
            {
                return new Manifold0(() => GetPositionAt(u, v));
            }
        }

        public Manifold0 Slice(Demote2 demote)
        {
            return new Manifold0(() =>
            {
                Vector2 v = demote();
                return GetPositionAt(v.X, v.Y);
            });
        }

        public Manifold0 Demote(Demote2 demote)
        {
            return new Manifold0(() =>
            {
                Vector2 v = demote();
                return GetPositionAt(v.X, v.Y);
            });
        }

        public Manifold1 this[FLOATINGTYPE u]
        {
            get
            {
                return new Manifold1((v) =>
                {
                    return GetPositionAt(u, v);
                }
                    );
            }
        }

        public Manifold1 Slice(Demote1 demote)
        {
            return new Manifold1((v) =>
            {
                Vector1 par = demote();
                return GetPositionAt(par.X, v);
            });
        }

        public Manifold1 Demote(Demote1 demote)
        {
            return new Manifold1((u) =>
            {
                Vector1 v = demote();
                return GetPositionAt(u, v.X);
            });
        }

        public Manifold2 Promote(Promote0 promotion)
        {
            return new Manifold2((u, v) => promotion(GetPositionAt(u, v)));
        }

        public Manifold3 Promote(Promote1 promotion)
        {
            return new Manifold3((u, v, w) => promotion(GetPositionAt(u, v), w));
        }
    }

    public class Manifold3 : Manifold
    {
        internal Manifold3(Func<FLOATINGTYPE, FLOATINGTYPE, FLOATINGTYPE, Vector3> positionFunction)
        {
            this.positionFunction = positionFunction;
        }

        Func<FLOATINGTYPE, FLOATINGTYPE, FLOATINGTYPE, Vector3> positionFunction;

        public override int Dimension
        {
            get { return 3; }
        }

        public Vector3 GetPositionAt(FLOATINGTYPE u, FLOATINGTYPE v, FLOATINGTYPE w)
        {
            return positionFunction(u, v, w);
        }

        protected override IManifold Slice(params FLOATINGTYPE[] parameters)
        {
            switch (parameters.Length)
            {
                case 0: return this;
                case 1: return Slice(() => new Vector1(parameters[0]));
                case 2: return Slice(() => new Vector2(parameters[0], parameters[1]));
                default: return Slice(() => new Vector3(parameters[0], parameters[1], parameters[2]));
            }
        }

        public Manifold0 this[FLOATINGTYPE u, FLOATINGTYPE v, FLOATINGTYPE w]
        {
            get
            {
                return new Manifold0(() => GetPositionAt(u, v, w));
            }
        }

        public Manifold0 Slice(Demote3 demote)
        {
            return new Manifold0(() =>
            {
                Vector3 v = demote();
                return GetPositionAt(v.X, v.Y, v.Z);
            });
        }

        public Manifold0 Demote(Demote3 demote)
        {
            return new Manifold0(() =>
            {
                Vector3 v = demote();
                return GetPositionAt(v.X, v.Y, v.Z);
            });
        }

        public Manifold1 this[FLOATINGTYPE u, FLOATINGTYPE v]
        {
            get
            {
                return new Manifold1((w) => GetPositionAt(u, v, w));
            }
        }

        public Manifold1 Slice(Demote2 demote)
        {
            return new Manifold1((w) =>
            {
                Vector2 v = demote();
                return GetPositionAt(v.X, v.Y, w);
            });
        }

        public Manifold1 Demote(Demote2 demote)
        {
            return new Manifold1((u) =>
            {
                Vector2 v = demote();
                return GetPositionAt(u, v.X, v.Y);
            });
        }

        public Manifold2 this[FLOATINGTYPE u]
        {
            get { return new Manifold2((v, w) => GetPositionAt(u, v, w)); }
        }

        public Manifold2 Slice(Demote1 demote)
        {
            return new Manifold2((u, v) =>
            {
                Vector1 param = demote();
                return GetPositionAt(param.X, u, v);
            });
        }

        public Manifold2 Demote(Demote1 demote)
        {
            return new Manifold2((u, v) =>
            {
                Vector1 param = demote();
                return GetPositionAt(u, v, param.X);
            });
        }

        public Manifold3 Promote(Promote0 promotion)
        {
            return new Manifold3((u, v, w) => promotion(GetPositionAt(u, v, w)));
        }
    }
}
