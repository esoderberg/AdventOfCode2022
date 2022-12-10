using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{
    [TestClass]
    public class Day10Test
    {
        [TestMethod]
        public void TestPart1()
        {
            Assert.AreEqual("13140", Day10.ExecutePart1(AoCFile.ReadTestInput(10)));
        }
        [TestMethod]
        public void TestPart2()
        {
            //Assert.AreEqual("45000", Day10.ExecutePart2(AoCFile.ReadTestInput(10)));
        }
        [TestMethod]
        public void TestSimple()
        {
            var instr = new[] { "noop", "addx 3", "addx -5" }.Select(Day10.Instruction.Parse).ToList();
            var p = new Day10.Processor();
            var expects = new List<int>() { 1, 1, 1, 4, -1 };
            
            for (int i = 0; i < 5; i++)
            {
                if (i < instr.Count) p.Load(instr[i]);
                Assert.AreEqual(expects[i], p.RegisterX);
                p.Process();

            }
        }
    }
}
