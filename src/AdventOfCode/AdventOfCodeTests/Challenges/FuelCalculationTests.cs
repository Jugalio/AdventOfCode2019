using AdventOfCode.Challenges.Day1;
using AdventOfCodeTests.Mocks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeTests.Challenges
{
    [TestFixture]
    public class FuelCalculationTests
    {

        [TestCase(12,2)]
        [TestCase(14, 2)]
        [TestCase(1969, 654)]
        [TestCase(100756, 33583)]
        public void TestCalculateFuelByMass(double mass, int fuel)
        {
            var calc = new FuelCalculation();
            Assert.IsTrue(calc.CalculateFuelByMass(mass) == fuel);
        }

        [TestCase(14, 2)]
        [TestCase(1969, 966)]
        [TestCase(100756, 50346)]
        public void TestCalculateFuelByFuel(double mass, int fuel)
        {
            var calc = new FuelCalculation();
            var fuelForMass = calc.CalculateFuelByMass(mass);
            var fuelForFuel = calc.CalcFuelForFuel(fuelForMass, 0);
            Assert.IsTrue(fuelForMass + fuelForFuel == fuel);
        }

    }
}
