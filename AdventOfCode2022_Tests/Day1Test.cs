using AdventOfCode2022;

namespace AdventOfCode2022_Tests
{

    [TestClass]
    public class Day1Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("24000", Day1.ExecutePart1(AoCFile.ReadTestInput(1)));
        }
        [TestMethod]
        public void TestPart2()
        {
            Assert.AreEqual("45000", Day1.ExecutePart2(AoCFile.ReadTestInput(1)));
        }
    }
}