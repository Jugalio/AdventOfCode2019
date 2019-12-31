using AdventOfCode.DataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day1
{
    public class FuelCalculation
    {
        public int CalculateFuel(IEnumerable<double> masses)
        {
            return masses.Select(mass =>
            {
                var fuelforModule = CalculateFuelByMass(mass);
                var fuelForFuel = CalcFuelForFuel(fuelforModule, 0);
                return fuelforModule + fuelForFuel;
            }).Sum();
        }

        public int CalcFuelForFuel(int addedFuel, int alreadyCountedFuel)
        {
            var additionalFuelNeeded = CalculateFuelByMass(addedFuel);
            if (additionalFuelNeeded <= 0)
            {
                return alreadyCountedFuel;
            }
            else
            {
                return CalcFuelForFuel(additionalFuelNeeded, alreadyCountedFuel + additionalFuelNeeded);
            }
        }

        public int CalculateFuelByMass(double mass)
        {
            var a = mass / 3;
            var b = (int)Math.Floor(a);
            return b - 2;
        }

    }
}
