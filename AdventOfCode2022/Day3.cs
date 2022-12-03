using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day3 : AoCDay
    {
        public static int ToPriority(char letter)
        {
            if ('a' <= letter && letter <= 'z') return letter - 'a' + 1;
            else if ('A' <= letter && letter <= 'Z') return letter - 'A' + 27;
            else return 0;
        }


        public static int CalculateRucksackSharedItemPriority(string rucksack)
        {

            HashSet<char> Compartment1 = new();
            char commonLetter = '\0';
            int compartmentSize = rucksack.Length / 2; // Length guaranteed to be even.
            for (int i = 0; i < compartmentSize; i++)
            {
                Compartment1.Add(rucksack[i]);
            }
            for (int i = compartmentSize; i < rucksack.Length; i++)
            {
                if (Compartment1.Contains(rucksack[i])) { commonLetter = rucksack[i]; break; }
            }
            return ToPriority(commonLetter);
        }

        public static string ExecutePart1(List<string> input)
        {
            int compartmentSums = 0;
            foreach(string line in input)
            {
                compartmentSums += CalculateRucksackSharedItemPriority(line);
            }
            return compartmentSums.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            throw new NotImplementedException();
        }
    }
}
