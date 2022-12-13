using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public  class Day13Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("13", Day13.ExecutePart1(AoCFile.ReadTestInput(13)));
        }

        [TestMethod]
        public void TestPart2()
        {
            //Assert.AreEqual("29", Day13.ExecutePart2(AoCFile.ReadTestInput(13)));
        }
    }
}
