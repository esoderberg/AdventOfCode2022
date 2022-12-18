using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day17Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("3068", Day17.ExecutePart1(AoCFile.ReadTestInput(17)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("1514285714288", Day17.ExecutePart2(AoCFile.ReadTestInput(17)));
        }
    }
}
