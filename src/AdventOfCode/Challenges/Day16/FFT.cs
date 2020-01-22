using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day16
{
    /// <summary>
    /// The (F)lawed (F)requency (T)ransmission algorithm is used to clean
    /// up signals send through space
    /// </summary>
    public class FFT
    {
        /// <summary>
        /// The offset to get the message after filtering
        /// </summary>
        private int _offset;

        /// <summary>
        /// The current signale
        /// </summary>
        private int[] _signal;

        /// <summary>
        /// The current signal as a string
        /// </summary>
        public string Signal => string.Join(string.Empty, _signal);

        /// <summary>
        /// Returns the message including the offset
        /// </summary>
        public string Message => string.Join(string.Empty, _signal.Skip(_offset).Take(8));


        /// <summary>
        /// Creates a new FFT for a given input signal
        /// </summary>
        /// <param name="originalSignal"></param>
        public FFT(string originalSignal)
        {
            _signal = originalSignal.ToCharArray().Select(c => int.Parse(c.ToString())).ToArray();
            _offset = int.Parse(originalSignal.Substring(0, 7));
        }

        /// <summary>
        /// Repeats the original input signal and put it together as the new input
        /// </summary>
        /// <param name="times"></param>
        public void RepeatSignal(int times)
        {
            List<int> newSignal = new List<int>();
            for (int i = 0; i< times; i++)
            {
                newSignal.AddRange(_signal);
            }
            _signal = newSignal.ToArray();
        }

        /// <summary>
        /// Runs the FFT a specific numer of times
        /// </summary>
        /// <param name="times"></param>
        public void Run(int times)
        {
            for (int i = 0; i < times; i++)
            {
                var trans = new FFTTransformation(_signal);
                trans.StartTransformation(0);
                _signal = trans.Signal;
            }
        }

        /// <summary>
        /// Runs the FFT a specific numer of times
        /// in order to receive the message
        /// </summary>
        /// <param name="times"></param>
        public void RunForMessage(int times)
        {
            for (int i = 0; i < times; i++)
            {
                var trans = new FFTTransformation(_signal);
                trans.StartTransformation(_offset);
                _signal = trans.Signal;
            }
        }
    }
}
