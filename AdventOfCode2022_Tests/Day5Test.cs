using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day5Test
    {

        [TestMethod]
        public void TestStackParsing()
        {
            var stacks = Day5.ParseStacks(new() {
                "    [D]",
                "[A] [B]",
                " 1   2 "
            });
            Assert.AreEqual(2, stacks.Length);
            Assert.AreEqual('A', stacks[0].Pop());
            Assert.AreEqual('D', stacks[1].Pop());
            Assert.AreEqual('B', stacks[1].Pop());
        }

        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("CMZ", Day5.ExecutePart1(AoCFile.ReadTestInput(5)));
        }
    }
}
