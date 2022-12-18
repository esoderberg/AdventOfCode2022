using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    public class Day17 : AoCDay
    {

        public static long rocks_to_drop = 2022;

        // pairs Offset from rock-bottom, height of column
        //public static (int offset, int height)[][] RockPatterns = new (int, int)[5][]
        //{
        //    new (int,int)[]{(0,1),(0,1),(0,1),(0,1)}, // Line
        //    new (int,int)[]{(1,1),(0,3),(0,1), }, // Cross
        //    new (int,int)[]{(0,1),(0,1),(0,3), }, // reverse L
        //    new (int,int)[]{(0,4), }, // Line 
        //    new (int,int)[]{(0,2),(0,2), }, // Square
        //};

        public static List<string[]> RockPatterns = new List<string[]>()
        {
            new string[]{"####"},
            new string[]{".#.","###", ".#."},
            new string[]{ "..#", "..#", "###" },
            new string[]{"#","#", "#", "#" },
            new string[]{"##", "##"},
        };


        public static bool IsOverlapping(int x, int y, string[] rockPattern, List<char[]> rows)
        {
            // Look from bottom of rock pattern upwards
            for (int ry = rockPattern.Length - 1; ry >= 0; ry--)
            {
                for (int rx = 0; rx < rockPattern[ry].Length; rx++)
                {
                    if (rockPattern[ry][rx] == '#' && (rows[y - ry][x + rx] == '#' || rows[y - ry][x + rx] == '-'))
                    { // Pattern collides with some other rock
                        return true;
                    }
                }
            }
            return false;
        }

        public static string ExecutePart1(List<string> input)
        {
            int chuteWidth = 7;
            var jets = input.First();
            int highestRock = 0;
            List<char[]> rows = new List<char[]>();
            rows.Add("-------".ToCharArray()); // Need to subtract 1 from heigh later.
            int nextRock = 0;
            int nextJet = 0;
            for (int i = 0; i < rocks_to_drop; i++)
            {
                var rockPattern = RockPatterns[nextRock];
                nextRock = (nextRock + 1) % RockPatterns.Count;
                var width = rockPattern[0].Length;
                int x = 2; // Left most rock
                int y = highestRock + 3 + rockPattern.Length; // Topmost rock
                
                while(rows.Count <= y)
                    rows.Add(".......".ToCharArray());

                bool atRest = false;

                while (!atRest)
                {
                    var jet = jets[nextJet];
                    nextJet = (nextJet + 1) % jets.Length;
                    // Push by jet
                    var preMoveX = x;
                    if (jet == '<' && x > 0) x = x - 1;
                    else if(jet == '>' && x + width < chuteWidth)  x = x + 1;
                    if (x != preMoveX) // We don't need to check against rocks if horizontal movement was stopped by a wall
                    {
                        if(IsOverlapping(x, y, rockPattern, rows))
                        {
                            x = preMoveX; // Cant come to rest during horizontal stage.
                        }
                    }
                    // Fall by gravity
                    var preMoveY = y;
                    y = y-1;
                    if(IsOverlapping(x, y, rockPattern, rows))
                    {
                        y = preMoveY;
                        atRest = true;
                        highestRock = Math.Max(y, highestRock);
                    }

                }
                // Record the pattern into the rows.
                for (int ry = 0; ry < rockPattern.Length; ry++)
                {
                    for (int rx = 0; rx < rockPattern[ry].Length; rx++)
                    {
                        if(rockPattern[ry][rx] == '#')
                            rows[y - ry][x + rx] = rockPattern[ry][rx];
                    }
                }
            }
            return highestRock.ToString();
        }

        public static void LogTower(List<char[]> rows)
        {
            System.Diagnostics.Debug.WriteLine("\n--------------------------");

            for (int row = rows.Count - 1; row >= 0; row--)
            {
                System.Diagnostics.Debug.WriteLine(string.Join("", rows[row]));
            }
        }

        public static (int nextRockIndex, int nextJetIndex, int rocksDropped, int height) DropRocksUntilFullWidth(int rockIndex, int jetIndex, string jets)
        {
            List<char[]> rows = new List<char[]>();
            int chuteWidth = 7;
            int droppedRocks = 0;
            int highestRock = 0;
            rows.Add("-------".ToCharArray()); // Need to subtract 1 from heigh later.
            while (true)
            {
                var rockPattern = RockPatterns[rockIndex];
                rockIndex = (rockIndex + 1) % RockPatterns.Count;
                var width = rockPattern[0].Length;
                int x = 2; // Left most rock
                int y = highestRock + 3 + rockPattern.Length; // Topmost rock
                droppedRocks++;
                while (rows.Count <= y)
                    rows.Add(".......".ToCharArray());

                bool atRest = false;
                while (!atRest)
                {
                    var jet = jets[jetIndex];
                    jetIndex = (jetIndex + 1) % jets.Length;
                    // Push by jet
                    var preMoveX = x;
                    if (jet == '<' && x > 0) x = x - 1;
                    else if (jet == '>' && x + width < chuteWidth) x = x + 1;
                    if (x != preMoveX) // We don't need to check against rocks if horizontal movement was stopped by a wall
                    {
                        if (IsOverlapping(x, y, rockPattern, rows))
                        {
                            x = preMoveX; // Cant come to rest during horizontal stage.
                        }
                    }
                    // Fall by gravity
                    var preMoveY = y;
                    y = y - 1;
                    if (IsOverlapping(x, y, rockPattern, rows))
                    {
                        y = preMoveY;
                        atRest = true;
                        highestRock = Math.Max(y, highestRock);
                    }

                }
                // Record the pattern into the rows.
                for (int ry = 0; ry < rockPattern.Length; ry++)
                {
                    for (int rx = 0; rx < rockPattern[ry].Length; rx++)
                    {
                        if (rockPattern[ry][rx] == '#')
                            rows[y - ry][x + rx] = rockPattern[ry][rx];
                    }
                }
                LogTower(rows);
                for (int ry = 0; ry < rockPattern.Length; ry++)
                {
                    if (rows[y - ry].All(c => c == '#')) return (rockIndex, jetIndex, droppedRocks, highestRock);
                }
            }
        }

        public static string ExecutePart2(List<string> input)
        {
            rocks_to_drop = 1000000000000;
            var jets = input.First();
            long highestRock = 0;
            List<char[]> rows = new List<char[]>();

            Dictionary<(int rockIndex, int jetIndex), (int rockIndex, int jetIndex, int rocksDropped, int height)> DropCyclePatterns = new();

            rows.Add("-------".ToCharArray()); // Need to subtract 1 from heigh later.
            int nextRock = 0;
            int nextJet = 0;
            for (long i = 0; i < rocks_to_drop; i++)
            {
                if(DropCyclePatterns.TryGetValue((nextRock, nextJet), out var val))
                {
                    i += val.rocksDropped;
                    highestRock += val.height;
                    nextRock = val.rockIndex;
                    nextJet = val.jetIndex;
                }
                else
                {
                    var res = DropRocksUntilFullWidth(nextRock, nextJet, jets);
                    DropCyclePatterns.Add((nextRock, nextJet), res);
                    i += res.rocksDropped;
                    highestRock += res.height;
                    nextRock = res.nextRockIndex;
                    nextJet = res.nextJetIndex;
                }
            }
            
            return highestRock.ToString();
        }

    }
}