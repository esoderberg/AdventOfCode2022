using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day6Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("5", Day6.ExecutePart1(new() { "bvwbjplbgvbhsrlpgdmjqwftvncz" }));
            Assert.AreEqual("6", Day6.ExecutePart1(new() { "nppdvjthqldpwncqszvftbrmjlhg" }));
            Assert.AreEqual("10", Day6.ExecutePart1(new() { "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg" }));
            Assert.AreEqual("11", Day6.ExecutePart1(new() { "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" }));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("19", Day6.ExecutePart2(new() { "mjqjpqmgbljsphdztnvjfqwrcgsmlb" }));
            Assert.AreEqual("23", Day6.ExecutePart2(new() { "bvwbjplbgvbhsrlpgdmjqwftvncz" }));
            Assert.AreEqual("23", Day6.ExecutePart2(new() { "nppdvjthqldpwncqszvftbrmjlhg" }));
            Assert.AreEqual("29", Day6.ExecutePart2(new() { "nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg" }));
            Assert.AreEqual("26", Day6.ExecutePart2(new() { "zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw" }));
        }

        [TestMethod]
        public void TestRingBufferIndexOf()
        {
            var ringbuffer = new RingBuffer<int>(4);
            ringbuffer.Add(1);
            ringbuffer.Add(2);
            ringbuffer.Add(3);
            ringbuffer.Add(4);
            ringbuffer.Add(5);
            ringbuffer.Add(6);
            Assert.AreEqual(0, ringbuffer.IndexOf(3));
            Assert.AreEqual(1, ringbuffer.IndexOf(4));
            Assert.AreEqual(2, ringbuffer.IndexOf(5));
            Assert.AreEqual(3, ringbuffer.IndexOf(6));
        }
        [TestMethod]
        public void TestRingBufferHasDuplicate()
        {
            var ringbuffer = new RingBuffer<int>(4);
            ringbuffer.Add(1);
            ringbuffer.Add(2);
            ringbuffer.Add(3);
            ringbuffer.Add(4);
            Assert.IsFalse(ringbuffer.HasDuplicateItem());
            ringbuffer.Add(2);
            Assert.IsTrue(ringbuffer.HasDuplicateItem());
        }
    }
}
