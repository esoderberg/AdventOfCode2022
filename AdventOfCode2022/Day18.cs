using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day18 : AoCDay
    {

        public static List<Point3D> Parse(List<string> input)
        {
            var points = input.Select(s => s.Split(",").Select(int.Parse).Thruple()).Select(s => new Point3D(s));
            return points.ToList();
        }

        public static string ExecutePart1(List<string> input)
        {
            var pointCubes = Parse(input);
            HashSet<Point3D> cubes = new(pointCubes);
            int coveredSides = 0;
            foreach (var cube in pointCubes)
            {
                foreach (var neighbour in cube.Neighbours())
                {
                    if (cubes.Contains(neighbour))
                    {
                        coveredSides++;
                    }
                }
            }
            return (pointCubes.Count * 6 - coveredSides).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            var lavaCubes = Parse(input);
            int minx = int.MaxValue,maxx = int.MinValue, miny = int.MaxValue, maxy = int.MinValue, minz = int.MaxValue, maxz = int.MinValue;

            foreach (var cube in lavaCubes)
            {
                minx = Math.Min(cube.x, minx);
                maxx = Math.Max(cube.x, maxx);
                miny = Math.Min(cube.y, miny);
                maxy = Math.Max(cube.y, maxy);
                minz = Math.Min(cube.z, minz);
                maxz = Math.Max(cube.z, maxz);
            }
            // Increase that maxes and decrease the mins so we get the boundaries where there
            // are no lava cubes, if we reach one of these coordinates then the air is not trapped.
            minx--; miny--; minz--; maxx++; maxy++; maxz++;


            var freeAir = new HashSet<Point3D>();
            var trappedAir = new HashSet<Point3D>();
            int coveredSides = 0;
            foreach (var cube in lavaCubes)
            {
                foreach (var neighbour in cube.Neighbours())
                {
                    if (!lavaCubes.Contains(neighbour))
                    {
                        // We know neighbour is an aircube
                        // Check if we've already checked if its trapped or free air.
                        if (trappedAir.Contains(neighbour))
                        {
                            coveredSides++;
                        }
                        else if (freeAir.Contains(neighbour)) { }
                        else // we haven't checked this cube
                        {
                            // BFS search until we get to the surface
                            bool reachedSurface = false;
                            HashSet<Point3D> visited = new HashSet<Point3D>();
                            Queue<Point3D> toVisit = new(); toVisit.Enqueue(neighbour); visited.Add(neighbour);
                            while(toVisit.Count > 0)
                            {
                                var node = toVisit.Dequeue();
                                // We have reached exposed air, add all visited air cubes to the set of freeAir
                                if (node.x == minx || node.x == maxx || node.y == miny || node.y == maxy || node.z == maxz || node.z == minz)
                                {
                                    reachedSurface = true;
                                    foreach (var airCube in visited)
                                    {
                                        freeAir.Add(airCube);
                                    }
                                    toVisit.Clear(); // Don't need to continue the BFS.
                                }
                                else
                                {
                                    // Move on to only the air-neighbours
                                    foreach (var n in node.Neighbours())
                                    {
                                        if (freeAir.Contains(n)) // We found a free air cube we've calcluated earlier.
                                        {
                                            reachedSurface = true;
                                            freeAir.Add(n);
                                            foreach (var airCube in visited)
                                            {
                                                freeAir.Add(airCube);
                                            }
                                            toVisit.Clear();
                                            break;
                                        }
                                        else if (!visited.Contains(n) && !lavaCubes.Contains(n))
                                        {
                                            toVisit.Enqueue(n);
                                            visited.Add(n);
                                        }
                                    }
                                }
                            }
                            if (!reachedSurface) coveredSides++;
                        }
                    }
                    else
                    {
                        coveredSides++;
                    }
                }
            }

            return (lavaCubes.Count * 6 - coveredSides).ToString();
        }
    }
}
