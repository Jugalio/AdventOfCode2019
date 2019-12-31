using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day3
{
    /// <summary>
    /// A wire is a line on a grid which is described by list direction operations
    /// that build up to a path
    /// </summary>
    public class Wire
    {
        private List<CoordinatePoint> _coordinates;

        /// <summary>
        /// The starting point of the wire
        /// </summary>
        public CoordinatePoint StartingPoint { get; private set; }

        /// <summary>
        /// The path the wire takes from the starting point
        /// </summary>
        public IEnumerable<string> Path { get; private set; }

        /// <summary>
        /// Creates a new instance of a wire
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="path"></param>
        public Wire(CoordinatePoint startingPoint, IEnumerable<string> path)
        {
            StartingPoint = startingPoint;
            Path = path;
        }

        /// <summary>
        /// Get all coordinates that the wire hits on its path
        /// </summary>
        /// <returns></returns>
        public List<CoordinatePoint> GetPathCoordinates()
        {
            //Calculate the coordinates only once
            if (_coordinates == null)
            {
                _coordinates = new List<CoordinatePoint>();
                _coordinates.Add(StartingPoint);

                for (int i = 0; i < Path.Count(); i++)
                {
                    HandlePathOperation(i, ref _coordinates);
                }
            }

            return _coordinates;
        }

        /// <summary>
        /// Handles the path operation at the given index
        /// </summary>
        /// <param name="index"></param>
        /// <param name="coordinates"></param>
        /// <returns></returns>
        private void HandlePathOperation(int index, ref List<CoordinatePoint> coordinates)
        {
            if (index >= Path.Count())
            {
                throw new ArgumentException("The index was not found in the path");
            }
            else
            {
                var operation = Path.ToList()[index];
                var transFormationFunction = GetTransformationFunction(operation);
                var steps = int.Parse(operation.Substring(1));
                var lastPoint = coordinates.Last();

                for (int i = 1; i <= steps; i++)
                {
                    coordinates.Add(transFormationFunction(lastPoint, i));
                }
            }
        }

        /// <summary>
        /// Decides which transformation function to use depending on the direction identifier
        /// in the operation code
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        private Func<CoordinatePoint, int, CoordinatePoint> GetTransformationFunction(string operation)
        {
            var direction = operation.Substring(0, 1);

            return direction switch
            {
                "U" => (CoordinatePoint lastPoint, int by) => lastPoint.GoUp(by),
                "D" => (CoordinatePoint lastPoint, int by) => lastPoint.GoDown(by),
                "R" => (CoordinatePoint lastPoint, int by) => lastPoint.GoRight(by),
                "L" => (CoordinatePoint lastPoint, int by) => lastPoint.GoLeft(by),
                _ => throw new ArgumentException($"Operation direction {direction} is unknown")
            };
        }

        /// <summary>
        /// Returns all intersections points this wire and the other wire have
        /// </summary>
        /// <param name="otherWire"></param>
        /// <returns></returns>
        public List<CoordinatePoint> GetIntersectionsWith(Wire otherWire)
        {
            var thisCoordinates = GetPathCoordinates().ToHashSet();
            var otherCoordinates = otherWire.GetPathCoordinates().ToHashSet();
            var intersections = thisCoordinates.Where(c => otherCoordinates.Contains(c) && c != StartingPoint).ToList();

            return intersections;
        }

        /// <summary>
        /// Returns the number of steps a wire needs to reach a given reference point.
        /// -1 if never reached
        /// </summary>
        /// <param name="referencePoint"></param>
        /// <returns></returns>
        public int GetStepsToReach(CoordinatePoint referencePoint)
        {
            var coordinates = GetPathCoordinates();
            return coordinates.IndexOf(referencePoint);
        }

        /// <summary>
        /// Returns the intersection of the two wires which can be found using the minimum number of combined steps
        /// the wires need to reach it
        /// </summary>
        /// <param name="otherWire"></param>
        /// <returns></returns>
        public (CoordinatePoint point, int steps) GetFirstIntersectionWith(Wire otherWire)
        {
            var intersections = GetIntersectionsWith(otherWire);

            var steps1 = intersections.Select(c => GetStepsToReach(c));
            var steps2 = intersections.Select(c => otherWire.GetStepsToReach(c));
            var combinedSteps = steps1.Zip(steps2).Select(tupel => tupel.First + tupel.Second).ToList();

            var minSteps = combinedSteps.Min();
            var firstIntersection = intersections[combinedSteps.IndexOf(minSteps)];

            return (firstIntersection, minSteps);
        }
    }
}