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
                    try
                    {
                        var mth = cls?.GetMethod(nameof(AoCDay.ExecutePart1));
                        var result = mth?.Invoke(null, new[] { lines });
                        if (result?.GetType() == typeof(string))
                        {
                            Console.Out.WriteLine($"Result of day {dayNum} part 1: {result}");
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
                        var result = mth?.Invoke(null, new[] { lines });
                        if (result?.GetType() == typeof(string))
                        {
                            Console.Out.WriteLine($"Result of day {dayNum} part 2: {result}");
                        }
                    }
                    catch (NotImplementedException _)
                    {
                        Console.WriteLine($"Day {dayNum} Part 2 not solved yet");
                    }
                    catch(Exception e)
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