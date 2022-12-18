using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace AdventOfCode2022
{
    public record Point(int x, int y)
    {
        public Point((int, int) xy) : this(xy.Item1, xy.Item2) { }
        
        public int ManhattanLengthTo(Point p)
        {
            return Math.Abs(p.x - x) + Math.Abs(p.y - y);
        }
        public int ManhattanLengthTo(int x, int y)
        {
            return Math.Abs(x - this.x) + Math.Abs(y - this.y);
        }
    }

    public record Point3D(int x, int y, int z)
    {
        public Point3D((int x, int y, int z) t) : this(t.x, t.y, t.z) { }
        public IEnumerable<Point3D> Neighbours()
        {
            yield return new Point3D(x + 1, y, z);
            yield return new Point3D(x - 1, y, z);
            yield return new Point3D(x, y + 1, z);
            yield return new Point3D(x, y - 1, z);
            yield return new Point3D(x, y, z + 1);
            yield return new Point3D(x, y, z - 1);
        }
    }

    public record Line(Point start, Point end)
    {
        public Line((int, int) start, (int, int) end) : this(new Point(start.Item1, start.Item2), new Point(end.Item1, end.Item2)) { }
        public bool IsHorizontal => start.y == end.y;
        public bool IsVertical => start.x == end.x;
        public Point LeftMost => start.x <= end.x ? start : end;
        public Point RightMost => start.x >= end.x ? start : end;
        public Point TopMost => start.y <= end.y ? start : end;
        public Point BottomMost => start.y >= end.y ? start : end;
    }
    
    public record Square(int left, int top, int right, int bottom)
    {
        public int Width => right - left;
        public int Height => bottom - top;
        public bool Contains(int x, int y)
        {
            return left <= x && x <= right && top <= y && y <= bottom;
        }
    }
    public record Rhombus
    {
        public Point[] vertices { get; init; }
        public Line[] edges { get; init; }
        public Point center { get; init; }
        private int indexLeftMost = 0;
        private int indexTopMost = 0;
        private int indexRightMost = 0;
        private int indexBottomMost = 0;
        public Rhombus(params Point[] vertices)
        {
            this.vertices = vertices;
            edges = new Line[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                if (vertices[indexLeftMost].x > vertices[i].x) indexLeftMost = i;
                if (vertices[indexRightMost].x < vertices[i].x) indexRightMost = i;
                if (vertices[indexTopMost].y > vertices[i].y) indexTopMost = i;
                if (vertices[indexBottomMost].y < vertices[i].y) indexBottomMost = i;
                edges[i] = new Line(vertices[i], vertices[(i + 1) % vertices.Length]);
            }
            center = new Point(
                vertices[indexLeftMost].x + (vertices[indexRightMost].ManhattanLengthTo(vertices[indexLeftMost]) / 2),
                vertices[indexTopMost].y + (vertices[indexBottomMost].ManhattanLengthTo(vertices[indexBottomMost]) / 2));
        }
        public int Width => vertices[indexLeftMost].ManhattanLengthTo(vertices[indexRightMost]);
        public int Height => vertices[indexTopMost].ManhattanLengthTo(vertices[indexBottomMost]);
        //public bool Contains(int x, int y)
        //{
        //    return vertices[indexLeftMost] <= x && x <= vertices[indexRightMost] && top <= y && y <= bottom;
        //}
    }
}
