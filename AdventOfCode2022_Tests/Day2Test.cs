using AdventOfCode2022;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day2Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("15", Day2.ExecutePart1(AoCFile.ReadTestInput(2)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("12", Day2.ExecutePart2(AoCFile.ReadTestInput(2)));
        }

        [TestMethod]
        public void TestLosingMoves()
        {
            Assert.AreEqual(Day2.GetLosingMoveTo(Day2.Move.Rock), Day2.Move.Scissors);
            Assert.AreEqual(Day2.GetLosingMoveTo(Day2.Move.Paper), Day2.Move.Rock);
            Assert.AreEqual(Day2.GetLosingMoveTo(Day2.Move.Scissors), Day2.Move.Paper);
        }
    }
}
