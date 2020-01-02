using AdventOfCode.Challenges.Day4;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class CriteriaFunctionTests
    {

        [TestCase("111111", true)]
        [TestCase("223450", false)]
        [TestCase("123789", false)]
        public void TestFirstCriteria(string psw, bool expectedResult)
        {
            var result = CriteriaFunctions.AdjacentEqual(psw) && CriteriaFunctions.NeverDecreasing(psw);
            Assert.IsTrue(result == expectedResult);
        }

        [TestCase("112233", true)]
        [TestCase("123444", false)]
        [TestCase("111122", true)]
        public void TestSecondCriteria(string psw, bool expectedResult)
        {
            var result = CriteriaFunctions.AdjacentEqualNoGroup(psw) && CriteriaFunctions.NeverDecreasing(psw);
            Assert.IsTrue(result == expectedResult);
        }

    }
}
