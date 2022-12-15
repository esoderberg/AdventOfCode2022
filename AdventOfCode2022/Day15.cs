using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day15 : AoCDay
    {
        public static Regex parser = new Regex(@"Sensor at x=(?<x>-?\d+), y=(?<y>-?\d+): closest beacon is at x=(?<bx>-?\d+), y=(?<by>-?\d+)");

        public static (Point, Point) Parse(string line)
        {
            var m = parser.Match(line);
            var sensor = new Point(int.Parse(m.Groups["x"].Value), int.Parse(m.Groups["y"].Value));
            var closestBeacon = new Point(int.Parse(m.Groups["bx"].Value), int.Parse(m.Groups["by"].Value));
            return (sensor, closestBeacon);
        }
        public static int searchRow = 2000000;
        public static string ExecutePart1(List<string> input)
        {
            var sensor_beacons = input.Select(Parse).ToList();

            List<(Point, int)> sensors_with_length = sensor_beacons.Select(p => (p.Item1, p.Item1.ManhattanLengthTo(p.Item2))).ToList();

            int leftMostX = sensors_with_length[0].Item1.x - sensors_with_length[0].Item2;
            int rightMostX = sensors_with_length[0].Item1.x + sensors_with_length[0].Item2;
            foreach (var pair in sensors_with_length)
            {
                var leftx = pair.Item1.x - pair.Item2;
                var rightx = pair.Item1.x + pair.Item2;
                if (leftx < leftMostX) leftMostX = leftx;
                if (rightx > rightMostX) rightMostX = rightx;
            }
            
            int impossiblePoints = -sensor_beacons.Select(p => p.Item2).Distinct().Count(b => b.y == searchRow);
            for (int x = leftMostX; x < rightMostX; x++)
            {
                foreach(var (sensor, len) in sensors_with_length)
                {
                    var distToPoint = sensor.ManhattanLengthTo(x, searchRow);
                    if (distToPoint <= len)
                    {
                        impossiblePoints++;
                        break;
                    }
                }
            }
            
            return impossiblePoints.ToString();
        }

        public static long coordinate_limit = 4000000;
        public static string ExecutePart2(List<string> input)
        {
            var sensor_beacons = input.Select(Parse).ToList();
            List<(Point, int)> sensors_with_length = sensor_beacons.Select(p => (p.Item1, p.Item1.ManhattanLengthTo(p.Item2))).ToList();
            sensors_with_length =  sensors_with_length.OrderBy(p => p.Item1.x).ThenBy(p => p.Item1.y).ToList();
            for (int y = 0; y < coordinate_limit; y++)
            {
                for (int x = 0; x < coordinate_limit; x++)
                {
                    bool isNotNearSensor = true;
                    foreach (var pair in sensors_with_length)
                    {
                        var sensor = pair.Item1;
                        var len = pair.Item2;
                        if (sensor.ManhattanLengthTo(x, y) <= len)
                        {
                            isNotNearSensor = false;
                            // Jump across the area covered by this sensor on this row.
                            var heightDiffFromCenter = Math.Abs(y - sensor.y);
                            var widthDiffFromCenter = Math.Abs(x - sensor.x);
                            x = sensor.x + (len - heightDiffFromCenter);
                            break;
                        }
                    }
                    if (isNotNearSensor)
                        return ((x * 4000000L) + (long)y).ToString();

                }
            }
            return "";
        }
    }
}
