using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day5 : AoCDay
    {
        /// <summary>
        /// Move something from 'FromColumn' to 'ToColumn' 'Count' times
        /// </summary>
        public record MoveInstruction(int FromColumn, int ToColumn, int Count=1);
        public static void ExecuteMoves(Stack<char>[] stacks, List<MoveInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Count; i++)
                {
                    stacks[instruction.ToColumn - 1].Push(stacks[instruction.FromColumn - 1].Pop());
                }
            }
        }

        public static Stack<char>[] ParseStacks(List<string> drawing)
        {
            drawing.Reverse();
            var NumberOfColumns = int.Parse(drawing.First().Trim().Last().ToString());
            Stack<char>[] stacks = new Stack<char>[NumberOfColumns];

            for (int column = 0; column < NumberOfColumns; column++)
            {
                stacks[column] = new Stack<char>();
                // First index is the column numbering so we skip that.
                for (int i = 1; i < drawing.Count; i++) // Go through entire height of column
                {
                    // 1 skips the initial bracket, column skips the space between stacks and column*3 targets the letter of the next stack
                    var letter = drawing[i][1 + column + column * 3 ];
                    if (letter != ' ')
                        stacks[column].Push(letter);
                }
            }
            return stacks;
        }

        private static Regex moveExtractor = new Regex(@"move (?<count>\d+) from (?<from>\d+) to (?<to>\d+)");
        public static List<MoveInstruction> ParseInstructions(List<string> stringInstructions)
        {
            List<MoveInstruction> instructions = new ();
            foreach (var line in stringInstructions)
            {
                var match = moveExtractor.Match(line);
                if (match.Success)
                {
                    instructions.Add(new(int.Parse(match.Groups["from"].Value), int.Parse(match.Groups["to"].Value), int.Parse(match.Groups["count"].Value)));
                }
                else throw new ArgumentException("Invalid input data");
            }
            return instructions;
        }

        public static string ExecutePart1(List<string> input)
        {
            var parts = input.Split("");
            var drawing = parts[0];
            var instructionLines = parts[1];
            var stacks = ParseStacks(drawing);
            var instructions = ParseInstructions(instructionLines);

            ExecuteMoves(stacks, instructions);

            var result = "";
            foreach (var stack in stacks)
            {
                result += stack.Peek();
            }
            return result;
        }


        public static void ExecuteMoves_CrateMover9001(Stack<char>[] stacks, List<MoveInstruction> instructions)
        {
            Stack<char> craneStack = new Stack<char>();
            foreach (var instruction in instructions)
            {
                for (int i = 0; i < instruction.Count; i++)
                {
                    craneStack.Push(stacks[instruction.FromColumn - 1].Pop());
                }
                for (int i = 0; i < instruction.Count; i++)
                {
                    stacks[instruction.ToColumn - 1].Push(craneStack.Pop());
                }
            }
        }


        public static string ExecutePart2(List<string> input)
        {
            var parts = input.Split("");
            var drawing = parts[0];
            var instructionLines = parts[1];
            var stacks = ParseStacks(drawing);
            var instructions = ParseInstructions(instructionLines);

            ExecuteMoves_CrateMover9001(stacks, instructions);

            var result = "";
            foreach (var stack in stacks)
            {
                result += stack.Peek();
            }
            return result;
        }
    }
}
