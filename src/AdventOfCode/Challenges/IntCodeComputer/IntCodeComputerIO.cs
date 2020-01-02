using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class IntCodeComputerIO : BaseIntCodeComputerInput, IIntCodeComputerOutput
    {
        public List<int> Outputs;

        public event IIntCodeComputerOutput.NewOutput SentOutput;

        public void RaiseNewOutput(int i)
        {
            SentOutput?.Invoke(i);
        }

        /// <summary>
        /// Creates a new IO object with a list of inputs
        /// </summary>
        /// <param name="inputs"></param>
        public IntCodeComputerIO(IEnumerable<int> inputs)
        {
            Outputs = new List<int>();
            SentOutput += (int i) => Outputs.Add(i);

            foreach (var input in inputs)
            {
                Inputs.Enqueue(input);
            }
        }

        public override void GetInput()
        {
            //Their is no extra get input behaviore in this case
            //This class juts waits for the next input added
        }
    }
}
