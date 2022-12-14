using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public record Point(int x, int y)
    {
        public Point((int, int) xy) : this(xy.Item1, xy.Item2) { }
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
    
}
