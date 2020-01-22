using AdventOfCode.Challenges.IntCodeComputer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                    if (exists != null)
                    {
                        Tiles.Remove(exists);
                    }

                    Tiles.Add(tile);
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
