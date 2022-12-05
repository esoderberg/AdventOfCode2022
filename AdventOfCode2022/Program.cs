using System.Diagnostics;
using System.Reflection;

namespace AdventOfCode2022
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 1)
            {
                if (int.TryParse(args[0], out int dayNum))
                {
                    // Part 1
                    var lines = AoCFile.ReadInput(dayNum);
                    var cls = Type.GetType($"AdventOfCode2022.Day{dayNum}");
                    var stopwatch = new Stopwatch();
                    try
                    {
                        var mth = cls?.GetMethod(nameof(AoCDay.ExecutePart1));
                        stopwatch.Start();
                        var result = mth?.Invoke(null, new[] { lines });
                        stopwatch.Stop();
                        if (result?.GetType() == typeof(string))
                        {
                            Console.Out.WriteLine($"Result of day {dayNum} part 1: {result}     ms: {stopwatch.ElapsedMilliseconds}");
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Day {dayNum} error encountered during processing: {e}");
                    }
                    // Part 2
                    try
                    {
                        var mth = cls?.GetMethod(nameof(AoCDay.ExecutePart2));
                        stopwatch.Start();
                        var result = mth?.Invoke(null, new[] { lines });
                        stopwatch.Stop();
                        if (result?.GetType() == typeof(string))
                        {
                            Console.Out.WriteLine($"Result of day {dayNum} part 2: {result}     ms: {stopwatch.ElapsedMilliseconds}");
                        }
                    }
                    catch (NotImplementedException)
                    {
                        Console.WriteLine($"Day {dayNum} Part 2 not solved yet");
                    }
                    catch (TargetInvocationException)
                    {
                        Console.WriteLine($"Day {dayNum} Part 2 not solved yet");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Day {dayNum} error encountered during processing: {e}");
                    }
                }
                else
                {
                    Console.WriteLine("Day number needs to be first argument");
                }
            }
            else
            {
                Console.WriteLine("Day argument missing");
            }
        }
    }
}