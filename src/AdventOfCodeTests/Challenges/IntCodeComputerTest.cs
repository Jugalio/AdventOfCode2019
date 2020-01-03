using AdventOfCode.Challenges.IntCodeComputer;
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
                new List<long> { 1, 9, 10, 3, 2, 3, 11, 0, 99, 30, 40, 50 },
                new List<long> { 3500,9,10,70,2,3,11,0,99,30,40,50 }
            },
        };

        private static readonly object[] _sourceListsDay5 =
        {
            new object[]
            {
                new List<long> { 1002,4,3,4,33 },
                new List<long> { 1002, 4, 3, 4, 99 }
            },
        };

        private static readonly object[] _sourceListsDay5Part2 =
{
            new object[]
            {
                new List<long> { 3,9,8,9,10,9,4,9,99,-1,8 }, 8, 1,
            },
            new object[]
            {
                new List<long> { 3,9,8,9,10,9,4,9,99,-1,8 }, 7, 0
            },

            new object[]
            {
                new List<long> { 3,9,7,9,10,9,4,9,99,-1,8 }, 8, 0,
            },
            new object[]
            {
                new List<long> { 3,9,7,9,10,9,4,9,99,-1,8 }, 7, 1
            },

            new object[]
            {
                new List<long> { 3,3,1108,-1,8,3,4,3,99 }, 8, 1,
            },
            new object[]
            {
                new List<long> { 3,3,1108,-1,8,3,4,3,99 }, 7, 0
            },

            new object[]
            {
                new List<long> { 3,3,1107,-1,8,3,4,3,99 }, 8, 0,
            },
            new object[]
            {
                new List<long> { 3,3,1107,-1,8,3,4,3,99 }, 7, 1
            },

            new object[]
            {
                new List<long> { 3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9 }, 0, 0
            },

            new object[]
            {
                new List<long> { 3,3,1105,-1,9,1101,0,0,12,4,12,99,1 }, 1, 1
            },

            new object[]
            {
                new List<long> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 1, 999
            },
            new object[]
            {
                new List<long> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 8, 1000
            },
            new object[]
            {
                new List<long> { 3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,
1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,
999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99 }, 9, 1001
            },
        };

        private static readonly object[] _sourceListsDay7 =
        {
            new object[]
            {
                new List<long> { 3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0 },
                new List<int> { 4, 3, 2, 1, 0 },
                43210
            },

            new object[]
            {
                new List<long> { 3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0 },
                new List<int> { 0,1,2,3,4 },
                54321
            },

            new object[]
            {
                new List<long> {3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0 },
                new List<int> { 1,0,4,3,2 },
                65210
            },
        };

        private static readonly object[] _sourceListsDay7Part2 =
{
            new object[]
            {
                new List<long> { 3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5 },
                new List<int> { 9,8,7,6,5 },
                139629729
            },

            new object[]
            {
                new List<long> { 3,52,1001,52,-5,52,3,53,1,52,56,54,1007,54,5,55,1005,55,26,1001,54,
-5,54,1105,1,12,1,53,54,53,1008,54,0,55,1001,55,1,55,2,53,55,53,4,
53,1001,56,-1,56,1005,56,6,99,0,0,0,0,10 },
                new List<int> { 9,7,8,5,6 },
                18216
            },
        };

        [TestCaseSource("_sourceListsDay2")]
        public void Day2Test(List<long> code, List<long> outCode)
        {
            var io = new IntCodeComputerIO(new List<long>());
            var intCodeComputer = new IntCodeComputer(code, io, io);
            intCodeComputer.Compute();

            Assert.IsTrue(intCodeComputer.Code.SequenceEqual(outCode));
        }

        [TestCaseSource("_sourceListsDay5")]
        public void Day5Test(List<long> code, List<long> outCode)
        {
            var io = new IntCodeComputerIO(new List<long>() { 1 });

            var intCodeComputer = new IntCodeComputer(code, io, io);
            intCodeComputer.Compute();

            Assert.IsTrue(intCodeComputer.Code.SequenceEqual(outCode));
        }

        [TestCaseSource("_sourceListsDay5Part2")]
        public void Day5Part2Test(List<long> code, int input, int expectedOutput)
        {
            var io = new IntCodeComputerIO(new List<long>() { input });

            long output = default;
            var outputFunc = new DelegateOutput((long i) => output = i);

            var intCodeComputer = new IntCodeComputer(code, io, outputFunc);
            intCodeComputer.Compute();

            Assert.IsTrue(output == expectedOutput);
        }

        [TestCaseSource("_sourceListsDay7")]
        public void Day7Test(List<long> code, List<int> phaseSequence, int expectedOutput)
        {
            var amply = new AmplificationCircuitConfig(code, phaseSequence);
            var output = amply.ConfigureAmplifiers(0);
            Assert.IsTrue(output == expectedOutput);
        }

        [TestCaseSource("_sourceListsDay7Part2")]
        public void Day7TestPart2(List<long> code, List<int> phaseSequence, int expectedOutput)
        {
            var amply = new AmplificationCircuitConfig(code, phaseSequence);
            var output = amply.ConfigureAmplifiersFeedBackLoop(0);
            Assert.IsTrue(output == expectedOutput);
        }

        [Test]
        public void Day9Test1()
        {
            var origCode = new List<long>() { 109, 1, 204, -1, 1001, 100, 1, 100, 1008, 100, 16, 101, 1006, 101, 0, 99 };
            var code = origCode.ToList();
            var io = new IntCodeComputerIO(new List<long>());

            var output = new List<long>();
            var outputFunc = new DelegateOutput((long i) => output.Add(i));

            var intCodeComputer = new IntCodeComputer(code, io, outputFunc);
            intCodeComputer.ContinueAfterOutput = true;
            intCodeComputer.Compute();

            Assert.IsTrue(origCode.SequenceEqual(output));
        }

        [Test]
        public void Day9Test2()
        {
            var code = new List<long>() { 1102, 34915192, 34915192, 7, 4, 7, 99, 0 };
            var io = new IntCodeComputerIO(new List<long>());

            long output = default;
            var outputFunc = new DelegateOutput((long i) => output = i);

            var intCodeComputer = new IntCodeComputer(code, io, outputFunc);
            intCodeComputer.ContinueAfterOutput = true;
            intCodeComputer.Compute();

            Assert.IsTrue(16 == output.ToString().Length);
        }

        [Test]
        public void Day9Test3()
        {
            var code = new List<long>() { 104, 1125899906842624, 99 };
            var io = new IntCodeComputerIO(new List<long>());

            long output = default;
            var outputFunc = new DelegateOutput((long i) => output = i);

            var intCodeComputer = new IntCodeComputer(code, io, outputFunc);
            intCodeComputer.ContinueAfterOutput = true;
            intCodeComputer.Compute();

            Assert.IsTrue(1125899906842624 == output);
        }

    }
}
