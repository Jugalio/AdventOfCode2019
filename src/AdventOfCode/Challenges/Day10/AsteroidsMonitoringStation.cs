using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day10
{
    public class AsteroidsMonitoringStation
    {
        private List<List<char>> _asteroids;

        public AsteroidsMonitoringStation(List<List<char>> asteroids)
        {
            _asteroids = asteroids;
        }

        /// <summary>
        /// Get the best position for the monitoring system
        /// </summary>
        /// <returns></returns>
        public (string position, int detectScore) GetBestPosition()
        {
            var xCoordinates = _asteroids[0].Count;
            var yCoordinates = _asteroids.Count;
            var scores = new List<(string position, int detectScore)>();

            for (int y = 0; y < yCoordinates; y++)
            {
                for (int x = 0; x < xCoordinates; x++)
                {
                    if (_asteroids[y][x] == '#')
                    {
                        scores.Add(($"({x},{y})", GetScore(x, y)));
                    }
                }
            }

            return scores.MaxBy(a => a.detectScore).First();
        }

        /// <summary>
        /// Get the score for the asteroid at the specified position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int GetScore(int x, int y)
        {
            //Calc Score
            return default;
        }

    }
}
