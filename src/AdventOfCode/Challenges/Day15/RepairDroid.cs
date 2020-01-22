using AdventOfCode.Challenges.IntCodeComputer;
using Extension.Mathematics.VectorSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day15
{
    public class RepairDroid
    {
        private IntCodeComputerInstance _computer;
        private IntCodeComputerIO _io;
        private IntVector _currentLocation;
        private List<IntVector> _openSpace = new List<IntVector>();
        private Stack<MovementCommands> _commands = new Stack<MovementCommands>();
        private MovementCommands _currentDirection = MovementCommands.NotSet;

        public IntVector OxygenSystem;

        /// <summary>
        /// Creates a new instance of the repair droid
        /// </summary>
        /// <param name="code"></param>
        public RepairDroid(List<long> code)
        {
            _io = new IntCodeComputerIO(GetNextMovementCommand);
            _io.SentOutput += _io_SentOutput;
            _computer = new IntCodeComputerInstance(code, _io, _io);
        }

        /// <summary>
        /// Explores the area the droid is located in
        /// </summary>
        public void Explore()
        {
            _currentLocation = new IntVector(0, 0);
            _openSpace.Add(_currentLocation);
            Enum.GetValues(typeof(MovementCommands)).Cast<MovementCommands>().Skip(1).ToList().ForEach(c => _commands.Push(c));

            while (_commands.Count != 0)
            {
                _computer.Compute();
            }
        }

        /// <summary>
        /// Get the shortest route to a specific location
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        public List<IntVector> GetShortestRoute(IntVector from, IntVector to)
        {
            //If not yet explored the area has to be explored first
            if (_openSpace.Count == 0)
            {
                Explore();
            }

            var tilesToCheck = _openSpace.Where(p => p != to).ToList();
            var routes = new List<List<IntVector>>() { new List<IntVector> { to } };

            do
            {
                routes = routes.SelectMany(route =>
                {
                    var last = route.Last();
                    var nextIteration = tilesToCheck.Where(t => t.DistanceTo(last) == 1).ToList();
                    nextIteration.ForEach(i => tilesToCheck.Remove(i));
                    return nextIteration.Select(i => route.Append(i).ToList());
                }).ToList();
            } while (tilesToCheck.Contains(from));

            var route = routes.FirstOrDefault(r => r.Contains(from));
            route.Reverse();
            return route;
        }

        /// <summary>
        /// Returns the time in minutes the oxygen need to spread to all other location starting from a given position
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public int GetOxygenSpreadingTime(IntVector from)
        {
            //If not yet explored the area has to be explored first
            if (_openSpace.Count == 0)
            {
                Explore();
            }

            var tilesToCheck = _openSpace.Where(p => p != from).ToList();
            var spreadingRoutes = new List<List<IntVector>>() { new List<IntVector> { from } };
            var time = 0;

            do
            {
                spreadingRoutes = spreadingRoutes.SelectMany(route =>
                {
                    var last = route.Last();
                    var nextIteration = tilesToCheck.Where(t => t.DistanceTo(last) == 1).ToList();
                    nextIteration.ForEach(i => tilesToCheck.Remove(i));
                    return nextIteration.Select(i => route.Append(i).ToList());
                }).ToList();

                time++;

            } while (tilesToCheck.Count != 0);

            return time;
        }

        /// <summary>
        /// Returns a map of the area
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetMap()
        {
            var minX = _openSpace.Min(v => v.X);
            var maxX = _openSpace.Max(v => v.X);
            var minY = _openSpace.Min(v => v.Y);
            var maxY = _openSpace.Max(v => v.Y);

            var map = new List<List<int>>();

            for (int i = 0; i <= maxY - minY; i++)
            {
                var newXVector = Enumerable.Repeat(0, maxX - minX + 1).ToList();
                map.Add(newXVector);
            }

            var start = new IntVector(0, 0);
            map[-minY][-minX] = 2;
            map[OxygenSystem.Y - minY][OxygenSystem.X - minX] = 2;

            _openSpace.Where(l => l != start).ToList().ForEach(p =>
            {
                map[p.Y - minY][p.X - minX] = 1;
            });

            map.Reverse();

            return map;
        }

        /// <summary>
        /// Returns a map of the area with the route marked
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetRouteMap(IntVector from, IntVector to)
        {
            return GetRouteMap(GetShortestRoute(from, to));
        }

        /// <summary>
        /// Returns a map of the area with the route marked
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetRouteMap(List<IntVector> route)
        {
            var minX = _openSpace.Min(v => v.X);
            var maxX = _openSpace.Max(v => v.X);
            var minY = _openSpace.Min(v => v.Y);
            var maxY = _openSpace.Max(v => v.Y);

            var map = new List<List<int>>();

            for (int i = 0; i <= maxY - minY; i++)
            {
                var newXVector = Enumerable.Repeat(0, maxX - minX + 1).ToList();
                map.Add(newXVector);
            }

            _openSpace.ToList().ForEach(p =>
            {
                if (route.Contains(p))
                {
                    map[p.Y - minY][p.X - minX] = 3;
                }
                else
                {
                    map[p.Y - minY][p.X - minX] = 1;
                }
            });

            var start = route.First();
            var target = route.Last();

            map[start.Y - minY][start.X - minX] = 2;
            map[target.Y - minY][target.X - minX] = 2;

            map.Reverse();

            return map;
        }

        /// <summary>
        /// The repair droid provided a new output
        /// </summary>
        /// <param name="s"></param>
        private void _io_SentOutput(long s)
        {
            var status = (Status)s;
            switch (status)
            {
                case Status.Wall:
                    break;
                case Status.SuccessfulMove:
                    UpdateLocation();
                    CheckCurrentLocation();
                    break;
                case Status.OxygenSystem:
                    UpdateLocation();
                    CheckCurrentLocation();
                    OxygenSystem = _currentLocation;
                    break;
            }
        }

        /// <summary>
        /// Updates the current location
        /// </summary>
        private void UpdateLocation()
        {
            switch (_currentDirection)
            {
                case MovementCommands.North:
                    _currentLocation += (0, 1);
                    break;
                case MovementCommands.South:
                    _currentLocation -= (0, 1);
                    break;
                case MovementCommands.West:
                    _currentLocation += (1, 0);
                    break;
                case MovementCommands.East:
                    _currentLocation -= (1, 0);
                    break;
            }
        }

        /// <summary>
        /// Logic executed when arriving on a new location
        /// </summary>
        /// <param name="loc"></param>
        private void CheckCurrentLocation()
        {
            //If the location was already visited nothing happens
            //if not all neighbor locations need to be checked
            if (!_openSpace.Contains(_currentLocation))
            {
                _openSpace.Add(_currentLocation);
                var from = GetOppositDirection();
                _commands.Push(from);
                var addedCommands = Enum.GetValues(typeof(MovementCommands)).Cast<MovementCommands>().Skip(1).Where(c => c != from);
                addedCommands.ToList().ForEach(c => _commands.Push(c));
            }
        }

        /// <summary>
        /// Iterates the direction in which the droid is moving
        /// </summary>
        private MovementCommands GetOppositDirection()
        {
            return _currentDirection switch
            {
                MovementCommands.North => MovementCommands.South,
                MovementCommands.East => MovementCommands.West,
                MovementCommands.South => MovementCommands.North,
                MovementCommands.West => MovementCommands.East,
                _ => throw new ArgumentException("Unknown"),
            };
        }

        /// <summary>
        /// Gets the next movement command
        /// </summary>
        /// <returns></returns>
        private long GetNextMovementCommand()
        {
            _currentDirection = _commands.Pop();
            return (long)_currentDirection;
        }

    }

    /// <summary>
    /// List of movement commands for the repair droid
    /// </summary>
    public enum MovementCommands
    {
        NotSet,
        North,
        South,
        West,
        East,
    }

    /// <summary>
    /// List of status replies
    /// </summary>
    public enum Status
    {
        Wall,
        SuccessfulMove,
        OxygenSystem
    }
}
