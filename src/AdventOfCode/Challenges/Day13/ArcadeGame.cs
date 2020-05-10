using AdventOfCode.Challenges.IntCodeComputer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MoreLinq;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day13
{
    public class ArcadeGame
    {
        private IntCodeComputerIO _io;
        private IntCodeComputerInstance _computer;
        private OutputHandle _handle;
        private int lastX;
        private int lastY;
        private (int x, int y) BallPosition;
        private (int x, int y) PanddlePosition;

        public ObservableCollection<Tile> Tiles = new ObservableCollection<Tile>();
        public int Score;

        public event TileUpdate NewTileValue;
        public delegate void TileUpdate(Tile t);

        /// <summary>
        /// Instanciates a new arcade game
        /// </summary>
        /// <param name="code"></param>
        public ArcadeGame(List<long> code)
        {
            _io = new IntCodeComputerIO(GetOptimalJoystickPosition);
            _handle = OutputHandle.XPos;
            _io.SentOutput += _io_SentOutput;

            _computer = new IntCodeComputerInstance(code, _io, _io);
            _computer.ContinueAfterOutput = true;
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void Start()
        {
            _computer.Compute();
        }

        /// <summary>
        /// Starts the game
        /// </summary>
        public void PlayFree()
        {
            _computer.Code[0] = 2;
            _computer.Compute();
        }

        /// <summary>
        /// Returns a map of the area
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetGameView()
        {
            var minX = Tiles.Min(v => v.X);
            var maxX = Tiles.Max(v => v.X);
            var minY = Tiles.Min(v => v.Y);
            var maxY = Tiles.Max(v => v.Y);

            var map = new List<List<int>>();

            for (int i = 0; i <= maxY - minY; i++)
            {
                var newXVector = Enumerable.Repeat(0, maxX - minX + 1).ToList();
                map.Add(newXVector);
            }

            Tiles.ToList().ForEach(p =>
            {
                map[p.Y - minY][p.X - minX] = p.Id switch
                {
                    TileId.Block => 0,
                    TileId.Empty => 1,
                    TileId.Ball => 2,
                    TileId.HorizontalPaddle => 3,
                    TileId.Wall => 2,
                    _ => 3,
                };
            });

            return map;
        }

        /// <summary>
        /// Gets the optimal position of the joystick depending on
        /// the position of the ball and the paddle
        /// </summary>
        /// <returns></returns>
        private long GetOptimalJoystickPosition()
        {
            var b = BallPosition.x;
            var p = PanddlePosition.x;

            return (b - p) switch
            {
                int i when i > 0 => 1,
                int i when i < 0 => -1,
                _  => 0,
            };
        }

        /// <summary>
        /// Reacts to an output of the intcode computer
        /// </summary>
        /// <param name="s"></param>
        private void _io_SentOutput(long s)
        {
            switch (_handle)
            {
                case OutputHandle.XPos:
                    lastX = (int)s;
                    _handle = OutputHandle.YPos;
                    break;
                case OutputHandle.YPos:
                    lastY = (int)s;
                    _handle = OutputHandle.Id;
                    break;
                case OutputHandle.Id:
                    HandleInstruction(lastX, lastY, (int)s);
                    _handle = OutputHandle.XPos;
                    break;
            }
        }

        /// <summary>
        /// Handles an instruction from three outputs
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="id"></param>
        private void HandleInstruction(int x, int y, int id)
        {
            switch (x, y)
            {
                case (-1, 0):
                    Score = id;
                    break;
                default:
                    var tile = new Tile(x, y, id);

                    if (tile.Id == TileId.Ball)
                    {
                        BallPosition = (x, y);
                    }
                    else if (tile.Id == TileId.HorizontalPaddle)
                    {
                        PanddlePosition = (x, y);
                    }

                    var exists = Tiles.FirstOrDefault(t => t.X == x && t.Y == y);
                    var index = Tiles.IndexOf(exists);

                    if (exists != null)
                    {
                        if (exists.Id != (TileId)id)
                        {
                            Tiles[index].Id = (TileId)id;
                            NewTileValue?.Invoke(Tiles[index]);
                        }
                    }
                    else
                    {
                        Tiles.Add(tile);
                    }                    
                    break;
            }
        }
    }

    public enum OutputHandle
    {
        XPos,
        YPos,
        Id,
    }
}
