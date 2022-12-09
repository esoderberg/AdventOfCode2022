using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day9 : AoCDay
    {
        
        public record Position(int X, int Y)
        {
            public override string ToString()
            {
                return $"({X},{Y})";
            }
        }
        public record Distance(int Dx, int Dy)
        {
            public int ManhattanLength => Math.Abs(Dx) + Math.Abs(Dy);
            public double EuclidLengthSquared => Dx*Dx + Dy*Dy;
        };
        public enum Direction
        {
            Left, Right, Up, Down
        }
        public class Mover
        {
            public Position Pos = new(0,0);
            public readonly List<Position> visitOrder = new();
            public readonly HashSet<Position> visited = new();
            public Mover()
            {
                visitOrder.Add(Pos);
                visited.Add(Pos);
            }
            public void Move(Direction dir, int times)
            {
                for (int i = 0; i < times; i++)
                {
                    Move(dir);
                }
            }
            public void Move(Direction dir)
            {
                switch (dir)
                {
                    case Direction.Left:
                        Move(-1, 0);
                        break;
                    case Direction.Right:
                        Move(1, 0);
                        break;
                    case Direction.Up:
                        Move(0, 1);
                        break;
                    case Direction.Down:
                        Move(0, -1);
                        break;
                    default:
                        break;
                }
            }
            public void Move(int dx, int dy)
            {
                if (dx != 0 || dy != 0)
                {
                    Pos = Pos with { X = Pos.X + dx, Y = Pos.Y + dy };
                    OnMoved();
                    visitOrder.Add(Pos);
                    visited.Add(Pos);
                }
            }

            public virtual void OnMoved() {}

            public Distance DistanceTo(Position target)
            {
                return new Distance(target.X - Pos.X, target.Y - Pos.Y);
            }

        }
        public class Head : Mover
        {
            public Mover tail;
            public Head(int knots)
            {
                if (knots > 1)
                    tail = new Head(knots - 1);
                else
                    tail = new Mover();
            }
            public Mover GetLast()
            {
                if (tail is Head h) return h.GetLast();
                else return tail;
            }
            public override void OnMoved()
            {
                var tailDist = tail.DistanceTo(Pos);
                var eucLen = tailDist.EuclidLengthSquared;
                if (eucLen > 2)
                {
                    tail.Move(Math.Clamp(tailDist.Dx, -1, 1), Math.Clamp(tailDist.Dy,-1,1));
                }
            }
        }

        public record MoveInstruction(Direction dir, int count);

        public static Direction Parse(string str)
        {
            if (str == "R") return Direction.Right;
            else if (str == "U") return Direction.Up;
            else if (str == "L") return Direction.Left;
            else if (str == "D") return Direction.Down;
            else throw new ArgumentException();
        }
        public static List<MoveInstruction> ParseInstructions(List<string> input)
        {
            List<MoveInstruction> instructions = new();
            foreach (var line in input)
            {
                var parts = line.Split(" ");
                instructions.Add(new MoveInstruction(Parse(parts[0]), int.Parse(parts[1])));
            }
            return instructions;
        }

        public static string ExecutePart1(List<string> input)
        {
            var instructions = ParseInstructions(input);
            var head = new Head(1);
            foreach (var instruction in instructions)
            {
                head.Move(instruction.dir, instruction.count);
            }
            return head.tail.visited.Count.ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            var instructions = ParseInstructions(input);
            var head = new Head(9);
            foreach (var instruction in instructions)
            {
                head.Move(instruction.dir, instruction.count);
            }
            System.Diagnostics.Debug.WriteLine(string.Join(", ", head.GetLast().visitOrder));
            return head.GetLast().visited.Count.ToString();
        }
    }
}
