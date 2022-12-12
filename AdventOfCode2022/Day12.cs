using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class NodePath<T>
    {
        private HashSet<T> visited;
        private List<T> path;
        public NodePath(T start) { visited = new() { start }; path = new() { start }; }
        public NodePath(NodePath<T> p, T node)
        {
            visited = new(p.visited);
            path = new(p.path);
            Visit(node);
        }
        public bool HasVisited(T node) { return visited.Contains(node); }
        public int Nodes { get { return path.Count; } }
        public int Edges { get { return path.Count - 1; } }
        public void Visit(T node) { visited.Add(node); path.Add(node); }
        public override string ToString()
        {
            return string.Join(", ", path);
        }
        // Gets the last node in the path.
        public T GetLast() { return path[path.Count - 1]; }
    }

    public class ClimbingGraph
    {
        private Dictionary<(int, int), List<(int, int)>> climbableNeighbours = new();
        public ClimbingGraph(int[,] grid, Func<int, int, bool> neighbourCondition)
        {
            // Up, Right, Down, Left
            List<(int, int)> dirs = new() { (-1, 0), (0, 1), (1, 0), (0, -1) };
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int column = 0; column < grid.GetLength(1); column++)
                {
                    var currentHeight = grid[row, column];
                    var neighbours = new List<(int, int)>();
                    // Check each of the four directions
                    foreach (var dir in dirs)
                    {
                        var coordinate = (row + dir.Item1, column + dir.Item2);
                        // Coordinate is inside the grid
                        if (0 <= coordinate.Item1 && coordinate.Item1 < grid.GetLength(0)
                            && 0 <= coordinate.Item2 && coordinate.Item2 < grid.GetLength(1))
                        {
                            // Coordinate can be climbed to
                            if (neighbourCondition(currentHeight, grid[coordinate.Item1, coordinate.Item2]))
                                neighbours.Add(coordinate);
                        }
                    }
                    climbableNeighbours[(row, column)] = neighbours;
                }
            }
        }

        public NodePath<(int, int)> CalculateShortestPath((int, int) start, (int, int) end)
        {
            return CalculateShortestPath(start, new HashSet<(int,int)>() { end });
        }
        /// <summary>
        /// Calculate the shortest path to any of the end nodes.
        /// </summary>
        public NodePath<(int, int)> CalculateShortestPath((int, int) start, HashSet<(int, int)> end)
        {
            Queue<NodePath<(int, int)>> paths = new();
            paths.Enqueue(new(start));
            HashSet<(int, int)> visited = new();
            while (paths.Count > 0)
            {
                var path = paths.Dequeue();
                foreach (var neighbour in climbableNeighbours[path.GetLast()])
                {
                    if (end.Contains(neighbour)) { return new(path, neighbour); }
                    if (visited.Contains(neighbour)) continue;
                    else
                    {
                        paths.Enqueue(new(path, neighbour));
                        visited.Add(neighbour);
                    }

                }
            }
            throw new ArgumentException($"No path found between {start} and {end}");
        }
    }

    public class Day12 : AoCDay
    {


        public static (int[,],(int,int),(int,int)) ParseHeightmap(List<string> input)
        {
            int rows = input.Count;
            int columns = input[0].Length;
            int[,] grid = new int[rows, columns];

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
                            grid[row, column] = 0;
                            break;
                        case 'E':
                            end = (row, column);
                            grid[row, column] = 'z' - 'a';
                            break;
                        default:
                            grid[row, column] = input[row][column] - 'a';
                            break;
                    }
                }
            }
            return (grid, start, end);
        }

        public static string ExecutePart1(List<string> input)
        {
            int rows = input.Count;
            int columns = input.Count;
            var result = ParseHeightmap(input);
            int[,] grid = result.Item1;
            (int, int) start = result.Item2;
            (int, int) end = result.Item3;

            var graph = new ClimbingGraph(grid, (height, neighbourHeight) => height+1 >= neighbourHeight);
            var path = graph.CalculateShortestPath(start, end);
            return path.Edges.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            int rows = input.Count;
            int columns = input.Count;
            var result = ParseHeightmap(input);
            int[,] grid = result.Item1;
            (int, int) _ = result.Item2;
            (int, int) start = result.Item3;

            var graph = new ClimbingGraph(grid, (height, neighbourHeight) => height-1 <= neighbourHeight);
            var endSet = new HashSet<(int, int)>();
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    if (grid[row,column] == 0)
                    {
                        endSet.Add((row, column));
                    }
                }
            }
            var path = graph.CalculateShortestPath(start, endSet);
            return path.Edges.ToString();
        }
    }
}
