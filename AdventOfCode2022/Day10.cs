using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventOfCode2022.Day10;

namespace AdventOfCode2022
{
    public class Day10 : AoCDay
    {

        public record Instruction(int TimeToExec)
        {
            public static Instruction Parse(string s)
            {
                if (s == "noop") return new Noop();
                else
                {
                    var parts = s.Split(" ");
                    if (parts[0] == "addx") return new Addx(int.Parse(parts[1]));
                }
                throw new ArgumentException("Invalid instruction");
            }
        };
        
        public record Addx(int V) : Instruction(1);
        public record Noop() : Instruction(0);

        public class Processor
        {
            private int registerX = 1;
            public int Cycle { get; private set; } = 1;
            public int RegisterX => registerX;
            private List<Instruction> activeInstructions = new();
            private Queue<Instruction> waitingInstructions = new();
            public void Process()
            {
                if(activeInstructions.Count == 0 && waitingInstructions.Count != 0)
                {
                    activeInstructions.Add(waitingInstructions.Dequeue());
                }
                List<Instruction> newActiveInstructions = new();
                foreach (var instruction in activeInstructions)
                {
                    if (instruction.TimeToExec == 0)
                    {
                        switch (instruction)
                        {
                            case Addx addx: registerX += addx.V; break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        newActiveInstructions.Add(instruction with { TimeToExec = instruction.TimeToExec - 1 });
                    }
                }
                activeInstructions = newActiveInstructions;
                Cycle++;
            }
            public void Load(Instruction instruction)
            {
                waitingInstructions.Enqueue(instruction);
            }
        }

        public static string ExecutePart1(List<string> input)
        {
            var instructions = input.Select(Instruction.Parse).ToList();
            Processor processor = new Processor();
            List<(int,int)> CycleRegXValues = new();
            for (int i = 0; i < 220; i++)
            {
                if (i < instructions.Count)
                    processor.Load(instructions[i]);

                if (processor.Cycle == 20 || processor.Cycle-CycleRegXValues.Count*40 == 20)
                    CycleRegXValues.Add((processor.Cycle, processor.RegisterX));
                processor.Process();
                
            }
            return CycleRegXValues.Select((t) => t.Item1*t.Item2).Sum().ToString();   
        }

        public static string ExecutePart2(List<string> input)
        {
            throw new NotImplementedException();
        }


    }
}
