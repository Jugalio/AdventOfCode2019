using AdventOfCode.Challenges.Day16;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class FFTTest
    {
        [TestCase("12345678", "01029498", 4)]
        [TestCase("80871224585914546619083218645595", "24176176", 100)]
        [TestCase("19617804207202209144916044189917", "73745418", 100)]
        [TestCase("69317163492948606335995924319873", "52432133", 100)]
        public void TestSignalFiltering(string input, string output, int repeat)
        {
            var fft = new FFT(input);
            fft.Run(repeat);

            Assert.IsTrue(output == fft.Signal.Substring(0, 8));
        }

        [TestCase("03036732577212944063491565474664", "84462026", 100)]
        [TestCase("02935109699940807407585447034323", "78725270", 100)]
        [TestCase("03081770884921959731165446850517", "53553731", 100)]
        public void TestPart2(string input, string output, int repeat)
        {
            var fft = new FFT(input);
            fft.RepeatSignal(10000);

            fft.RunForMessage(repeat);

            Assert.IsTrue(output == fft.Message);
        }

    }
}
