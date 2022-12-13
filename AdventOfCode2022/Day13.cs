using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{

    public class Day13 : AoCDay
    {

        public abstract record class PacketData
        {

            public static int Compare(PacketData @this, PacketData other)
            {
                return @this.Compare(other);
            }

            public int Compare(PacketData other)
            {
                switch (other)
                {
                    case PacketInt i: return Compare(i);
                    case PacketList l: return Compare(l);
                    default:
                        throw new ArgumentException("Invalid type");
                }
            }
            public abstract int Compare(PacketInt other);
            public abstract int Compare(PacketList other);
        }
        public record class PacketList(List<PacketData> Items) : PacketData
        {

            public override int Compare(PacketInt other)
            {
                return Compare(new PacketList(new List<PacketData>() { other }));
            }

            public override int Compare(PacketList other)
            {
                for (int i = 0; i < Items.Count && i < other.Items.Count; i++)
                {
                    switch (Items[i].Compare(other.Items[i])) {
                        case -1:  return -1;
                        case 1: return 1;
                        default:break;
                    }
                }
                if (Items.Count < other.Items.Count) return -1;
                else if (Items.Count > other.Items.Count) return 1;
                else return 0;
            }
            public override string ToString()
            {
                return $"[{string.Join(", ", Items)}]";
            }
        }
        public record class PacketInt(int Value) : PacketData
        {
            /// <summary>
            /// Returns 1 if other is larger, 0 if equal and -1 if other smaller
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public override int Compare(PacketInt other)
            {
                return Value < other.Value ? -1 : (Value == other.Value ? 0 : 1);
            }

            public override int Compare(PacketList other)
            {
                return new PacketList(new List<PacketData>() { this }).Compare(other);
            }
            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public static PacketData ParsePacket(string packetStr)
        {
            var packet = ParsePacket(packetStr, 1, out int _); ;
            return packet;
        }


        private static PacketData ParsePacket(string packetStr, int index, out int continueFromIndex)
        {
            StringBuilder builder = new();
            List<PacketData> list = new();
            continueFromIndex = 0;
            for (int i = index; i < packetStr.Length; i++)
            {
                char c = packetStr[i];
                switch (c)
                {
                    case '[':
                        list.Add(ParsePacket(packetStr, i+1, out continueFromIndex));
                        i = continueFromIndex;
                        break;
                    case ',':
                        if (builder.Length != 0)
                            list.Add(new PacketInt(int.Parse(builder.ToString())));
                        builder.Clear();
                        break;
                    case ']':
                        if (builder.Length != 0)
                            list.Add(new PacketInt(int.Parse(builder.ToString())));
                        continueFromIndex = i;
                        return new PacketList(list);
                    default:
                        builder.Append(c);
                        break;
                }
            }
            throw new ArgumentException("Invalid data");
        }

        public static string ExecutePart1(List<string> input)
        {
            var packetPairs = input.Split("").Select(pair => pair.Select(ParsePacket).ToList()).ToList();
            int index = 1;
            List<int> correctOrder = new();
            foreach (var pair in packetPairs)
            {
                var p1 = pair[0];
                var p2 = pair[1];
                if (p1.Compare(p2) == -1)
                {
                    correctOrder.Add(index);
                }
                index++;
            }
            return correctOrder.Sum().ToString();
        }

        public static string ExecutePart2(List<string> input)
        {

            var packets = input.Split("").Select(pair => pair.Select(ParsePacket).ToList()).SelectMany(s => s).ToList();
            // Yes this is beautiful, okay?
            var d1 = new PacketList(new List<PacketData>() { new PacketList(new List<PacketData>() { new PacketInt(2) }) });
            var d2 = new PacketList(new List<PacketData>() { new PacketList(new List<PacketData>() { new PacketInt(6) }) });
            packets.Add(d1);
            packets.Add(d2);
            List<int> dividers = new();
            packets.Sort(PacketData.Compare);
            dividers.Add(packets.FindIndex(p => p.Compare(d1) == 0)+1);
            dividers.Add(packets.FindIndex(p => p.Compare(d2) == 0)+1);
            return dividers.Aggregate(1, (acc, n) => acc*n).ToString();
        }
    }
}
