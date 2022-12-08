using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day7Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("95437", Day7.ExecutePart1(AoCFile.ReadTestInput(7)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("24933642", Day7.ExecutePart2(AoCFile.ReadTestInput(7)));
        }
    }
}
