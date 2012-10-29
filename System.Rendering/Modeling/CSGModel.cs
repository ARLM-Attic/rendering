using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Rendering.Resourcing;
using System.Maths;

namespace System.Rendering.Modeling
{
	public class CSGModel : AllocateableBase, IModel
	{
		private Basic _primitive;
		private Node _root;

		public CSGModel(Basic primitive)
		{
			_primitive = primitive;

			InitializeRoot();
		}

		private CSGModel()
		{
		}

		private void InitializeRoot()
		{
			_root = new Node();
			_root.Build(GetPolygons());
		}

		private List<Polygon> GetPolygons()
		{
			var polygons = new List<Polygon>();
			foreach (var item in _primitive.GetTriangles())
			{
				polygons.Add(new Polygon(new[] {
					new Vertex(item.V1, item.Normal),
					new Vertex(item.V2, item.Normal),
					new Vertex(item.V3, item.Normal)
				}, false));
			}
			return polygons;
		}

		public static CSGModel Union(CSGModel csg1, CSGModel csg2)
		{
			if (csg1 == null)
				throw new ArgumentNullException("csg1");

			if (csg2 == null)
				throw new ArgumentNullException("csg2");

			var a = csg1.Clone();
			var b = csg2.Clone();
			a._root.ClipTo(b._root);
			b._root.ClipTo(a._root);
			b._root.Invert();
			b._root.ClipTo(a._root);
			b._root.Invert();
			a._root.Build(b._root.AllPolygons());
			return a;
		}

		public static CSGModel Intersect(CSGModel csg1, CSGModel csg2)
		{
			if (csg1 == null)
				throw new ArgumentNullException("csg1");

			if (csg2 == null)
				throw new ArgumentNullException("csg2");

			var a = csg1.Clone();
			var b = csg2.Clone();
			a._root.Invert();
			b._root.ClipTo(a._root);
			b._root.Invert();
			a._root.ClipTo(b._root);
			b._root.ClipTo(a._root);
			a._root.Build(b._root.AllPolygons());
			a._root.Invert();
			return a;
		}

		public static CSGModel Subtract(CSGModel csg1, CSGModel csg2)
		{
			if (csg1 == null)
				throw new ArgumentNullException("csg1");

			if (csg2 == null)
				throw new ArgumentNullException("csg2");

			var a = csg1.Clone();
			var b = csg2.Clone();
			a._root.Invert();
			a._root.ClipTo(b._root);
			b._root.ClipTo(a._root);
			b._root.Invert();
			b._root.ClipTo(a._root);
			b._root.Invert();
			a._root.Build(b._root.AllPolygons());
			a._root.Invert();
			return a;
		}

		public CSGModel Clone()
		{
			var csg = new CSGModel();
			csg._primitive = _primitive;
			csg._root = _root.Clone();
			return csg;
		}

		#region [ AllocateableBase overrides ]

		protected override Location OnClone(AllocateableBase toFill, IRenderDevice render)
		{
			var filling = toFill as CSGModel;

			var allocatedVB = (VertexBuffer)_primitive.VertexBuffer.Clone(render);
			var allocatedIB = (IndexBuffer)(_primitive.Indexes != null ? _primitive.Indexes.Clone(render) : null);
			filling._primitive = Basic.Create(_primitive.Type, allocatedVB, allocatedIB);

			if (filling._primitive.Indexes == null)
				return filling._primitive.VertexBuffer.Location;

			if (filling._primitive.VertexBuffer.Location == Location.Device && filling._primitive.Indexes.Location == Location.Device)
				return Location.Device;

			if (filling._primitive.VertexBuffer.Location == Location.User && filling._primitive.Indexes.Location == Location.User)
				return Location.User;

			return Location.Render;
		}

		protected override void OnDispose()
		{
			_primitive.VertexBuffer.Dispose();
			if (_primitive.Indexes != null)
				_primitive.Indexes.Dispose();
		}

		#endregion

		#region [ IModel Memebers ]

		public bool IsSupported(IRenderDevice device)
		{
			return device.TessellatorInfo.IsSupported<Basic>();
		}

		public void Tesselate(ITessellator tessellator)
		{
			tessellator.Draw(_primitive);
		}

		#endregion

		#region [ BSP ]

		class Vertex
		{
			public Vector3 _position;
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
						var	b = new List<Vertex>();

						for (int i = 0, len = polygon._vertices.Length; i < len; i++)
						{
							var j = (i + 1) % len;
							
							var ti = types[i];
							var	tj = types[j];

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

			public void Invert()
			{
				_polygons.ForEach(p => p.Flip());
				_plane.Flip();
				if (_front != null) _front.Invert();
				if (_back != null) _back.Invert();
				var temp = _front;
				_front = _back;
				_back = temp;
			}

			public List<Polygon> ClipPolygons(List<Polygon> polygons)
			{
				var front = new List<Polygon>();
				var back = new List<Polygon>();
				foreach (var p in _polygons)
					_plane.SplitPolygon(p, front, back, front, back);

				if (_front != null)
					front = _front.ClipPolygons(front);
				if (_back != null)
					back = _back.ClipPolygons(back);

				return front.Concat(back).ToList();
			}

			public void ClipTo(Node bsp)
			{
				_polygons = bsp.ClipPolygons(_polygons);
				if (_front != null) _front.ClipTo(bsp);
				if (_back != null) _back.ClipTo(bsp);
			}

			public List<Polygon> AllPolygons()
			{
				var polygons = _polygons.Select(p => p.Clone()).ToList();
				if (_front != null) polygons = polygons.Concat(_front.AllPolygons()).ToList();
				if (_back != null) polygons = polygons.Concat(_back.AllPolygons()).ToList();
				return polygons;
			}

			public void Build(List<Polygon> polygons)
			{
				if (polygons == null || polygons.Count == 0)
					return;

				if (_plane == null)
					_plane = polygons[0]._plane.Clone();

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
				if(back.Count > 0)
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

		#endregion
	}

	public static class CSGExtensions
	{
		public static CSGModel Union(this CSGModel csg1, CSGModel csg2)
		{
			return CSGModel.Union(csg1, csg2);
		}

		public static CSGModel Intersect(this CSGModel csg1, CSGModel csg2)
		{
			return CSGModel.Intersect(csg1, csg2);
		}

		public static CSGModel Subtract(this CSGModel csg1, CSGModel csg2)
		{
			return CSGModel.Subtract(csg1, csg2);
		}
	}
}
