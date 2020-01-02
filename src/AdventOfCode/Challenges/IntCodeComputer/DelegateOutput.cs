using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class DelegateOutput : IIntCodeComputerOutput
    {
        public event IIntCodeComputerOutput.NewOutput SentOutput;

        public DelegateOutput(Action<int> output)
        {
            SentOutput += (int i) => output(i);
        }

        public void RaiseNewOutput(int i)
        {
            SentOutput?.Invoke(i);
        }
    }
}
