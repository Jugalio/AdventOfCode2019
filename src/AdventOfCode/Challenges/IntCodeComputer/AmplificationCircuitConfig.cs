using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class AmplificationCircuitConfig
    {
        private List<int> _phaseSequence;
        private IEnumerable<long> _code;

        /// <summary>
        /// Creates an amplification circuit program
        /// </summary>
        /// <param name="code"></param>
        /// <param name="phaseSequence"></param>
        /// <param name="outFunc"></param>
        public AmplificationCircuitConfig(IEnumerable<long> code, IEnumerable<int> phaseSequence)
        {
            _phaseSequence = phaseSequence.ToList();
            _code = code;
        }

        /// <summary>
        /// Configura all amplifiers and returns the output sent to the thruster
        /// </summary>
        public long ConfigureAmplifiers(int startingInput)
        {
            long lastOutput = startingInput;

            for (int i = 0; i < _phaseSequence.Count(); i++)
            {
                var phaseSetting = _phaseSequence[i];
                var io = new IntCodeComputerIO(new List<long>() { phaseSetting, lastOutput });
                var comp = new IntCodeComputer(_code.ToList(), io, io);

                comp.Compute();
                lastOutput = io.Outputs.Last();
            }

            return lastOutput;
        }

        /// <summary>
        /// Configures the amplifiers with the addition of a feedback loop
        /// </summary>
        /// <param name="startingInput"></param>
        /// <returns></returns>
        public long ConfigureAmplifiersFeedBackLoop(int startingInput)
        {
            int lastOutput = startingInput;
            var range = Enumerable.Range(0, _phaseSequence.Count()).ToList();
            var ios = _phaseSequence.Select(phaseSetting => new IntCodeComputerIO(new List<long>() { phaseSetting })).ToList();

            range.ForEach(i =>
            {
                var thisIO = ios[i];

                //Set the input io and for the first one the initial start value
                if (i == 0)
                {
                    thisIO.AddNewInput(startingInput);
                }

                //Make the output an input for the following io
                if (i == ios.Count - 1)
                {
                    AddOutAsInput(thisIO, ios.First());
                }
                else
                {
                    AddOutAsInput(thisIO, ios[i + 1]);
                }
            });

            var intComps = range.Select(i => new IntCodeComputer(_code.ToList(), ios[i], ios[i])).ToList();
            bool loop = true;

            while (loop)
            {
                intComps.ForEach(comp => comp.Compute());
                loop = intComps.Last().State != IntCodeComputerState.Finished && intComps.Last().State != IntCodeComputerState.Failed;
            }

            return ios.Last().Outputs.Last();
        }

        private void AddOutAsInput(IntCodeComputerIO fromAmplifier, IntCodeComputerIO toAmplifier)
        {
            fromAmplifier.SentOutput += (long i) => toAmplifier.AddNewInput(i);
        }

    }
}
