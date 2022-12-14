using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day14Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("24", Day14.ExecutePart1(AoCFile.ReadTestInput(14)));
        }

        [TestMethod]
        public void TestPart2()
        {
           Assert.AreEqual("93", Day14.ExecutePart2(AoCFile.ReadTestInput(14)));
        }
    }
}
