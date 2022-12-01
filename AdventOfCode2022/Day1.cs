using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day1 : AoCDay
    {

        public static List<int> CalculateTotalCaloriesForElves(List<List<string>> elves)
        {
            List<int> totalCalories = new();
            foreach (var elfCalories in elves)
            {
                totalCalories.Add(elfCalories.Select(s => int.Parse(s)).Sum());
            }
            return totalCalories;
        }


        public static string ExecutePart2(List<string> input)
        {
            var elves = input.ToList().Split("");
            var totalCalories = CalculateTotalCaloriesForElves(elves);
            return totalCalories.OrderByDescending(c => c).Take(3).Sum().ToString();
        }

        public static string ExecutePart1(List<string> input)
        {
            var elves = input.ToList().Split("");
            var totalCalories = CalculateTotalCaloriesForElves(elves);
            return totalCalories.Max().ToString();
        }        
    }
}
