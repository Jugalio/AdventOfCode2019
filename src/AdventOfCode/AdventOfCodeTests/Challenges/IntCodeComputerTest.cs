using AdventOfCode.Challenges.IntCodeComputer;
using AdventOfCodeTests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class IntCodeComputerTest
    {
        private static readonly object[] _sourceListsDay2 =
        {
            new object[]
            {
                new List<int> { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 },
                new List<int> { 3500,9,10,70,2,3,11,0,99,30,40,50 }
            },
        };

        private static readonly object[] _sourceListsDay5 =
        {
            new object[]
            {
                new List<int> { 1002,4,3,4,33 },
                new List<int> { 1002, 4, 3, 4, 99 }
            },
        };

        private static readonly object[] _sourceListsDay5Part2 =
{
            new object[]
            {
                new List<int> { 3,9,8,9,10,9,4,9,99,-1,8 }, 8, 1,
            },
            new object[]
            {
                new List<int> { 3,9,8,9,10,9,4,9,99,-1,8 }, 7, 0
            },

            new object[]
            {
                new List<int> { 3,9,7,9,10,9,4,9,99,-1,8 }, 8, 0,
            },
            new object[]
            {
                new List<int> { 3,9,7,9,10,9,4,9,99,-1,8 }, 7, 1
            },

            new object[]
            {
                new List<int> { 3,3,1108,-1,8,3,4,3,99 }, 8, 1,
            },
            new object[]
            {
                new List<int> { 3,3,1108,-1,8,3,4,3,99 }, 7, 0
            },

            new object[]
            {
                new List<int> { 3,3,1107,-1,8,3,4,3,99 }, 8, 0,
            },
            new object[]
            {
                new List<int> { 3,3,1107,-1,8,3,4,3,99 }, 7, 1
            },

            new object[]
            {
                new List<int> { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, 0, 0
            },

            new object[]
            {
                new List<int> { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 }, 1, 1
            },

            new object[]
            {
                new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 1, 999
            },
            new object[]
            {
                new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 8, 1000
            },
            new object[]
            {
                new List<int> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 9, 1001
            },
        };

        [TestCaseSource("_sourceListsDay2")]
        public void Day2Test(List<int> code, List<int> outCode)
        {
            var intCodeComputer = new IntCodeComputer(code, (string s) => Console.WriteLine(s), new InputReaderMock());
            var result = intCodeComputer.Compute();

            Assert.IsTrue(result.SequenceEqual(outCode));
        }

        [TestCaseSource("_sourceListsDay5")]
        public void Day5Test(List<int> code, List<int> outCode)
        {
            var mock = new InputReaderMock();
            mock.returnValue = "1";

            var intCodeComputer = new IntCodeComputer(code, (string s) => Console.WriteLine(s), mock);
            var result = intCodeComputer.Compute();

            Assert.IsTrue(result.SequenceEqual(outCode));
        }

        [TestCaseSource("_sourceListsDay5Part2")]
        public void Day5Part2Test(List<int> code, int input, int expectedOutput)
        {
            var mock = new InputReaderMock();
            mock.returnValue = input.ToString();
            string output = string.Empty;

            var intCodeComputer = new IntCodeComputer(code, (string s) => output = s, mock);
            intCodeComputer.Compute();

            Assert.IsTrue(int.Parse(output) == expectedOutput);
        }

    }
}
