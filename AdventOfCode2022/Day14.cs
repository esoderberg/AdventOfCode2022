using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day14 : AoCDay
    {

        public static List<Line> Parse(List<string> input)
        {
            List<Line> rockLines = new();
            foreach (var line in input)
            {
                var points = line.Split(" -> ").Select(s => s.Split(",").Select(int.Parse).Tuple()).Select(xy => new Point(xy));
                var lines = points.Zip(points.Skip(1)).Select(points => new Line(points.First, points.Second));
                rockLines.AddRange(lines);
            }
            return rockLines;
        }

        public static Square DetermineBounds(List<Line> lines)
        {
            var left = int.MaxValue;
            var top = 0; // It always starts at 0
            var right = int.MinValue;
            var bottom = int.MinValue;
            foreach (var line in lines)
            {
                if (line.start.x < left) left = line.start.x;
                if (line.end.x < left) left = line.end.x;
                if (line.start.x > right) right = line.start.x;
                if (line.end.x > right) right = line.end.x;
                if (line.start.y < top) top = line.start.y;
                if (line.end.y < top) top = line.start.y;
                if (line.start.y > bottom) bottom = line.start.y;
                if (line.end.y > bottom) bottom = line.end.y;
            }
            return new Square(left, top, right, bottom);
        }


        public static (Square,char[,]) CreateGrid(List<Line> rockLines)
        {
            var bounds = DetermineBounds(rockLines);
            var grid = new char[bounds.Width + 1, bounds.Height + 1];
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    grid[x, y] = '.';
                }
            }
            foreach (var line in rockLines)
            {
                if (line.IsHorizontal)
                {
                    for (int i = line.LeftMost.x; i <= line.RightMost.x; i++)
                    {
                        grid[i - bounds.left, line.start.y - bounds.top] = '#';
                    }
                }
                else if (line.IsVertical)
                {
                    for (int i = line.TopMost.y; i <= line.BottomMost.y; i++)
                    {
                        grid[line.start.x - bounds.left, i - bounds.top] = '#';
                    }
                }
            }
            return (bounds, grid);
        }

        public static void PrintGrid(char[,] grid)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    System.Diagnostics.Debug.Write(grid[x, y]);
                }
                System.Diagnostics.Debug.WriteLine("");
            }
        }
        public static int Simulate(char[,] grid, Square bounds)
        {
            var gridBounds = new Square(0, 0, bounds.right - bounds.left, bounds.bottom - bounds.top);
            var grainsDropped = 0;

            while (grid[500 - bounds.left, 0] != 'o')
            {
                int sandX = 500 - bounds.left;

                for (int i = 0; i <= gridBounds.Height; i++)
                {
                    var tile = grid[sandX, i];
                    if (tile == '#' || tile == 'o')
                    {
                        // Check tile to the left of the rock/sand
                        if (sandX - 1 < 0) return grainsDropped;
                        else if (grid[sandX - 1, i] == '.')
                            sandX--;
                        else if (sandX + 1 > gridBounds.right) return grainsDropped;
                        else if (grid[sandX + 1, i] == '.')
                            sandX++;
                        else if (i == 0) return grainsDropped;
                        else
                        {
                            grid[sandX, i - 1] = 'o'; // Comes to rest at the tile above we saw the rock/sand
                            grainsDropped++;
                            break;
                        }
                    } else if (tile == '.' && i == bounds.Height) return grainsDropped;
                }
                //PrintGrid(grid);
            }
            
            return grainsDropped;
        }

        public static string ExecutePart1(List<string> input)
        {
            var lines = Parse(input);
            var (bounds, grid) = CreateGrid(lines);
            //PrintGrid(grid);
            var grains = Simulate(grid, bounds);
            return grains.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            var lines = Parse(input);
            var firstBounds = DetermineBounds(lines);
            // Yes this is hacky
            lines.Add(new Line((firstBounds.left - 200, firstBounds.Height + 2), (firstBounds.right + 200, firstBounds.Height + 2)));
            var (bounds, grid) = CreateGrid(lines);
            //PrintGrid(grid);
            var grains = Simulate(grid, bounds);
            return grains.ToString();
        }
    }
}
