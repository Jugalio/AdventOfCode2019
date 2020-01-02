using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class Instruction
    {
        public int InstructionPointer;
        public InstructionModes OpCode;
        public List<ParameterModes> Modes = new List<ParameterModes>();

        /// <summary>
        /// Returns the next instruction pointer
        /// </summary>
        public int NextPointer { get; set; }

        /// <summary>
        /// Creates a new instruction object from where we can get all the parameters
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        public Instruction(int instructionPointer, List<int> code)
        {
            InstructionPointer = instructionPointer;

            var stringInstruction = string.Format($"{{0:00}}", code[instructionPointer]);
            OpCode = (InstructionModes)int.Parse(stringInstruction.Substring(stringInstruction.Length - 2));

            var leadingZeros = string.Concat(Enumerable.Repeat("0", GetParameterCount() + 2));
            var fullInstruction = string.Format($"{{0:{leadingZeros}}}", code[instructionPointer]);

            SetParameterModes(fullInstruction);

            NextPointer = InstructionPointer + GetParameterCount() + 1;
        }

        /// <summary>
        /// Sets the parameter modes 
        /// </summary>
        private void SetParameterModes(string encodeInstruction)
        {
            var count = GetParameterCount();
            for (int i = count - 1; i >= 0; i--)
            {
                Modes.Add((ParameterModes)int.Parse(encodeInstruction.Substring(i, 1)));
            }
        }

        /// <summary>
        /// Returns the number of parameters based on the opcode
        /// </summary>
        /// <returns></returns>
        private int GetParameterCount()
        {
            return OpCode switch
            {
                InstructionModes.Addition => 3,
                InstructionModes.Multiplication => 3,
                InstructionModes.Input => 1,
                InstructionModes.Output => 1,
                InstructionModes.JumpIfTrue => 2,
                InstructionModes.JumpIfFalse => 2,
                InstructionModes.LessThan => 3,
                InstructionModes.Equals => 3,
                _ => 0,
            };
        }

        /// <summary>
        /// Gets the parameter depending on its mode from the code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="paremeterPosition"></param>
        /// <returns></returns>
        public int GetParameterValue(List<int> code, int paremeterPosition)
        {
            var mode = Modes[paremeterPosition];
            return mode == ParameterModes.PositionMode ? code[code[InstructionPointer + paremeterPosition + 1]] : code[InstructionPointer + paremeterPosition + 1];
        }

        /// <summary>
        /// Sets a value depending on the parameter mode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="paremeterPosition"></param>
        /// <param name="value"></param>
        public void SetValue(ref List<int> code, int paremeterPosition, int value)
        {
            var mode = Modes[paremeterPosition];
            if (mode == ParameterModes.PositionMode)
            {
                code[code[InstructionPointer + paremeterPosition + 1]] = value;
            }
            else
            {
                throw new Exception("Writting only works in position mode");
            };
        }
    }

    /// <summary>
    /// The modes a parameter can have
    /// </summary>
    public enum ParameterModes
    {
        PositionMode,
        ImmediateMode,
    }

    /// <summary>
    /// The different instruction modes
    /// </summary>
    public enum InstructionModes
    {
        NotSet,
        Addition,
        Multiplication,
        Input,
        Output,
        JumpIfTrue,
        JumpIfFalse,
        LessThan,
        Equals,
        Exit = 99,
    }
}
