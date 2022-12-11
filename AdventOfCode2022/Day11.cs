using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day11 : AoCDay
    {

        public class Monkey
        {
            private readonly List<int> items;
            private readonly Func<int, int> inspect;
            private readonly Func<int, bool> test;
            private readonly Func<bool, int> target;
            public int InspectedItems { get; private set; } = 0;
            public Monkey(List<int> startingItems, Func<int, int> operation, Func<int, bool> Test, Func<bool, int> Throw)
            {
                this.items = startingItems;
                this.inspect = operation;
                this.test = Test;
                this.target = Throw;
            }
            public void Round(List<Monkey> monkeys)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i] = inspect(items[i]);
                    InspectedItems++;
                    items[i] = items[i] / 3;
                    var testResult = test(items[i]);
                    var throwTo = target(testResult);
                    monkeys[throwTo].Receive(items[i]);
                }
                items.Clear();
            }
            public void Receive(int item) { items.Add(item); }


            #region Parsing monkey
            private static Regex numberRe = new Regex(@"\d+");
            private static Regex operationRe = new Regex(@"Operation: new = (?<p1>\w+) (?<op>.) (?<p2>\w+)");
            private static Regex testRe = new Regex(@"Test: divisible by (?<divBy>\d+)");
            private static Regex throwRe = new Regex(@"If (?<bool>\w+): throw to monkey (?<target>\d+)");
            public static Monkey ParseMonkey(List<string> monkeylines)
            {
                int monkeyNo = int.Parse(numberRe.Match(monkeylines[0]).Value);
                List<int> startingItems = numberRe.Matches(monkeylines[1]).Select(m => int.Parse(m.Value)).ToList();
                var operation = ParseOperation(monkeylines[2]);
                var divBy = int.Parse(testRe.Match(monkeylines[3]).Groups["divBy"].Value);
                Func<int, bool> test = (int worry) => worry % divBy == 0;
                var throwRes = throwRe.Match(monkeylines[4]);
                var ifTrue = int.Parse(throwRes.Groups["target"].Value);
                throwRes = throwRe.Match(monkeylines[5]);
                var ifFalse = int.Parse(throwRes.Groups["target"].Value);
                Func<bool, int> thrw = (bool testResult) => testResult ? ifTrue : ifFalse;

                return new Monkey(startingItems, operation, test, thrw);
            }
            #endregion

            private static Func<int,int> ParseOperation(string line)
            {
                var opMatch = operationRe.Match(line);
                var p1 = opMatch.Groups["p1"].Value;
                var op = opMatch.Groups["op"].Value;
                var p2 = opMatch.Groups["p2"].Value;
                var old = Expression.Parameter(typeof(int), "old");
                Expression p1Expr = p1 == "old" ? old : Expression.Constant(int.Parse(p1), typeof(int));
                Expression p2Expr = p2 == "old" ? old : Expression.Constant(int.Parse(p2), typeof(int));
                BinaryExpression opExpr = op switch
                {
                    "*" =>  Expression.Multiply(p1Expr, p2Expr),
                    "+" => Expression.Add(p1Expr, p2Expr),
                    _ => throw new ArgumentException("Unandled operator")
                };
                var l = Expression.Lambda<Func<int, int>>(opExpr, old);
                return l.Compile();
            }
        }


        

        public static string ExecutePart1(List<string> input)
        {
            var monkeys = input.Split("").Select(ml => Monkey.ParseMonkey(ml)).ToList();
            int rounds = 20;
            for (int i = 0; i < rounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.Round(monkeys);
                }
            }
            var order = monkeys.OrderByDescending(m => m.InspectedItems).ToList();
            return (order[0].InspectedItems * order[1].InspectedItems).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            throw new NotImplementedException();
        }
    }
}
