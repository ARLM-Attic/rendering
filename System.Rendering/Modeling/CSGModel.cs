using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;
using System.Maths;

namespace System.Rendering.Modeling
{
    class CSGOperations
    {
        #region BSP Tools

        class Plane
        {
            public Vector3 _normal;
            public float _w;

            public const float EPSILON = 1e-5f;

            public static Plane FromPoints(Vector3 a, Vector3 b, Vector3 c)
            {
                var n = GMath.normalize(GMath.cross((c - a), (b - a)));
                return new Plane(n, GMath.dot(n, a));
            }

            public Plane(Vector3 n, float w)
            {
                _normal = n;
                _w = w;
            }

            public Plane Clone()
            {
                return new Plane(_normal, _w);
            }

            public void Flip()
            {
                _normal = -1 * _normal;
                _w = -_w;
            }

            public void SplitPolygon(Polygon polygon, List<Polygon> coplanarFront, List<Polygon> coplanarBack, List<Polygon> front, List<Polygon> back)
            {
                const int COPLANAR = 0;
                const int FRONT = 1;
                const int BACK = 2;
                const int SPANNING = 3;

                var polygonType = 0;
                var types = new List<int>();
                foreach (var v in polygon._vertices)
                {
                    var t = GMath.dot(_normal, v._position) - _w;
                    var type = (t < -EPSILON) ? BACK : (t > EPSILON ? FRONT : COPLANAR);
                    polygonType |= type;
                    types.Add(type);
                }

                switch (polygonType)
                {
                    case COPLANAR:
                        if (GMath.dot(_normal, polygon._plane._normal) > 0)
                            coplanarFront.Add(polygon);
                        else
                            coplanarBack.Add(polygon);
                        break;

                    case FRONT:
                        front.Add(polygon);
                        break;

                    case BACK:
                        back.Add(polygon);
                        break;

                    case SPANNING:
                        var f = new List<Vertex>();
                        var b = new List<Vertex>();

                        for (int i = 0, len = polygon._vertices.Length; i < len; i++)
                        {
                            var j = (i + 1) % len;

                            var ti = types[i];
                            var tj = types[j];

                            var vi = polygon._vertices[i];
                            var vj = polygon._vertices[j];

                            if (ti != BACK) f.Add(vi);
                            if (ti != FRONT) b.Add(ti != BACK ? vi.Clone() : vi);

                            if ((ti | tj) == SPANNING)
                            {
                                var t = (_w - GMath.dot(_normal, vi._position)) / GMath.dot(_normal, vj._position - vi._position);
                                var v = vi.Interpolate(vj, t);
                                f.Add(v);
                                b.Add(v.Clone());
                            }
                        }

                        if (f.Count >= 3) front.Add(new Polygon(f.ToArray(), polygon._shared));
                        if (b.Count >= 3) back.Add(new Polygon(b.ToArray(), polygon._shared));

                        break;
                    default:
                        break;
                }
            }

            public override string ToString()
            {
                return String.Format("({0}, {1}, {2}, {3})", _normal.X, _normal.Y, _normal.Z, _w);
            }
        }

        class Node
        {
            public Plane _plane;
            public Node _front;
            public Node _back;
            public List<Polygon> _polygons = new List<Polygon>();

            public Node Clone()
            {
                return new Node()
                {
                    _plane = _plane != null ? _plane.Clone() : null,
                    _front = _front != null ? _front.Clone() : null,
                    _back = _back != null ? _back.Clone() : null,
                    _polygons = _polygons.Select(p => p.Clone()).ToList()
                };
            }

            public void InvertNormals()
            {
                if (_plane == null)
                    return;

                _polygons.ForEach(p => p.Flip());
                //_plane.Flip();
                if (_front != null) _front.InvertNormals();
                if (_back != null) _back.InvertNormals();
                //var temp = _front;
                //_front = _back;
                //_back = temp;
            }

            public enum Side
            {
                None = 0,
                Outside = 1,
                Inside = 2,
                Both = Outside | Inside
            }

            public List<Polygon> SlicePolygons(List<Polygon> polygons, Side side)
            {
                var front = new List<Polygon>();
                var back = new List<Polygon>();
                foreach (var p in polygons)
                    _plane.SplitPolygon(p, front, back, front, back);


                var backClone = new List<Polygon>(back);
                var frontClone = new List<Polygon>(front);

                if (side == Side.Inside || side == Side.None)
                    front.Clear();
                if (side == Side.Outside || side == Side.None)
                    back.Clear();

                if (_front != null)
                    front = _front.SlicePolygons(frontClone, side);
                if (_back != null)
                    back = _back.SlicePolygons(backClone, side);

                return front.Concat(back).ToList();
            }

            public IEnumerable<Polygon> GetPolygons(Node bsp, Side keepSide)
            {
                var polygons = bsp.SlicePolygons(AllPolygons(), keepSide);

                return polygons;
            }

            public List<Polygon> AllPolygons()
            {
                var polygons = _polygons.Select(p => p.Clone()).ToList();
                if (_front != null) polygons = polygons.Concat(_front.AllPolygons()).ToList();
                if (_back != null) polygons = polygons.Concat(_back.AllPolygons()).ToList();
                return polygons;
            }

            public void Build(IEnumerable<Polygon> polygons)
            {
                if (polygons == null || !polygons.Any())
                    return;

                if (_plane == null)
                    _plane = polygons.First ()._plane.Clone();

                var front = new List<Polygon>();
                var back = new List<Polygon>();
                foreach (var p in polygons)
                    _plane.SplitPolygon(p, _polygons, _polygons, front, back);

                if (front.Count > 0)
                {
                    if (_front == null)
                        _front = new Node();
                    _front.Build(front);
                }
                if (back.Count > 0)
                {
                    if (_back == null)
                        _back = new Node();
                    _back.Build(back);
                }
            }

