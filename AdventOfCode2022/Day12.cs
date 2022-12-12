using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2022.ClimbingGraph;

namespace AdventOfCode2022
{
    public class ClimbingGraph
    {
        public struct Node
        {
            public int Height;
            public bool visited;
            public (int, int) visitedBy;
        }

        private Dictionary<(int, int), List<(int, int)>> climbableNeighbours = new();
        public ClimbingGraph(Node[,] grid, Func<int, int, bool> neighbourCondition)
        {
            this.grid = grid;
            this.neighbourCondition = neighbourCondition;
        }
        Node[,] grid;
        Func<int, int, bool> neighbourCondition;

        public List<(int, int)> CalculateShortestPath((int, int) start, (int, int) end)
        {
            Queue<(int, int)> nodes = new();
            nodes.Enqueue(start);
            // Up, Right, Down, Left
            List<(int, int)> dirs = new() { (-1, 0), (0, 1), (1, 0), (0, -1) };
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                foreach (var dir in dirs)
                {
                    var neighbour = (node.Item1 + dir.Item1, node.Item2 + dir.Item2);
                    if (0 <= neighbour.Item1 && neighbour.Item1 < rows
                        && 0 <= neighbour.Item2 && neighbour.Item2 < columns)
                    {
                        if (grid[neighbour.Item1, neighbour.Item2].visited) continue;
                        else if (neighbourCondition(grid[node.Item1, node.Item2].Height, grid[neighbour.Item1, neighbour.Item2].Height))
                        {
                            if (neighbour == end)
                            {
                                List<(int, int)> path = new() { neighbour, node };
                                while (node != start)
                                {
                                    node = grid[node.Item1, node.Item2].visitedBy;
                                    path.Add(node);
                                }
                                return path;
                            }
                            else
                            {
                                grid[neighbour.Item1, neighbour.Item2].visited = true;
                                grid[neighbour.Item1, neighbour.Item2].visitedBy = node;
                                nodes.Enqueue(neighbour);
                            }
                        }
                    }
                }
            }
            throw new ArgumentException($"No path found between {start} and {end}");
        }
        /// <summary>
        /// Calculate the shortest path to any of the end nodes.
        /// </summary>
        public List<(int, int)> CalculateShortestPathToHeight((int, int) start, int end)
        {
            Queue<(int, int)> nodes = new();
            nodes.Enqueue(start);
            // Up, Right, Down, Left
            (int, int)[] dirs = new (int, int)[]{ (-1, 0), (0, 1), (1, 0), (0, -1) };
            int rows = grid.GetLength(0);
            int columns = grid.GetLength(1);
            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                for (int dirIx = 0; dirIx < dirs.Length; dirIx++)
                {
                    var neighbour = (node.Item1 + dirs[dirIx].Item1, node.Item2 + dirs[dirIx].Item2);
                    if (0 <= neighbour.Item1 && neighbour.Item1 < rows
                        && 0 <= neighbour.Item2 && neighbour.Item2 < columns)
                    {
                        if (grid[neighbour.Item1,neighbour.Item2].visited) continue;
                        else if (neighbourCondition(grid[node.Item1, node.Item2].Height, grid[neighbour.Item1, neighbour.Item2].Height))
                        {
                            if (grid[neighbour.Item1, neighbour.Item2].Height == end)
                            {
                                List<(int, int)> path = new() { neighbour, node };
                                while (node != start)
                                {
                                    node = grid[node.Item1, node.Item2].visitedBy;
                                    path.Add(node);
                                }
                                return path;
                            }
                            else
                            {
                                grid[neighbour.Item1, neighbour.Item2].visited = true;
                                grid[neighbour.Item1, neighbour.Item2].visitedBy = node;
                                nodes.Enqueue(neighbour);
                            }
                        }
                    }
                }
            }
            throw new ArgumentException($"No path found between {start} and {end}");
        }
    }

    public class Day12 : AoCDay
    {


        public static (Node[,],(int,int),(int,int)) ParseHeightmap(List<string> input)
        {
            Stopwatch w = new();
            w.Start();
            int rows = input.Count;
            int columns = input[0].Length;
            Node[,] grid = new Node[rows, columns];

            (int, int) start = (-1,-1);
            (int, int) end = (-1, -1);

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var chr = input[row][column];
                    switch (chr)
                    {
                        case 'S':
                            start = (row, column);
                            grid[row, column].Height = 0;
                            break;
                        case 'E':
                            end = (row, column);
                            grid[row, column].Height = 'z' - 'a';
                            break;
                        default:
                            grid[row, column].Height = input[row][column] - 'a';
                            break;
                    }
                    grid[row, column].visited = false;
                    grid[row, column].visitedBy = (-1, -1);
                }
            }
            w.Stop();
            System.Console.Out.WriteLine($"Grid creation ms {w.ElapsedMilliseconds}");
            return (grid, start, end);
        }

        public static string ExecutePart1(List<string> input)
        {
            int rows = input.Count;
            int columns = input.Count;
            var result = ParseHeightmap(input);
            var grid = result.Item1;
            (int, int) start = result.Item2;
            (int, int) end = result.Item3;
            Stopwatch w = new();
            w.Start();
            var graph = new ClimbingGraph(grid, (height, neighbourHeight) => height+1 >= neighbourHeight);
            w.Stop();
            System.Console.Out.WriteLine($" parse ms {w.ElapsedMilliseconds}");
            var path = graph.CalculateShortestPath(start, end);
            return (path.Count -1).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            int rows = input.Count;
            int columns = input[0].Length;
            var result = ParseHeightmap(input);
            var grid = result.Item1;
            (int, int) _ = result.Item2;
            (int, int) start = result.Item3;
            Stopwatch w = new();
            w.Start();
            var graph = new ClimbingGraph(grid, (height, neighbourHeight) => height-1 <= neighbourHeight);
            w.Stop();
            System.Console.Out.WriteLine($" parse ms {w.ElapsedMilliseconds}");
            var path = graph.CalculateShortestPathToHeight(start, 0);
            return (path.Count -1).ToString();
        }
    }
}
