using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public interface IIntCodeComputerInput
    {
        /// <summary>
        /// Call to request an input
        /// </summary>
        /// <returns></returns>
        Task<long> RequestInput();

        /// <summary>
        /// Adds a new input
        /// </summary>
        /// <param name="input"></param>
        void AddNewInput(long input);
    }
}
