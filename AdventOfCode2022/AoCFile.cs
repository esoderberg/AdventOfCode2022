using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    /// <summary>
    /// Helper class for reading test and real input.
    /// </summary>
    public static class AoCFile
    {
        public static List<string> ReadTestInput(int day)
        {
            return ReadInputLines(day, true).ToList();
        }
        public static List<string> ReadInput(int day)
        {
            return ReadInputLines(day, false).ToList();
        }

        private static IEnumerable<string> ReadInputLines(int day, bool test)
        {
            using (var file = new StreamReader(File.OpenRead($"Input/day{day}{(test ? "_test" : "")}.txt")))
            {
                string? line = null;
                while ((line = file.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
