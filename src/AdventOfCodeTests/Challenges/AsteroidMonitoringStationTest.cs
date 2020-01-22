using AdventOfCode.Challenges.Day10;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class AsteroidMonitoringStationTest
    {
        private static readonly object[] _sourceListsDay =
{
            new object[]
            {
                new List<List<char>>
                {
                    new List<char>{'.', '.', '.', '.', '.', '.', '#', '.', '#', '.'},
                    new List<char>{'#', '.', '.', '#', '.', '#', '.', '.', '.', '.'},
                    new List<char>{'.', '.', '#', '#', '#', '#', '#', '#', '#', '.'},
                    new List<char>{'.', '#', '.', '#', '.', '#', '#', '#', '.', '.'},
                    new List<char>{'.', '#', '.', '.', '#', '.', '.', '.', '.', '.'},
                    new List<char>{'.', '.', '#', '.', '.', '.', '.', '#', '.', '#'},
                    new List<char>{'#', '.', '.', '#', '.', '.', '.', '.', '#', '.'},
                    new List<char>{'.', '#', '#', '.', '#', '.', '.', '#', '#', '#'},
                    new List<char>{'#', '#', '.', '.', '.', '#', '.', '.', '#', '.'},
                    new List<char>{'.', '#', '.', '.', '.', '.', '#', '#', '#', '#'},
                },
                5,
                8,
                33
            },
        };

        [TestCase(1, 0, 7)]
        [TestCase(4, 0, 7)]
        [TestCase(0, 2, 6)]
        [TestCase(4, 2, 5)]
        [TestCase(3, 4, 8)]
        public void TestScore(int x, int y, int expectedScore)
        {
            var map = new List<List<char>>
                {
                    new List<char>{'.', '#', '.', '.', '#'},
                    new List<char>{'.', '.', '.', '.', '.'},
                    new List<char>{'#', '#', '#', '#', '#'},
                    new List<char>{'.', '.', '.', '.', '#'},
                    new List<char>{'.', '.', '.', '#', '#'},
                };

            var testObject = new AsteroidsMonitoringSystem(map);

            var score = testObject.GetScore(new Extension.Mathematics.VectorSpace.IntVector(x, y));
            Assert.IsTrue(score == expectedScore);
        }

        [TestCaseSource("_sourceListsDay")]
        public void TestMap(List<List<char>> map, int expectedX, int expectedY, int expectedScore)
        {

            var testObject = new AsteroidsMonitoringSystem(map);

            var (position, score) = testObject.GetBestPosition();

            Assert.IsTrue(position.X == expectedX);
            Assert.IsTrue(position.Y == expectedY);
            Assert.IsTrue(score == expectedScore);
        }

    }
}
