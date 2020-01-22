using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class IntCodeComputerIO : BaseIntCodeComputerInput, IIntCodeComputerOutput
    {
        private Func<long> _getNextInput;

        public List<long> Outputs;

        public event IIntCodeComputerOutput.NewOutput SentOutput;

        public void RaiseNewOutput(long i)
        {
            SentOutput?.Invoke(i);
        }

        /// <summary>
        /// Creates a new IO object with an action for requesting inputs
        /// </summary>
        /// <param name="inputs"></param>
        public IntCodeComputerIO(Func<long> getNextInput)
        {
            Outputs = new List<long>();
            SentOutput += (long i) => Outputs.Add(i);
            _getNextInput = getNextInput;
        }

        /// <summary>
        /// Creates a new IO object with a list of inputs
        /// </summary>
        /// <param name="inputs"></param>
        public IntCodeComputerIO(IEnumerable<long> inputs)
        {
            Outputs = new List<long>();
            SentOutput += (long i) => Outputs.Add(i);

            foreach (var input in inputs)
            {
                Inputs.Enqueue(input);
            }
        }

        /// <summary>
        /// Gets the next input and adds it to the list of inputs
        /// </summary>
        public override void GetInput()
        {
            if (_getNextInput != null)
            {
                var next = _getNextInput();
                AddNewInput(next);
            }
            else
            {
                //Do nothing
            }
        }
    }
}
