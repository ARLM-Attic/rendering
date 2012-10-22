using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Rendering.Effects.Geometry
{
    /// <summary>
    /// You are not intendend to use this interface
    /// </summary>
    public interface IPrimitive
    {
    }

    /// <summary>
    /// You are not intendend to use this interface
    /// </summary>
    public interface ISinglePrimitive : IPrimitive
    {
    }

    /// <summary>
    /// You are not intendend to use this interface
    /// </summary>
    public interface IStream
    {
    }

    /// <summary>
    /// You are not intendend to use this interface
    /// </summary>
    public interface ISingleStream : IStream
    {
    }

    public struct Point<T> : ISinglePrimitive where T : struct
    {
        public readonly T Vertex;

        public Point(T vertex)
        {
            this.Vertex = vertex;
        }
    }

    public struct Segment<T> : ISinglePrimitive where T : struct
    {
        public readonly T Vertex1;
        public readonly T Vertex2;

        public Segment(T vertex1, T vertex2)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
        }
    }

    public struct Triangle<T> : ISinglePrimitive where T : struct
    {
        public readonly T Vertex1;
        public readonly T Vertex2;
        public readonly T Vertex3;

        public Triangle(T vertex1, T vertex2, T vertex3)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;
        }
    }

    public struct SegmentWithAdjacency<T> : IPrimitive
    {
        public readonly T Vertex1;
        public readonly T Vertex2;

        public readonly T Adj1;
        public readonly T Adj2;

        public SegmentWithAdjacency(T vertex1, T vertex2, T adj1, T adj2)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Adj1 = adj1;
            this.Adj2 = adj2;
        }
    }

    public struct TriangleWithAdjacency<T> : IPrimitive
    {
        public readonly T Vertex1;
        public readonly T Vertex2;
        public readonly T Vertex3;

        public readonly T Adj1;
        public readonly T Adj2;
        public readonly T Adj3;

        public TriangleWithAdjacency(T vertex1, T vertex2, T vertex3, T adj1, T adj2, T adj3)
        {
            this.Vertex1 = vertex1;
            this.Vertex2 = vertex2;
            this.Vertex3 = vertex3;

            this.Adj1 = adj1;
            this.Adj2 = adj2;
            this.Adj3 = adj3;
        }
    }

    public class Stream<P> where P : ISinglePrimitive
    {
        List<P> __DebugIntendendOnlyList = new List<P>();
        public void Add(P primitive)
        {
            __DebugIntendendOnlyList.Add(primitive);
        }
    }

    public class StripStream<T> where T : struct
    {
        List<T> __DebugIntendendOnlyList = new List<T>();

        public void Append(T vertex)
        {
            __DebugIntendendOnlyList.Add(vertex);
        }
    }

    public delegate void ProcessGeometry<P, S>(P primitive, S stream)
        where P : IPrimitive
        where S : IStream;
}
