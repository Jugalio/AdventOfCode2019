using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Challenges.IntCodeComputer;
using Extension.Mathematics.VectorSpace;

namespace AdventOfCode.Challenges.Day11
{
    public class HullPaintingRobot
    {
        private OutputHandles _handle = OutputHandles.ColorHandle;
        private IntCodeComputerIO io;
        private IntCodeComputerInstance _intCode;

        /// <summary>
        /// The next move the hull painting robot will do
        /// When started it is facing up so the initial vector here is (0,1)
        /// </summary>
        private IntVector _facingDirection = new IntVector(0, 1);

        /// <summary>
        /// The current position of the robot.
        /// Whens started this is always (0,0)
        /// </summary>
        public IntVector CurrentPosition = new IntVector(0, 0);

        /// <summary>
        /// All panels that are currently painted in white
        /// </summary>
        public HashSet<IntVector> WhitePanels = new HashSet<IntVector>();

        /// <summary>
        /// All panels that have been painted at least once
        /// </summary>
        public HashSet<IntVector> PaintedPanels = new HashSet<IntVector>();

        /// <summary>
        /// An object representing the hull paining robot
        /// </summary>
        /// <param name="code"></param>
        public HullPaintingRobot(List<long> code)
        {
            io = new IntCodeComputerIO(new List<long>());
            _intCode = new IntCodeComputerInstance(code, io, io);
        }

        /// <summary>
        /// Paints the hull with the given int code
        /// </summary>
        /// <param name="startColor">0 - Black, 1 - White</param>
        public void Paint(int startColor)
        {
            PaintPanel(CurrentPosition, startColor);
            SetNextInput();
            io.SentOutput += Io_SentOutput;

            _intCode.ContinueAfterOutput = true;
            _intCode.Compute();
        }

        /// <summary>
        /// Returns the painting
        /// </summary>
        /// <returns></returns>
        public List<List<int>> GetPainting()
        {
            var minX = WhitePanels.Min(v => v.X);
            var maxX = WhitePanels.Max(v => v.X);
            var minY = WhitePanels.Min(v => v.Y);
            var maxY = WhitePanels.Max(v => v.Y);

            var painting = new List<List<int>>();

            for (int i = 0; i <= maxY - minY; i++)
            {
                var newXVector = Enumerable.Repeat(0, maxX - minX + 1).ToList();
                painting.Add(newXVector);
            }

            WhitePanels.ToList().ForEach(p =>
            {
                painting[p.Y - minY][p.X - minX] = 1;
            });

            painting.Reverse();

            return painting;
        }

        /// <summary>
        /// New output receieved from the intcode computer
        /// </summary>
        /// <param name="s"></param>
        private void Io_SentOutput(long s)
        {
            switch (_handle)
            {
                case OutputHandles.ColorHandle:
                    PaintPanel(CurrentPosition, s);
                    _handle = OutputHandles.DirectionHandle;
                    break;
                case OutputHandles.DirectionHandle:
                    Turn(s);
                    Move();
                    _handle = OutputHandles.ColorHandle;
                    break;
                default:
                    throw new ArgumentException("Unknown handle");
            }
        }

        /// <summary>
        /// Turns the robot
        /// </summary>
        /// <param name="turnCode"></param>
        private void Turn(long turnCode)
        {
            switch (turnCode)
            {
                case 0:
                    TurnLeft();
                    break;
                case 1:
                    TurnRight();
                    break;
                default:
                    throw new ArgumentException("Unknown turn code");
            }
        }

        /// <summary>
        /// Moves the hull painting robot 1 panel to its current direction
        /// </summary>
        private void Move()
        {
            CurrentPosition = CurrentPosition + _facingDirection;
            SetNextInput();
        }

        /// <summary>
        /// Turns the robot right by 90°
        /// </summary>
        private void TurnRight()
        {
            _facingDirection = (_facingDirection.X, _facingDirection.Y) switch
            {
                (0, 1) => new IntVector(1, 0),
                (1, 0) => new IntVector(0, -1),
                (0, -1) => new IntVector(-1, 0),
                (-1, 0) => new IntVector(0, 1),
                _ => throw new ArgumentException("The current direction step is not a correct one"),
            };
        }

        /// <summary>
        /// Turns the robot right by 90°
        /// </summary>
        private void TurnLeft()
        {
            _facingDirection = (_facingDirection.X, _facingDirection.Y) switch
            {
                (0, 1) => new IntVector(-1, 0),
                (-1, 0) => new IntVector(0, -1),
                (0, -1) => new IntVector(1, 0),
                (1, 0) => new IntVector(0, 1),
                _ => throw new ArgumentException("The current direction step is not a correct one"),
            };
        }

        /// <summary>
        /// Paints a given panel
        /// </summary>
        /// <param name="position"></param>
        /// <param name="colorCode"></param>
        private void PaintPanel(IntVector position, long colorCode)
        {
            PaintedPanels.Add(position);
            switch (colorCode)
            {
                case 0:
                    WhitePanels.Remove(position);
                    break;
                case 1:
                    WhitePanels.Add(position);
                    break;
                default:
                    throw new ArgumentException("Unknown color code");
            }
        }

        /// <summary>
        /// Adds the next input
        /// </summary>
        private void SetNextInput()
        {
            var next = IsPanelWhite(CurrentPosition) ? 1 : 0;
            io.AddNewInput(next);
        }

        /// <summary>
        /// Checks if the given panel is white
        /// </summary>
        /// <param name="panel"></param>
        /// <returns></returns>
        private bool IsPanelWhite(IntVector panel)
        {
            return WhitePanels.Contains(panel);
        }

    }

    public enum OutputHandles
    {
        ColorHandle,
        DirectionHandle,
    }

}
