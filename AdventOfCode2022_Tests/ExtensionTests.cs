using AdventOfCode2022;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022_Tests
{

    [TestClass]
    public class ExtensionTests
    {
        [TestMethod]
        public void TestListSplit()
        {
            List<string> list = new() { "1", "2", "", "3", "4", "", "5" };
            var grouped = list.Split("");
            Assert.AreEqual(3, grouped.Count);
            Assert.AreEqual("1", grouped[0][0]);
            Assert.AreEqual("2", grouped[0][1]);
            Assert.AreEqual("3", grouped[1][0]);
            Assert.AreEqual("4", grouped[1][1]);
            Assert.AreEqual("5", grouped[2][0]);
            
        }
    }
}
