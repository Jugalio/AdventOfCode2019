using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public abstract class BaseIntCodeComputerInput : IIntCodeComputerInput
    {
        private AutoResetEvent waitHandle = new AutoResetEvent(false);

        /// <summary>
        /// Flag that shows if an input request is open
        /// </summary>
        private bool _hasOpenRequest = false;

        /// <summary>
        /// All inputs in a queue
        /// </summary>
        protected Queue<long> Inputs = new Queue<long>();

        /// <summary>
        /// Adds a new input to the queue
        /// </summary>
        /// <param name="input"></param>
        public void AddNewInput(long input)
        {
            Inputs.Enqueue(input);
            if (_hasOpenRequest)
            {
                waitHandle.Set();
            }
        }

        /// <summary>
        /// Called for additional input request behaviore
        /// </summary>
        public abstract void GetInput();

        /// <summary>
        /// Requests an input for the int code computer
        /// </summary>
        /// <returns></returns>
        public async Task<long> RequestInput()
        {
            var input = await Task.Run(() =>
            {
                if (Inputs.Count() > 0)
                {
                    return Inputs.Dequeue();
                }
                else
                {
                    _hasOpenRequest = true;

                    waitHandle.Reset();
                    GetInput();
                    waitHandle.WaitOne();

                    _hasOpenRequest = false;

                    return Inputs.Dequeue();
                }
            }).ConfigureAwait(false);

            return input;
        }

        public int NumberOfInputsInQueque()
        {
            return Inputs.Count;
        }
    }
}
