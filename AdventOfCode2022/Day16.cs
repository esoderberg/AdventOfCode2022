using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdventOfCode2022.Day16;

namespace AdventOfCode2022
{
    public class Day16 : AoCDay
    {
        public record Valve(string Id, int FlowRate, List<string> Neighbours)
        {
            public override string ToString()
            {
                return $"{Id}-{FlowRate}";
            }
        }

        private static Regex parser = new Regex(@"Valve (?<id>\w\w) has flow rate=(?<flow>\d+); tunnels? leads? to valves? (?<neighbours>.*)");
        public static Valve Parse(string line)
        {
            var match = parser.Match(line);
            string id = match.Groups["id"].Value;
            int flow = int.Parse(match.Groups["flow"].Value);
            List<string> neighbours = match.Groups["neighbours"].Value.Split(", ").ToList();
            return new Valve(id, flow, neighbours);
        }

        public static Dictionary<(string,string), int> CalculateShortestDistances(List<Valve> valves)
        {
            Dictionary<(string, string), int> distance = new();
            var valveDict = new Dictionary<string, Valve>(valves.Select(v => new KeyValuePair<string, Valve>(v.Id, v)));
            // Calculate distance between each valve.
            foreach (var valve in valves)
            {
                HashSet<string> visited = new();
                Queue<(Valve, int)> toVisitforLen = new();
                toVisitforLen.Enqueue((valve, 0));
                visited.Add(valve.Id);
                while (toVisitforLen.Count > 0)
                {
                    var tmp = toVisitforLen.Dequeue();
                    var v = tmp.Item1;
                    var length = tmp.Item2;
                    distance[(valve.Id, v.Id)] = length;
                    distance[(v.Id, valve.Id)] = length;

                    foreach (var neighbour in v.Neighbours)
                    {
                        if (!visited.Contains(neighbour))
                        {
                            visited.Add(neighbour);
                            toVisitforLen.Enqueue((valveDict[neighbour], length + 1));
                        }
                    }
                }
            }
            return distance;
        }

        public static string ExecutePart1(List<string> input)
        {
            var valves = input.Select(Parse).ToList();
            var nonZeroValves = valves.Where(v => v.FlowRate > 0).ToList();
            var valveDict = new Dictionary<string, Valve>(valves.Select(v => new KeyValuePair<string, Valve>(v.Id, v)));
            var openValves = new List<Valve>();
            var distance = CalculateShortestDistances(valves);

            List<(int, List<(string,int)>)> completed = new();
            Queue<(string, HashSet<string>, int, int, List<(string,int)>)> toVisit = new();
            // Current node      , remaining nodes                     , minutes left, released pressure, path 
            toVisit.Enqueue(("AA", new(nonZeroValves.Select(v => v.Id)),           30,                 0, new() { ("AA",30) }));
            while(toVisit.Count > 0)
            {
                var p = toVisit.Dequeue();
                if (p.Item2.Count > 0) // This is some valve we can still visit
                {
                    bool wentSomewhere = false;
                    // Go through each of the remaining valves
                    foreach (var valve in p.Item2)
                    {
                        var timeRequiredToGoAndOpen = distance[(valve, p.Item1)] + 1;
                        var timeLeftAfterWalkAndOpen = p.Item3 - timeRequiredToGoAndOpen;
                        // We must have time to walk to the node, open it and have some time afterwards to gain anything from it it.
                        if (timeLeftAfterWalkAndOpen > 0)
                        {
                            wentSomewhere = true;
                            HashSet<string> remaining = new(p.Item2);
                            remaining.Remove(valve);
                            var pressureReleased = p.Item4 + timeLeftAfterWalkAndOpen * valveDict[valve].FlowRate;
                            toVisit.Enqueue((valve, remaining, timeLeftAfterWalkAndOpen, pressureReleased, new(p.Item5) { (valve,timeLeftAfterWalkAndOpen) }));
                        }
                        else 
                        { 
                        }

                    }
                    // If there was no new valve we could visit within the remaining time then this path is completed.
                    if (!wentSomewhere)
                        completed.Add((p.Item4,p.Item5));
                }
                else completed.Add((p.Item4, p.Item5));
            }
            completed = completed.OrderByDescending(p => p.Item1).ToList();
            return completed.Max(p => p.Item1).ToString();
        }

        public static string ExecutePart2(List<string> input)
        {
            var valves = input.Select(Parse).ToList();
            var nonZeroValves = valves.Where(v => v.FlowRate > 0).ToList();
            var valveDict = new Dictionary<string, Valve>(valves.Select(v => new KeyValuePair<string, Valve>(v.Id, v)));
            var openValves = new List<Valve>();
            var distance = CalculateShortestDistances(valves);

            List<(int, List<((string,string), (int,int))>)> completed = new();
            Queue<((string me,string ele) pos, HashSet<string> remaining, (int me,int ele) time, int pressure, List<((string mpos,string epos), (int mt,int et))> path)> toVisit = new();
            // Current node      , remaining nodes                     , minutes left, released pressure, path 
            toVisit.Enqueue((("AA","AA"), new(nonZeroValves.Select(v => v.Id)), (26,26), 0, new() { (("AA","AA"), (26,26)) }));
            while (toVisit.Count > 0)
            {
                var p = toVisit.Dequeue();
                if (p.remaining.Count > 0) // This is some valve we can still visit
                {
                    bool wentSomewhere = false;
                    // Go through each of the remaining valves
                    var mypos = p.pos.me;
                    var mytime = p.time.me;
                    var elepos = p.pos.ele;
                    var eletime = p.time.ele;

                    var valvesICanVisit = p.remaining.Select<string,(string id, int time)>(n => (n, mytime - (distance[(n, mypos)] + 1))).Where(n => n.time > 0).ToList();
                    if(valvesICanVisit.Count == 0)
                        valvesICanVisit.Add((mypos,0));
                    var valvesElephantCanVisit = p.remaining.Select<string, (string id, int time)>(n => (n,eletime -(distance[(n, elepos)] + 1))).Where(n => n.time > 0).ToList();
                    if(valvesElephantCanVisit.Count == 0)
                        valvesElephantCanVisit.Add((elepos, 0));

                    foreach (var myValve in valvesICanVisit)
                    {
                        foreach (var eleValve in valvesElephantCanVisit)
                        {
                            if (myValve.id == eleValve.id) continue;
                            wentSomewhere = true;
                            HashSet<string> remaining = new(p.remaining);
                            remaining.Remove(myValve.id);
                            remaining.Remove(eleValve.id);
                            var pressureReleased = p.pressure + myValve.time * valveDict[myValve.id].FlowRate + eleValve.time * valveDict[eleValve.id].FlowRate;
                            toVisit.Enqueue(
                                ((myValve.id, eleValve.id),
                                remaining, 
                                (myValve.time, eleValve.time),
                                pressureReleased,
                                new(p.path) { ((myValve.id,eleValve.id),(myValve.time, eleValve.time)) }) );
                        }
                    }

                    // If there was no new valve we could visit within the remaining time then this path is completed.
                    if (!wentSomewhere)
                        completed.Add((p.pressure, p.path));
                }
                else completed.Add((p.pressure, p.path));
            }
            completed = completed.OrderByDescending(p => p.Item1).ToList();
            return completed.Max(p => p.Item1).ToString();
        }
    }
}