            public override string ToString()
            {
                return _plane != null ? _plane.ToString() : String.Empty;
            }
        }

        class Vertex
        {
            [Position]
            public Vector3 _position;
            [Normal]
            public Vector3 _normal;

            public Vertex(Vector3 pos, Vector3 n)
            {
                _position = pos;
                _normal = n;
            }

            public Vertex Clone()
            {
                return new Vertex(_position, _normal);
            }

            public void Flip()
            {
                _normal = -1 * _normal;
            }

            public Vertex Interpolate(Vertex other, float t)
            {
                var tv = new Vector3(t, t, t);
                return new Vertex(GMath.lerp(_position, other._position, tv), GMath.lerp(_normal, other._normal, tv));
            }

            public override string ToString()
            {
                return String.Format("[({0},{1},{2}), ({3},{4},{5})]", _position.X, _position.Y, _position.Z, _normal.X, _normal.Y, _normal.Z);
            }

            internal PositionNormalData ToData()
            {
                return new PositionNormalData { Position = _position, Normal = _normal };
            }
        }

        class Polygon
        {
            public Vertex[] _vertices;
            public bool _shared;
            public Plane _plane;

            public Polygon(Vertex[] vertices, bool shared)
            {
                _vertices = vertices;
                _shared = shared;
                _plane = Plane.FromPoints(vertices[0]._position, vertices[1]._position, vertices[2]._position);
            }

            public Polygon Clone()
            {
                var vertices = _vertices.Select(v => v.Clone()).ToArray();
                return new Polygon(vertices, _shared);
            }

            public void Flip()
            {
                _vertices = _vertices.Reverse().ToArray();
                foreach (var v in _vertices)
                    v.Flip();
                _plane.Flip();
            }

            public override string ToString()
            {
                return String.Join(", ", _vertices.AsEnumerable());
            }
        }

        class ModelToCSGTransformer : ITessellator, ITessellatorOf<Basic>
        {
            List<Polygon> polygons = new List<Polygon>();

            public IEnumerable<Polygon> Polygons { get { return polygons; } }

            public IRenderDevice Render
            {
                get { return null; }
            }

            public void Draw<GP>(GP primitive) where GP : struct, IGraphicPrimitive
            {
                if (this is ITessellatorOf<GP>)
                    ((ITessellatorOf<GP>)this).Draw(primitive);
            }

            public bool IsSupported<GP>() where GP : struct, IGraphicPrimitive
            {
                return typeof(GP) == typeof(Basic);
            }

            void ITessellatorOf<Basic>.Draw(Basic primitive)
            {
                foreach (var t in primitive.GetTriangles())
                {
                    var normal = t.Normal;
                    if (t.Normal.X != 0 || t.Normal.Y != 0 || t.Normal.Z != 0)
                        polygons.Add(new Polygon(new Vertex[] { new Vertex(t.V1, t.N1), new Vertex(t.V2, t.N2), new Vertex(t.V3, t.N3) }, true));
                }
            }
        }

        private static Node CreateNode(IModel model)
        {
            ModelToCSGTransformer transformer = new ModelToCSGTransformer();
            model.Tesselate(transformer);

            if (!transformer.Polygons.Any())
                return null;

            Node node = new Node();
            node.Build(transformer.Polygons);

            return node;
        }

        private static Mesh Construct(IEnumerable<Polygon> allPolygons)
        {
            List<PositionNormalData> vertexes = new List<PositionNormalData>();
            List<uint> indices = new List<uint>();
            foreach (var p in allPolygons)
            {
                uint startIndex = (uint)vertexes.Count;
                vertexes.Add(p._vertices[0].ToData());
                vertexes.Add(p._vertices[1].ToData());
                for (uint i = 2; i < p._vertices.Length; i++)
                {
                    vertexes.Add(p._vertices[i].ToData());
                    indices.AddRange(new uint[] { startIndex, startIndex + i - 1, startIndex + i });
                }
            }

            var m = new Mesh<PositionNormalData>(vertexes.ToArray(), indices.ToArray());
            //m.ComputeNormals();
            return m;
        }


        #endregion

        #region Operations

        public static Mesh Union(IModel model1, IModel model2)
        {
            Node a = CreateNode(model1);
            Node b = CreateNode(model2);

            var polygonsA = a.GetPolygons(b, Node.Side.Outside);
            var polygonsB = b.GetPolygons(a, Node.Side.Outside);

            return Construct(polygonsA.Concat(polygonsB));
        }

        public static Mesh Intersect(IModel model1, IModel model2)
        {
            Node a = CreateNode(model1);
            Node b = CreateNode(model2);

            var polygonsA = a.GetPolygons(b, Node.Side.Inside);
            var polygonsB = b.GetPolygons(a, Node.Side.Inside);

            return Construct(polygonsA.Concat(polygonsB));
        }

        public static Mesh Subtract(IModel model1, IModel model2)
        {
            Node a = CreateNode(model1);

            if (a == null) return Mesh<PositionNormalData>.Empty;

            Node b = CreateNode(model2);

            if (b == null)
                return model1.ToMesh();

            b.InvertNormals();

            var polygonsA = a.GetPolygons(b, Node.Side.Outside);
            var polygonsB = b.GetPolygons(a, Node.Side.Inside);

            return Construct(polygonsA.Concat(polygonsB));
        }

        #endregion
    }
}
