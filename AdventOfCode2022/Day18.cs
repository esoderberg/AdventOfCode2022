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
            throw new NotImplementedException();
        }
    }
}
