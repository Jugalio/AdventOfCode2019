using AdventOfCode.Views.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    /// <summary>
    /// A int code computer which is able to
    /// translate a list of integers and execute the opt codes in it
    /// </summary>
    public class IntCodeComputer
    {
        private Action<string> _write;
        private Instruction _currentInstruction;
        private List<int> _code;
        private IReceiveInput _inputReader;

        public IntCodeComputer(List<int> code, Action<string> write, IReceiveInput inputReader)
        {
            _write = write;
            _code = code;
            _inputReader = inputReader;
        }

        /// <summary>
        /// Gets the opCode from the specified index and calls the specified
        /// method for that code. With the new code this function is called again,
        /// until the opCode 99 is hit
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<int> Compute()
        {
            var instructionPointer = _currentInstruction?.NextPointer ?? 0;
            _currentInstruction = new Instruction(instructionPointer, _code);

            switch (_currentInstruction.OpCode)
            {
                case InstructionModes.Exit:
                    return _code;
                default:
                    ExecuteOpCode();
                    return Compute();
            }
        }

        /// <summary>
        /// Exxecutes the defined function for a given opCode
        /// </summary>
        /// <returns></returns>
        public void ExecuteOpCode()
        {
            switch (_currentInstruction.OpCode)
            {
                case InstructionModes.Addition:
                    Addition();
                    break;
                case InstructionModes.Multiplication:
                    Multiplication();
                    break;
                case InstructionModes.Input:
                    Input();
                    break;
                case InstructionModes.Output:
                    Output();
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
                default:
                    throw new Exception($"OpCode {_currentInstruction.OpCode} unkonwn");
            }
        }

        /// <summary>
        /// Addition the numbers at position index + 1
        /// with the number at position index + 2
        /// and overrides the number at position index + 3
        /// </summary>
        private void Addition()
        {
            RunFunction((int a, int b) => a + b);
        }

        /// <summary>
        /// Multiplicate the numbers at position index + 1
        /// with the number at position index + 2
        /// and overrides the number at position index + 3
        /// </summary>
        private void Multiplication()
        {
            RunFunction((int a, int b) => a * b);
        }

        /// <summary>
        /// Reads an input parameter
        /// </summary>
        private void Input()
        {
            var input = _inputReader.GetInput();
            _write($"Userinput: {input}");

            var inputInteger = int.Parse(input);
            _currentInstruction.SetValue(ref _code, 0, inputInteger);
        }


        /// <summary>
        /// Outputs a parameter
        /// </summary>
        private void Output()
        {
            _write(_currentInstruction.GetParameterValue(_code, 0).ToString());
        }

        /// <summary>
        /// Resets the instruction point if true
        /// </summary>
        private void JumpIfTrue()
        {
            var num1 = _currentInstruction.GetParameterValue(_code, 0);
            if (num1 != 0)
            {
                _currentInstruction.NextPointer = _currentInstruction.GetParameterValue(_code, 1);
            }
        }

        /// <summary>
        /// Resets the instruction point if false
        /// </summary>
        private void JumpIfFalse()
        {
            var num1 = _currentInstruction.GetParameterValue(_code, 0);
            if (num1 == 0)
            {
                _currentInstruction.NextPointer = _currentInstruction.GetParameterValue(_code, 1);
            }
        }

        /// <summary>
        /// Compares two parameters and checks if the first is less than the second
        /// </summary>
        private void LessThan()
        {
            var num1 = _currentInstruction.GetParameterValue(_code, 0);
            var num2 = _currentInstruction.GetParameterValue(_code, 1);

            if (num1 < num2)
            {
                _currentInstruction.SetValue(ref _code, 2, 1);
            }
            else
            {
                _currentInstruction.SetValue(ref _code, 2, 0);
            }
        }

        /// <summary>
        /// Compares two parameters and checks if they are equal
        /// </summary>
        private void Equals()
        {
            var num1 = _currentInstruction.GetParameterValue(_code, 0);
            var num2 = _currentInstruction.GetParameterValue(_code, 1);

            if (num1 == num2)
            {
                _currentInstruction.SetValue(ref _code, 2, 1);
            }
            else
            {
                _currentInstruction.SetValue(ref _code, 2, 0);
            }
        }

        /// <summary>
        /// Runs the defined function on the code
        /// </summary>
        /// <param name="delegateFunc"></param>
        /// <returns></returns>
        private void RunFunction(Func<int, int, int> delegateFunc)
        {
            var num1 = _currentInstruction.GetParameterValue(_code, 0);
            var num2 = _currentInstruction.GetParameterValue(_code, 1);

            _currentInstruction.SetValue(ref _code, 2, delegateFunc(num1, num2));
        }

    }
}
