using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day3
{
    /// <summary>
    /// A board which is ordered in a grid
    /// </summary>
    public class GridBoard
    {

        /// <summary>
        /// Returns the closest point from a list of candidates to a given reference Point on the grid
        /// </summary>
        /// <param name="candidates"></param>
        /// <param name="referencePoint"></param>
        /// <param name="metric">The default metric is Manhatten</param>
        /// <returns></returns>
        public (CoordinatePoint point, int distance) GetClosestPointTo(List<CoordinatePoint> candidates, CoordinatePoint referencePoint, Metric metric = Metric.Manhatten)
        {

            var distances = candidates.Select(c => c.MeasureDistance(referencePoint, metric));
            var shortestDistance = distances.Min();
            var indexOfShortestDistance = distances.ToList().IndexOf(shortestDistance);

            return (candidates[indexOfShortestDistance], shortestDistance);

        }

    }
}
