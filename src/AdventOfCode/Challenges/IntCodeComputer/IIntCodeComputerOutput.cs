using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public interface IIntCodeComputerOutput
    {
        /// <summary>
        /// Event fired, if a new output is sent
        /// </summary>
        event NewOutput SentOutput;

        /// <summary>
        /// Delegate for new outputs
        /// </summary>
        /// <param name="s"></param>
        delegate void NewOutput(int s);

        /// <summary>
        /// Sents a new output event
        /// </summary>
        /// <param name="i"></param>
        void RaiseNewOutput(int i);
    }
}
