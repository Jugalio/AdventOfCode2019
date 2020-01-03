using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class DelegateOutput : IIntCodeComputerOutput
    {
        public event IIntCodeComputerOutput.NewOutput SentOutput;

        public DelegateOutput(Action<long> output)
        {
            SentOutput += (long i) => output(i);
        }

        public void RaiseNewOutput(long i)
        {
            SentOutput?.Invoke(i);
        }
    }
}
