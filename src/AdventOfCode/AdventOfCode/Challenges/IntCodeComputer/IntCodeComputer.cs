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
        private Action<string> _writeToConsole;

        public IntCodeComputer(Action<string> writeToConsole)
        {
            _writeToConsole = writeToConsole;
        }

        /// <summary>
        /// Gets the opCode from the specified index and calls the specified
        /// method for that code. With the new code this function is called again,
        /// until the opCode 99 is hit
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<int> HandleOpCode(int instructionPointer, List<int> code)
        {
            var instruction = code[instructionPointer];

            switch (instruction)
            {
                case 1:
                    return HandleOpCode(instructionPointer + 4, Addition(instructionPointer, code));
                case 2:
                    return HandleOpCode(instructionPointer + 4, Multiplication(instructionPointer, code));
                case 99:
                    return code;
                default:
                    return new List<int>() { -1 };
            }
        }

        /// <summary>
        /// Addition the numbers at position index + 1
        /// with the number at position index + 2
        /// and overrides the number at position index + 3
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<int> Addition(int instructionPointer, List<int> code)
        {
            return RunFunction(instructionPointer, code, (int a, int b) => a + b);
        }

        /// <summary>
        /// Multiplicate the numbers at position index + 1
        /// with the number at position index + 2
        /// and overrides the number at position index + 3
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private List<int> Multiplication(int instructionPointer, List<int> code)
        {
            return RunFunction(instructionPointer, code, (int a, int b) => a * b);
        }

        /// <summary>
        /// Runs the defined function on the code
        /// </summary>
        /// <param name="instructionPointer"></param>
        /// <param name="code"></param>
        /// <param name="delegateFunc"></param>
        /// <returns></returns>
        private List<int> RunFunction(int instructionPointer, List<int> code, Func<int, int, int> delegateFunc)
        {
            var num1 = code[code[instructionPointer + 1]];
            var num2 = code[code[instructionPointer + 2]];
            code[code[instructionPointer + 3]] = delegateFunc(num1, num2);
            return code;
        }

    }
}
