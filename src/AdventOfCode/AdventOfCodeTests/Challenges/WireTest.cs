using AdventOfCode.Challenges.Day3;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class WireTest
    {

        /// <summary>
        /// Testcases
        /// </summary>
        private static readonly object[] _sourceLists =
        {
            new object[]
            {
                new List<string> { "R8", "U5", "L5", "D3" },
                new List<string> { "U7","R6","D4","L4" },
                6,
                30
            },  //case 1
            new object[]
            {
                new List<string> { "R75","D30","R83","U83","L12","D49","R71","U7","L72" },
                new List<string> { "U62","R66","U55","R34","D71","R55","D58","R83" },
                159,
                610
            },  //case 2
            new object[]
            {
                new List<string> { "R98","U47","R26","D63","R33","U87","L62","D20","R33","U53","R51" },
                new List<string> { "U98","R91","D20","R16","D67","R40","U7","R15","U6","R7" },
                135,
                410
            },  //case 3
        };

        [TestCaseSource("_sourceLists")]
        public void DistanceAndStepTest(List<string> wire1, List<string> wire2, int distance, int steps)
        {
            var startingPoint = new CoordinatePoint(0, 0);

            var w1 = new Wire(startingPoint, wire1);
            var w2 = new Wire(startingPoint, wire2);

            var intersections = w1.GetIntersectionsWith(w2);

            var board = new GridBoard();
            var closestPoint = board.GetClosestPointTo(intersections, startingPoint);

            var firstIntersection = w1.GetFirstIntersectionWith(w2);

            Assert.IsTrue(closestPoint.distance == distance);
            Assert.IsTrue(firstIntersection.steps == steps);
        }

    }
}
