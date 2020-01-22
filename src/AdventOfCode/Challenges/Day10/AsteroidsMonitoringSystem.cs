using Extension.Mathematics;
using Extension.Mathematics.VectorSpace;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day10
{
    public class AsteroidsMonitoringSystem
    {
        /// <summary>
        /// The asteroid map
        /// </summary>
        private List<List<char>> _asteroidMap;

        /// <summary>
        /// Creates a new monitoring station object for finding the best position 
        /// for a monitoring station within an asteroid field
        /// </summary>
        /// <param name="asteroids"></param>
        public AsteroidsMonitoringSystem(List<List<char>> asteroids)
        {
            _asteroidMap = asteroids;
        }

        /// <summary>
        /// Get the best position for the monitoring system
        /// </summary>
        /// <returns></returns>
        public (IntVector position, int detectScore) GetBestPosition()
        {
            var coordinates = GetAsteroidCoordinates().ToList();
            var scores = coordinates.Select(c =>
            {
                if (ContainsAsteroid(c))
                {
                    return GetScore(c);
                }
                else
                {
                    return 0;
                }
            }).ToList();

            var maxScore = scores.Max();
            var index = scores.IndexOf(maxScore);
            var maxCoordinates = coordinates[index];

            return (maxCoordinates, maxScore);
        }

        /// <summary>
        /// Get the score for the asteroid at the specified position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int GetScore(IntVector pol)
        {
            HashSet<IntVector> asteroidsDirections = new HashSet<IntVector>();
            GetAsteroidCoordinates().ToList().ForEach(coordinates =>
            {
                IntVector direction = GetDirection(pol, coordinates);
                var normalized = direction.Normalize();
                if (!normalized.All(e => e == 0))
                {
                    asteroidsDirections.Add(normalized);
                }
            });

            return asteroidsDirections.Count;
        }

        /// <summary>
        /// Returns the sequence in which the asteroids will be vaporized
        /// </summary>
        /// <returns></returns>
        public List<IntVector> GetVaporizationSequence(IntVector pol)
        {
            var all = GetAsteroidCoordinates();
            var notPol = all.Where(a => a != pol);
            var otherAsteroids = notPol.Select(a => GetDirection(pol, a)).ToList();
            var polarCoordinates = otherAsteroids.ToDictionary(a => a.GetPolarCoordinate());
            var grouped = polarCoordinates.Keys.GroupBy(a => a.Angle).ToDictionary(a => a.Key, a => a.OrderBy(b => b.Radius).ToList());
            var angles = grouped.Keys.OrderBy(k => k, new ClockwisePolarCoordinateComparer()).ToList();

            var result = new List<IntVector>();
            var count = 0;

            while (otherAsteroids.Count != count)
            {
                angles.ForEach(angle =>
                {
                    grouped.TryGetValue(angle, out var polars);
                    var nextP = polars.FirstOrDefault();
                    if (nextP != null)
                    {
                        polars.RemoveAt(0);
                        var next = GetCoordinateFromDirection(pol, polarCoordinates[nextP]);
                        result.Add(next);
                        count++;
                    }
                });
            }

            return result;
        }


        /// <summary>
        /// Get the direction from the system to an asteroid
        /// </summary>
        /// <returns></returns>
        private IntVector GetDirection(IntVector pol, IntVector coordinates)
        {
            return new IntVector(coordinates.X - pol.X, pol.Y - coordinates.Y);
        }

        /// <summary>
        /// Get coordinates from a given direction
        /// </summary>
        /// <returns></returns>
        private IntVector GetCoordinateFromDirection(IntVector pol, IntVector direction)
        {
            return new IntVector(direction.X + pol.X, pol.Y - direction.Y);
        }

        /// <summary>
        /// Get all coordinates in the current map that contain an asteroid
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IntVector> GetAsteroidCoordinates()
        {
            return GetMapCoordinates().Where(c => ContainsAsteroid(c));
        }

        /// <summary>
        /// Get all coordinates in the current map
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IntVector> GetMapCoordinates()
        {
            var xCoordinates = _asteroidMap[0].Count;
            var yCoordinates = _asteroidMap.Count;

            for (int y = 0; y < yCoordinates; y++)
            {
                for (int x = 0; x < xCoordinates; x++)
                {
                    yield return new IntVector(x, y);
                }
            }
        }

        /// <summary>
        /// Checks, if at the coordinate location contains an asteroid
        /// </summary>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        private bool ContainsAsteroid(IntVector position)
        {
            return _asteroidMap[position.Y][position.X] == '#';
        }

    }

    public class ClockwisePolarCoordinateComparer : IComparer<double>
    {
        public int Compare([AllowNull] double x, [AllowNull] double y)
        {
            return (GetQuardrant(x), GetQuardrant(y)) switch
            {
                (int qX, int qY) when qX != 1 && qY == 1 => 1,
                (int qX, int qY) when qX == 1 && qY != 1 => -1,
                _ => y.CompareTo(x),
            };
        }

        /// <summary>
        /// returns the quardrant from the angle
        /// </summary>
        /// <param name="x"></param>
        /// <returns>1, 2, 3, 4</returns>
        public int GetQuardrant(double x)
        {
            return x switch
            {
                _ when x >= 0 && x <= Math.PI / 2 => 1,
                _ when x > Math.PI / 2 && x <= Math.PI => 2,
                _ when x > Math.PI && x <= Math.PI * 3 / 2 => 3,
                _ when x > Math.PI * 3 / 2 && x <= Math.PI * 2 => 4,
                _ => throw new ArgumentOutOfRangeException("Values are out of range of [0, 2Pi)"),
            };
        }
    }
}
