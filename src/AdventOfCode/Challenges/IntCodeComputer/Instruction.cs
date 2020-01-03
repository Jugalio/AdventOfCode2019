using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class Instruction
    {
        public int RelativeBase = 0;
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
        public Instruction(int instructionPointer, List<long> code)
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
                InstructionModes.RelativeBaseOffset => 1,
                _ => 0,
            };
        }

        /// <summary>
        /// Gets the parameter depending on its mode from the code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parameterPosition"></param>
        /// <returns></returns>
        public long GetParameterValue(ref List<long> code, int parameterPosition)
        {
            var adress = GetAdress(ref code, parameterPosition);
            return code[adress];
        }

        /// <summary>
        /// Sets a value depending on the parameter mode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parameterPosition"></param>
        /// <param name="value"></param>
        public void SetValue(ref List<long> code, int parameterPosition, long value)
        {
            var adress = GetAdress(ref code, parameterPosition);
            code[adress] = value;
        }

        /// <summary>
        /// Get the adress by parameter mode
        /// </summary>
        /// <param name="code"></param>
        /// <param name="parameterPosition"></param>
        /// <returns></returns>
        private int GetAdress(ref List<long> code, int parameterPosition)
        {
            var adress = Modes[parameterPosition] switch
            {
                ParameterModes.PositionMode => (int)code[CheckMemoryAdress(InstructionPointer + parameterPosition + 1, ref code)],
                ParameterModes.ImmediateMode => InstructionPointer + parameterPosition + 1,
                ParameterModes.RelativeMode => (int)code[CheckMemoryAdress(InstructionPointer + parameterPosition + 1, ref code)] + RelativeBase,
                _ => throw new ArgumentException("Parameter mode unknown"),
            };

            return CheckMemoryAdress(adress, ref code);
        }

        /// <summary>
        /// Checks the memory adress and increases the memory if necessary
        /// </summary>
        /// <param name="adress"></param>
        /// <returns></returns>
        private int CheckMemoryAdress(int adress, ref List<long> code)
        {
            if (code.Count <= adress)
            {
                var increase = Enumerable.Range(0, adress - code.Count + 1).Select(m => (long)0);
                code.AddRange(increase);
            }
            return adress;
        }
    }

    /// <summary>
    /// The modes a parameter can have
    /// </summary>
    public enum ParameterModes
    {
        PositionMode,
        ImmediateMode,
        RelativeMode,
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
        RelativeBaseOffset,
        Exit = 99,
    }
}
