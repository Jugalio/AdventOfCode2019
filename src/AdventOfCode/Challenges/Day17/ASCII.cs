using AdventOfCode.Challenges.IntCodeComputer;
using Extension.Mathematics.VectorSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day17
{
    /// <summary>
    /// The (A)ft (S)caffolding (C)ontrol and (I)nformation (I)nterface
    /// is used to get a map of the outer scaffolding from the space ship
    /// </summary>
    public class ASCII
    {
        private int _currentRow;
        private int _currentColumn;
        private int _maxTiles = int.MaxValue;
        private IntCodeComputerInstance _computer;
        private IntCodeComputerIO _io;
        private List<IntVector> _scaffold = new List<IntVector>();

        public long DustCollected;
        public IntVector VacuumRobot;
        public char VacuumRobotStatus;
        private char[] _allowed = new char[5] { '<', '>', '^', 'v', 'X' };

        /// <summary>
        /// New instance of the ascii system
        /// </summary>
        /// <param name="code"></param>
        public ASCII(List<long> code)
        {
            _io = new IntCodeComputerIO(new List<long> { });
            _io.SentOutput += _io_SentOutput;
            _computer = new IntCodeComputerInstance(code, _io, _io);
            _computer.ContinueAfterOutput = true;
        }

        /// <summary>
        /// STarts the scan of the area
        /// </summary>
        public void Scan()
        {
            _computer.Reset();
            _scaffold = new List<IntVector>();
            _currentRow = 0;
            _currentColumn = 0;
            _computer.Compute();

            _maxTiles = _currentColumn * _currentRow;
        }

        /// <summary>
        /// STarts the scan of the area
        /// </summary>
        public void StartRobot(List<char> mainRoutine, List<char> a, List<char> b, List<char> c, bool videoFeed)
        {
            //_computer.Reset();
            _computer.Code[0] = 2;

            mainRoutine.AddRange(a);
            mainRoutine.AddRange(b);
            mainRoutine.AddRange(c);
            mainRoutine.Add(videoFeed ? 'y' : 'n');
            mainRoutine.Add('\n');

            mainRoutine.Select(c => (int)c).ToList().ForEach(v => _io.AddNewInput(v));

            _computer.Compute();
        }

        /// <summary>
        /// Returns the alignment parameters
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetAlignmentParameters()
        {
            var inter = GetIntersections();
            return inter.Select(i => i.X * i.Y);
        }

        /// <summary>
        /// Returns a map of the scaffold
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetScaffoldMap()
        {
            var maxX = _scaffold.Max(v => v.X);
            var maxY = _scaffold.Max(v => v.Y);

            var map = new List<List<int>>();

            for (int i = 0; i <= maxY; i++)
            {
                var newXVector = Enumerable.Repeat(0, maxX + 1).ToList();
                map.Add(newXVector);
            }

            map[VacuumRobot.Y][VacuumRobot.X] = 2;

            _scaffold.Where(p => p != VacuumRobot).ToList().ForEach(p =>
            {
                map[p.Y][p.X] = 1;
            });

            return map;
        }

        /// <summary>
        /// Get the intersections in the scaffold
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IntVector> GetIntersections()
        {
            foreach (IntVector v in _scaffold)
            {
                var neighBors = GetNeighbors(v);
                if (neighBors.Count() == 4)
                {
                    yield return v;
                }
            }
        }

        /// <summary>
        /// Get the scaffold elements that are directly adjacent to the target
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private IEnumerable<IntVector> GetNeighbors(IntVector target)
        {
            return _scaffold.Where(s => s.DistanceTo(target) == 1);
        }

        /// <summary>
        /// A new scan from the cameras comes through
        /// </summary>
        /// <param name="s"></param>
        private void _io_SentOutput(long s)
        {
            if (s > 127)
            {
                DustCollected = s;
            }
                
            var output = (char)s;
            switch (output)
            {
                case '.':
                    _currentColumn++;
                    break;
                case '\n':
                    _currentColumn = 0;
                    _currentRow++;
                    break;
                case '#':
                    _scaffold.Add(new IntVector(_currentColumn, _currentRow));
                    _currentColumn++;
                    break;
                default:
                    AddVacuumRobot(new IntVector(_currentColumn, _currentRow), output);
                    break;
            }

            if(_currentRow * _currentColumn >= _maxTiles)
            {
                _currentRow = 0;
                _currentColumn = 0;
            }
        }

        private void AddVacuumRobot(IntVector position, char status)
        {
            if (!_allowed.Contains(status))
            {
                //Do nothing
            }
            else
            {
                AddScaffold(position);
                VacuumRobot = position;
                VacuumRobotStatus = status;
            }
        }

        private void AddScaffold(IntVector position)
        {
            _scaffold.Add(position);
            _currentColumn++;
        }
    }
}
