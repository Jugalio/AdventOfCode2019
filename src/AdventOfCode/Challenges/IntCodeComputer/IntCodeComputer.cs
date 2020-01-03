using AdventOfCode.Views.Inputs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    /// <summary>
    /// A int code computer which is able to
    /// translate a list of integers and execute the opt codes in it
    /// </summary>
    public class IntCodeComputer
    {
        private Instruction _currentInstruction;
        public List<long> Code;
        public bool ContinueAfterOutput = false;
        public IntCodeComputerState State = IntCodeComputerState.Idle;
        public IIntCodeComputerInput InputReader;
        public IIntCodeComputerOutput OutWriter;

        public IntCodeComputer(List<long> code, IIntCodeComputerInput inputReader, IIntCodeComputerOutput outWriter)
        {
            OutWriter = outWriter;
            Code = code;
            InputReader = inputReader;
        }

        /// <summary>
        /// Gets the opCode from the specified index and calls the specified
        /// method for that code. With the new code this function is called again,
        /// until the opCode 99 is hit
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public void Compute()
        {
            try
            {
                State = IntCodeComputerState.Running;
                while (State == IntCodeComputerState.Running)
                {
                    ExecuteOpCode();
                }
            }
            catch (Exception e)
            {
                //Exception during the execution of the int code computer
                State = IntCodeComputerState.Failed;
            }
        }

        /// <summary>
        /// Exxecutes the defined function for a given opCode
        /// </summary>
        /// <returns></returns>
        private void ExecuteOpCode()
        {
            UpdateInstructionPointer();

            switch (_currentInstruction.OpCode)
            {
                case InstructionModes.Addition:
                    Addition();
                    break;
                case InstructionModes.Multiplication:
                    Multiplication();
                    break;
                case InstructionModes.Input:
                    Input().Wait();
                    break;
                case InstructionModes.Output:
                    Output();
                    if (!ContinueAfterOutput)
                    {
                        State = IntCodeComputerState.ProducedOutput;
                    }
                    break;
                case InstructionModes.JumpIfTrue:
                    JumpIfTrue();
                    break;
                case InstructionModes.JumpIfFalse:
                    JumpIfFalse();
                    break;
                case InstructionModes.LessThan:
                    LessThan();
                    break;
                case InstructionModes.Equals:
                    Equals();
                    break;
                case InstructionModes.RelativeBaseOffset:
                    RelativeBaseOffset();
                    break;
                case InstructionModes.Exit:
                    State = IntCodeComputerState.Finished;
                    break;
                default:
                    throw new Exception($"OpCode {_currentInstruction.OpCode} unkonwn");
            }
        }

        /// <summary>
        /// Updates the instruction pointer
        /// </summary>
        private void UpdateInstructionPointer()
        {
            var instructionPointer = _currentInstruction?.NextPointer ?? 0;
            var relativeBase = _currentInstruction?.RelativeBase ?? 0;

            _currentInstruction = new Instruction(instructionPointer, Code);
            _currentInstruction.RelativeBase = relativeBase;
        }

        /// <summary>
        /// Addition the numbers at position index + 1
        /// with the number at position index + 2
        /// and overrides the number at position index + 3
        /// </summary>
        private void Addition()
        {
            RunFunction((long a, long b) => a + b);
        }

        /// <summary>
        /// Multiplicate the numbers at position index + 1
        /// with the number at position index + 2
        /// and overrides the number at position index + 3
        /// </summary>
        private void Multiplication()
        {
            RunFunction((long a, long b) => a * b);
        }

        /// <summary>
        /// Reads an input parameter
        /// </summary>
        private async Task Input()
        {
            State = IntCodeComputerState.WaitingForInput;

            var input = await InputReader.RequestInput().ConfigureAwait(false);

            State = IntCodeComputerState.Running;

            _currentInstruction.SetValue(ref Code, 0, input);
        }
        /// <summary>
        /// Outputs a parameter
        /// </summary>
        private void Output()
        {
            OutWriter.RaiseNewOutput(_currentInstruction.GetParameterValue(ref Code, 0));
        }

        /// <summary>
        /// Resets the instruction point if true
        /// </summary>
        private void JumpIfTrue()
        {
            var num1 = _currentInstruction.GetParameterValue(ref Code, 0);
            if (num1 != 0)
            {
                _currentInstruction.NextPointer = (int)_currentInstruction.GetParameterValue(ref Code, 1);
            }
        }

        /// <summary>
        /// Resets the instruction point if false
        /// </summary>
        private void JumpIfFalse()
        {
            var num1 = _currentInstruction.GetParameterValue(ref Code, 0);
            if (num1 == 0)
            {
                _currentInstruction.NextPointer = (int)_currentInstruction.GetParameterValue(ref Code, 1);
            }
        }

        /// <summary>
        /// Compares two parameters and checks if the first is less than the second
        /// </summary>
        private void LessThan()
        {
            var num1 = _currentInstruction.GetParameterValue(ref Code, 0);
            var num2 = _currentInstruction.GetParameterValue(ref Code, 1);

            if (num1 < num2)
            {
                _currentInstruction.SetValue(ref Code, 2, 1);
            }
            else
            {
                _currentInstruction.SetValue(ref Code, 2, 0);
            }
        }

        /// <summary>
        /// Compares two parameters and checks if they are equal
        /// </summary>
        private void Equals()
        {
            var num1 = _currentInstruction.GetParameterValue(ref Code, 0);
            var num2 = _currentInstruction.GetParameterValue(ref Code, 1);

            if (num1 == num2)
            {
                _currentInstruction.SetValue(ref Code, 2, 1);
            }
            else
            {
                _currentInstruction.SetValue(ref Code, 2, 0);
            }
        }

        /// <summary>
        /// Offsets the relative base of the int code computer
        /// </summary>
        private void RelativeBaseOffset()
        {
            var num1 = (int)_currentInstruction.GetParameterValue(ref Code, 0);
            _currentInstruction.RelativeBase += num1;
        }

        /// <summary>
        /// Runs the defined function on the code
        /// </summary>
        /// <param name="delegateFunc"></param>
        /// <returns></returns>
        private void RunFunction(Func<long, long, long> delegateFunc)
        {
            var num1 = _currentInstruction.GetParameterValue(ref Code, 0);
            var num2 = _currentInstruction.GetParameterValue(ref Code, 1);

            _currentInstruction.SetValue(ref Code, 2, delegateFunc(num1, num2));
        }
    }

    /// <summary>
    /// The different state the int code computer can have
    /// </summary>
    public enum IntCodeComputerState
    {
        Idle,
        Running,
        Finished,
        Failed,
        WaitingForInput,
        ProducedOutput
    }
}
