using AdventOfCode.Challenges.IntCodeComputer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    public class IntCodeComputerTest
    {
        /// <summary>
        /// Testcases
        /// </summary>
        private static readonly object[] _sourceLists =
        {
            new object[]
            {
                new List<int> { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 },
                new List<int> { 3500,9,10,70,2,3,11,0,99,30,40,50 }
            },  //case 1
        };

        [TestCaseSource("_sourceLists")]
        public void Day2Test(List<int> code, List<int> outCode)
        {
            var intCodeComputer = new IntCodeComputer();
            var result = intCodeComputer.HandleOpCode(0, code);

            Assert.IsTrue(result.SequenceEqual(outCode));
        }

    }
}
