using AdventOfCode.Challenges.Day12;
using Extension.Mathematics.VectorSpace;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class MoonMonitoringSystemTest
    {

        private static readonly object[] _sourceList1 = new object[]
        {
            new object[]
            {
               new List<IntVector>
                {
                    new IntVector(-1,0,2),
                    new IntVector(2,-10,-7),
                    new IntVector(4,-8,8),
                    new IntVector(3,5,-1),
                },
                new List<IntVector>
                {
                    new IntVector(2,-1,1),
                    new IntVector(3,-7,-4),
                    new IntVector(1,-7,5),
                    new IntVector(2,2,0),
                }
            },
        };

        [TestCaseSource("_sourceList1")]
        public void TestTimeStep(IEnumerable<IntVector> startingPosition, IEnumerable<IntVector> expectedPosition)
        {
            var system = new MoonMonitoringSystem(startingPosition);
            system.NextTimeStep();

            var positions = system.CurrentPositions;
            var equal = positions.SequenceEqual(expectedPosition);

            Assert.IsTrue(equal);
        }

        [TestCaseSource("_sourceList1")]
        public void TestEnergy(IEnumerable<IntVector> startingPosition, IEnumerable<IntVector> doesntMatter)
        {
            var system = new MoonMonitoringSystem(startingPosition);

            system.Simulate(10);

            var energy = system.GetTotalEnegry();

            Assert.IsTrue(energy == 179);
        }

        [TestCaseSource("_sourceList1")]
        public void RepetitionTest(IEnumerable<IntVector> startingPosition, IEnumerable<IntVector> doesntMatter)
        {
            var system = new MoonMonitoringSystem(startingPosition);

            var count = system.GetRepetitionCount();

            Assert.IsTrue(count == 2772);
        }

    }
}
