using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public interface AoCDay
    {
        /// <summary>
        /// Execute the first part of the day with the given input.
        /// </summary>
        static abstract string ExecutePart1(List<string> input);
        /// <summary>
        /// Execute the second part of the day with the given input.
        /// </summary>
        static abstract string ExecutePart2(List<string> input);

    }
}
