using System;
using System.Collections.Generic;
using System.Text;

namespace System.Maths
{
    public class IntersectInfo : IComparable<IntersectInfo>
    {
        public readonly float U, V, Distance;
        public Vector3 Intersection { get { return Triangle.V1 * U + Triangle.V2 * V + Triangle.V3 * (1 - U - V); } }
        public int TriangleIndex;
        public Triangle Triangle;
        public IntersectInfo(float u, float v, float dist, Triangle triangle)
        {
            this.TriangleIndex = 0;
            this.Triangle = triangle;
            this.U = u;
            this.V = v;
            this.Distance = dist;
        }

        #region IComparable<IntersectInfo> Members

        public int CompareTo(IntersectInfo other)
        {
            return Distance.CompareTo(other.Distance);
        }

        #endregion
    }
}
