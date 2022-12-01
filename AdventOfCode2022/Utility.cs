using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public static class Utility
    {


        /// <summary>
        /// Split the list into groups by the given separator.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<List<string>> Split(this List<string> list, string separator)
        {
            List<List<string>> groups = new List<List<string>>();
            List<string> currentGroup = new List<string>();
            foreach (var item in list)
            {
                if (item != separator)
                {
                    currentGroup.Add(item);
                }
                else
                {
                    groups.Add(currentGroup);
                    currentGroup = new List<string>();
                }
            }
            groups.Add(currentGroup);
            return groups;
        }
    }
}
