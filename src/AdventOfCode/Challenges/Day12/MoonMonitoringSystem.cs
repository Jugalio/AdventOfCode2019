using Extension.Mathematics;
using Extension.Mathematics.VectorSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day12
{
    /// <summary>
    /// The moon monitoring system is used to simulate the movement of a given number of
    /// moons given its initial position
    /// </summary>
    public class MoonMonitoringSystem
    {
        /// <summary>
        /// The current positions of the moons
        /// </summary>
        public List<IntVector> CurrentPositions;

        /// <summary>
        /// The current positions of the moons
        /// </summary>
        public List<IntVector> CurrentVelocity;

        /// <summary>
        /// Creates a new object with a given list of moons and there positions
        /// </summary>
        /// <param name="moons"></param>
        public MoonMonitoringSystem(IEnumerable<IntVector> moons)
        {
            CurrentPositions = moons.ToList();
            CurrentVelocity = moons.Select(m => new IntVector(0, 0, 0)).ToList();
        }

        /// <summary>
        /// Simulates the system
        /// </summary>
        /// <param name="timeSteps"></param>
        public void Simulate(int timeSteps)
        {
            for (int i = 0; i < timeSteps; i++)
            {
                NextTimeStep();
            }
        }

        /// <summary>
        /// Does the next timestep by calculating the velocity and apply it to the current positions
        /// </summary>
        public void NextTimeStep()
        {
            UpdateVelocity();
            CurrentPositions = CurrentPositions.Zip(CurrentVelocity, (p, v) => p + v).ToList();
        }

        /// <summary>
        /// Returns the total energy in the system
        /// </summary>
        /// <returns></returns>
        public int GetTotalEnegry()
        {
            return GetPotentialEnergy().Zip(GetKineticEnergy(), (a, b) => a * b).Sum();
        }

        /// <summary>
        /// Returns the vector with the potential energy of each moon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetPotentialEnergy()
        {
            return CurrentPositions.Select(p => p.Select(e => Math.Abs(e)).Sum());
        }

        /// <summary>
        /// Returns the vector with the kinetic energy of each moon
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetKineticEnergy()
        {
            return CurrentVelocity.Select(p => p.Select(e => Math.Abs(e)).Sum());
        }

        /// <summary>
        /// Returns the number of stepes needed to put the univers back to its original position
        /// </summary>
        /// <returns></returns>
        public ulong GetRepetitionCount()
        {
            ulong xRep = GetCicleLength(CurrentPositions.Select(p => p.X));
            ulong yRep = GetCicleLength(CurrentPositions.Select(p => p.Y));
            ulong zRep = GetCicleLength(CurrentPositions.Select(p => p.Z));

            var lcm1 = Operations.LCM(xRep, yRep);
            var lcm2 = Operations.LCM(lcm1, zRep);

            return lcm2;
        }

        /// <summary>
        /// Returns the length of the cicle for one axis
        /// </summary>
        /// <returns></returns>
        private ulong GetCicleLength(IEnumerable<int> dimensionPositions)
        {
            var velocities = CalculateOneDimensionVelocity(dimensionPositions).ToList();
            var compare = dimensionPositions.ToList();
            ulong length = 1;

            do
            {
                length++;
                dimensionPositions = dimensionPositions.Zip(velocities, (a, b) => a + b).ToList();
                velocities = velocities.Zip(CalculateOneDimensionVelocity(dimensionPositions), (a, b) => a + b).ToList();

            } while (!compare.SequenceEqual(dimensionPositions));

            return length;
        }

        /// <summary>
        /// Updates the velocity vectors in the system
        /// </summary>
        private void UpdateVelocity()
        {
            CurrentVelocity = GetVelocity().ToList();
        }

        /// <summary>
        /// Returns the velocity vector to the current positions vectors
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IntVector> GetVelocity()
        {
            var xVeloChange = CalculateOneDimensionVelocity(CurrentPositions.Select(p => p.X));
            var yVeloChange = CalculateOneDimensionVelocity(CurrentPositions.Select(p => p.Y));
            var zVeloChange = CalculateOneDimensionVelocity(CurrentPositions.Select(p => p.Z));

            for (int i = 0; i < CurrentVelocity.Count(); i++)
            {
                yield return CurrentVelocity[i] + new IntVector(xVeloChange.ElementAt(i), yVeloChange.ElementAt(i), zVeloChange.ElementAt(i));
            }
        }

        /// <summary>
        /// Gets the velocity per dimension.
        /// Given all x,y or z positions it outputs the new x,y or z velocities
        /// </summary>
        /// <param name="dimensionPositions"></param>
        /// <returns></returns>
        private IEnumerable<int> CalculateOneDimensionVelocity(IEnumerable<int> dimensionPositions)
        {
            foreach (int pos in dimensionPositions)
            {
                var lesser = dimensionPositions.Where(p => p < pos).Count();
                var greater = dimensionPositions.Where(p => p > pos).Count();
                yield return greater - lesser;
            }
        }

    }
}
