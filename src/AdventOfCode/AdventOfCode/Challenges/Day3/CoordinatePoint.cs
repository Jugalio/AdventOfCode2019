using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.Day3
{
    public class CoordinatePoint
    {
        public int X;
        public int Y;

        /// <summary>
        /// Creates a coordinate point with grid coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public CoordinatePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Creates a new point
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public CoordinatePoint GoUp(int by)
        {
            return new CoordinatePoint(X, Y + by);
        }

        /// <summary>
        /// Creates a new point
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public CoordinatePoint GoDown(int by)
        {
            return new CoordinatePoint(X, Y - by);
        }

        /// <summary>
        /// Creates a new point
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public CoordinatePoint GoRight(int by)
        {
            return new CoordinatePoint(X + by, Y);
        }

        /// <summary>
        /// Creates a new point
        /// </summary>
        /// <param name="by"></param>
        /// <returns></returns>
        public CoordinatePoint GoLeft(int by)
        {
            return new CoordinatePoint(X - by, Y);
        }

        /// <summary>
        /// Measures the distance of two point with a given metric.
        /// Default is manhatten metric
        /// </summary>
        /// <param name="otherPoint"></param>
        /// <param name="metric"></param>
        /// <returns></returns>
        public int MeasureDistance(CoordinatePoint otherPoint, Metric metric = Metric.Manhatten)
        {
            return metric switch
            {
                Metric.Manhatten => Math.Abs(X - otherPoint.X) + Math.Abs(Y - otherPoint.Y),
                _ => throw new ArgumentException("Metric is not defined"),
            };
        }

        /// <summary>
        /// A point is equal to another point, if their coordinates are equal
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return obj switch
            {
                CoordinatePoint p => X == p.X && Y == p.Y,
                _ => false,
            };
        }

        /// <summary>
        /// Returns the hash code of a coordinate point
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return X.GetHashCode() + Y.GetHashCode();
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }

    public enum Metric
    {
        Manhatten,
    }
}
