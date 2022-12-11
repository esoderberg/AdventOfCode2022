using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode2022.Day11;

using WorryType = System.Int64;
namespace AdventOfCode2022
{
    public class Day11 : AoCDay
    {

        public class Monkey
        {
            private readonly List<WorryType> items;
            private readonly Func<WorryType, WorryType> inspect;
            
            private readonly Func<WorryType, bool> test;
            private readonly Func<bool, int> target;

            public Func<WorryType, WorryType> relief;
            public long InspectedItems { get; private set; } = 0;
            public List<WorryType> Items => items;
            public WorryType DivBy { get; }
            public Monkey(List<WorryType> startingItems, Func<WorryType, WorryType> inspect, Func<WorryType, WorryType> relief, Func<WorryType, bool> Test, Func<bool, int> Throw, WorryType divBy)
            {
                this.items = startingItems;
                this.inspect = inspect;
                this.test = Test;
                this.target = Throw;
                this.relief = relief;
                DivBy = divBy;
            }
            public void Round(List<Monkey> monkeys)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i] = inspect(items[i]);
                    InspectedItems++;
                    items[i] = relief(items[i]);
                    var testResult = test(items[i]);
                    var throwTo = target(testResult);
                    monkeys[throwTo].Receive(items[i]);
                }
                items.Clear();
            }
            public void Receive(WorryType item) { items.Add(item); }


            #region Parsing monkey
            private static Regex numberRe = new Regex(@"\d+");
            private static Regex operationRe = new Regex(@"Operation: new = (?<p1>\w+) (?<op>.) (?<p2>\w+)");
            private static Regex testRe = new Regex(@"Test: divisible by (?<divBy>\d+)");
            private static Regex throwRe = new Regex(@"If (?<bool>\w+): throw to monkey (?<target>\d+)");
            public static Monkey ParseMonkey(List<string> monkeylines, Func<WorryType, WorryType> relief)
            {
                int monkeyNo = int.Parse(numberRe.Match(monkeylines[0]).Value);
                List<WorryType> startingItems = numberRe.Matches(monkeylines[1]).Select(m => WorryType.Parse(m.Value)).ToList();
                // Operation
                var operation = ParseOperation(monkeylines[2]);
                // Test stage
                var divBy = WorryType.Parse(testRe.Match(monkeylines[3]).Groups["divBy"].Value);
                var test = (WorryType worry) => worry % divBy == 0;

                // Throw stage
                var throwRes = throwRe.Match(monkeylines[4]);
                var ifTrue = int.Parse(throwRes.Groups["target"].Value);
                throwRes = throwRe.Match(monkeylines[5]);
                var ifFalse = int.Parse(throwRes.Groups["target"].Value);
                Func<bool, int> thrw = (bool testResult) => testResult ? ifTrue : ifFalse;

                return new Monkey(startingItems, operation, relief, test, thrw, divBy);
            }
            #endregion

            private static Func<WorryType, WorryType> ParseOperation(string line)
            {
                var opMatch = operationRe.Match(line);
                var p1 = opMatch.Groups["p1"].Value;
                var op = opMatch.Groups["op"].Value;
                var p2 = opMatch.Groups["p2"].Value;
                var old = Expression.Parameter(typeof(WorryType), "old");
                Expression p1Expr = p1 == "old" ? old : Expression.Constant(WorryType.Parse(p1), typeof(WorryType));
                Expression p2Expr = p2 == "old" ? old : Expression.Constant(WorryType.Parse(p2), typeof(WorryType));
                BinaryExpression opExpr = op switch
                {
                    "*" =>  Expression.Multiply(p1Expr, p2Expr),
                    "+" => Expression.Add(p1Expr, p2Expr),
                    _ => throw new ArgumentException("Unandled operator")
                };
                var l = Expression.Lambda<Func<WorryType, WorryType>>(opExpr, old);
                return l.Compile();
            }
        }


        public static void PrintInspections(List<Monkey> monkeys)
        {
            var i = 0;
            foreach (var monkey in monkeys)
            {
                System.Diagnostics.Debug.WriteLine($"Monkey {i} inspected items {monkey.InspectedItems} times");
                i++;
            }
        }
        public static void PrintItems(List<Monkey> monkeys)
        {
            var i = 0;
            foreach (var monkey in monkeys)
            {
                System.Diagnostics.Debug.WriteLine($"Monkey {i}: {string.Join(", ", monkey.Items)}");
                i++;
            }
        }

        public static string ExecuteRounds(List<Monkey> monkeys, int rounds)
        {
            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.Round(monkeys);
                }
                //if (i <= 19 || i == 999 || i == 1999)
                //{
                //    System.Diagnostics.Debug.WriteLine($"=== After round {i+1} ===");
                //    PrintItems(monkeys);
                //    PrintInspections(monkeys);
                //}
            }
            var order = monkeys.OrderByDescending(m => m.InspectedItems).ToList();
            return (order[0].InspectedItems * order[1].InspectedItems).ToString();
        }

        public static string ExecutePart1(List<string> input)
        {
            var monkeys = input.Split("").Select(ml => Monkey.ParseMonkey(ml, i => i / 3)).ToList();
            return ExecuteRounds(monkeys, 20);
        }

        public static string ExecutePart2(List<string> input)
        {
            var monkeys = input.Split("").Select(ml => Monkey.ParseMonkey(ml, i => i)).ToList();
            var commonMod = monkeys.Select(m => m.DivBy).Aggregate((WorryType)1, (prev, curr) => prev * curr);
            foreach (var monkey in monkeys)
            {
                monkey.relief = (old) => old % commonMod;
            }
            return ExecuteRounds(monkeys, 10000);
        }
    }
}
