using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day15Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Day15.searchRow = 10;
            Assert.AreEqual("26", Day15.ExecutePart1(AoCFile.ReadTestInput(15)));
        }

        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("93", Day15.ExecutePart2(AoCFile.ReadTestInput(15)));
        }
    }
}
