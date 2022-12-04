using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day4Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("2", Day4.ExecutePart1(AoCFile.ReadTestInput(4)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("4", Day4.ExecutePart2(AoCFile.ReadTestInput(4)));
        }
        [TestMethod]
        public void TestOverlappingRange()
        {
            Assert.IsTrue(Day4.IsRangeOverlappedByOtherRange(5, 7, 7, 9));
            Assert.IsTrue(Day4.IsRangeOverlappedByOtherRange(2, 8, 3, 7));
            Assert.IsTrue(Day4.IsRangeOverlappedByOtherRange(6, 6, 4, 6));
            Assert.IsTrue(Day4.IsRangeOverlappedByOtherRange(2, 6, 4, 8));
        }
    }
}
