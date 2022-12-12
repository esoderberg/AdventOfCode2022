using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day12Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("31", Day12.ExecutePart1(AoCFile.ReadTestInput(12)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("29", Day12.ExecutePart2(AoCFile.ReadTestInput(12)));
        }
    }
}
