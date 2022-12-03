using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day3Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("157", Day3.ExecutePart1(AoCFile.ReadTestInput(3)));
        }
    }
}
