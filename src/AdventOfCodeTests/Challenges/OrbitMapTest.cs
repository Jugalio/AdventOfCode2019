using AdventOfCode.Challenges.Day6;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class OrbitMapTest
    {
        private static readonly object[] _sourceLists =
        {
            new object[]
            {
                new List<string> { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L" },
                42
            },
        };

        private static readonly object[] _sourceListsNav =
        {
            new object[]
            {
                new List<string> { "COM)B", "B)C", "C)D", "D)E", "E)F", "B)G", "G)H", "D)I", "E)J", "J)K", "K)L", "K)YOU", "I)SAN" },
                4,
                "YOU",
                "SAN"
            },
        };

        [TestCaseSource("_sourceLists")]
        public void TestOrbits(IEnumerable<string> relations, int expectedOrbits)
        {
            var map = new OrbitMap(relations);

            var orbits = map.GetNumberOfOrbits();
            Assert.IsTrue(orbits == expectedOrbits);
        }

        [TestCaseSource("_sourceListsNav")]
        public void TestNavigation(IEnumerable<string> relations, int expectedTransfers, string from, string to)
        {
            var map = new OrbitMap(relations);
            var objA = map.SpaceObjects.FirstOrDefault(so => so.Id == from);
            var objB = map.SpaceObjects.FirstOrDefault(so => so.Id == to);

            var (transfers, path) = objA.GetTransfersToReach(objB);
            Assert.IsTrue(transfers == expectedTransfers);
        }

    }
}
