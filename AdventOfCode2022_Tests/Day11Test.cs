using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day11Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("10605", Day11.ExecutePart1(AoCFile.ReadTestInput(11)));
        }
        [TestMethod]
        public void TestPart2()
        {
           
        }
    }
}
