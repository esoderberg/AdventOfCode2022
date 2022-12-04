using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day4 : AoCDay
    {
        private static Regex pairExtractor = new Regex(@"(?<s1>\d+)-(?<e1>\d+),(?<s2>\d+)-(?<e2>\d+)");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s1">Start range 1</param>
        /// <param name="e1">End range 1, must be LEQ than s1</param>
        /// <param name="s2">Start range 2</param>
        /// <param name="e2">End range 2, must be LEQ than s2</param>
        /// <returns></returns>
        public static bool IsRangeContainedByOtherRange(int s1, int e1, int s2, int e2)
        {
            if (s1 <= s2 && e2 <= e1) // Range 2 is contained by range 1  s1 - s2  e2
            {
                return true;
            }
            else if (s2 <= s1 && e1 <= e2) // Range 1 is contained by range 2
            {
                return true;
            }
            else return false;
        }

        public static bool IsRangeOverlappedByOtherRange(int s1, int e1, int s2, int e2)
        {
            // The ranges are only non-overlapping if the end of range 1 is before start of range 2
            // or if the end of range 2 is before the start of range 1.
            if (e1 < s2 || e2 < s1) return false;
            else return true;
        }

        public static string ExecutePart1(List<string> input)
        {
            int containedRanged = 0;
            foreach (var line in input)
            {
                var match = pairExtractor.Match(line);
                if (match.Success)
                {
                    int s1 = int.Parse(match.Groups["s1"].Value);
                    int e1 = int.Parse(match.Groups["e1"].Value);
                    int s2 = int.Parse(match.Groups["s2"].Value);
                    int e2 = int.Parse(match.Groups["e2"].Value);
                    if(IsRangeContainedByOtherRange(s1,e1, s2, e2))
                    {
                        containedRanged++;
                    }
                }
            }
            return containedRanged.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            int overlappedRanges = 0;
            foreach (var line in input)
            {
                var match = pairExtractor.Match(line);
                if (match.Success)
                {
                    int s1 = int.Parse(match.Groups["s1"].Value);
                    int e1 = int.Parse(match.Groups["e1"].Value);
                    int s2 = int.Parse(match.Groups["s2"].Value);
                    int e2 = int.Parse(match.Groups["e2"].Value);
                    if (IsRangeOverlappedByOtherRange(s1, e1, s2, e2))
                    {
                        overlappedRanges++;
                    }
                }
            }
            return overlappedRanges.ToString();
        }
    }
}
