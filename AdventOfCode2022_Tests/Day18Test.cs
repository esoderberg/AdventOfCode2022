using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day18Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("64", Day18.ExecutePart1(AoCFile.ReadTestInput(18)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("58", Day18.ExecutePart2(AoCFile.ReadTestInput(18)));
        }
    }
}
