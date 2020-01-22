using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace AdventOfCode.Challenges.Day16
{
    /// <summary>
    /// Used to do one transformation phase
    /// </summary>
    public class FFTTransformation
    {
        private int _length;
        private int _middle;
        private int _third;
        public int[] _ref;
        public int[] Signal;

        public FFTTransformation(int[] signal)
        {
            Signal = signal;
            
            _length = Signal.Length;
            _middle = _length / 2;
            _third = (_length + 1) / 3;

            _ref = new int[_length];
            Array.Copy(Signal, _ref, _length);
        }

        /// <summary>
        /// Starts the transformation from the point of the message position
        /// as everything which is located before this point is irrelevant
        /// </summary>
        /// <param name="messagePosition"></param>
        public void StartTransformation(int messagePosition)
        {
            TransformLastHalf(messagePosition);
            TransFromFirstThird(messagePosition);
            TransformRest(messagePosition);

            Signal = Signal.Select(k => GetLasteDigit(k)).ToArray();
        }

        /// <summary>
        /// An easy transformation in the last half of the signal
        /// since it only adds up numbers
        /// </summary>
        public void TransformLastHalf(int messagePosition)
        {
            var until = Math.Max(_middle, messagePosition);

            //We do not need to transform the last digit as it stays the same
            for (int k = _length - 2; k >= until; k--)
            {
                Signal[k] = Signal[k] + Signal[k + 1];
            }
        }

        /// <summary>
        /// Also not as hard since the transformation is just adding and subtracting from the back of the signal since 
        /// zeros come in from the end in 1,3,5,7,...
        /// </summary>
        public void TransFromFirstThird(int messagePosition)
        {
            if (_middle - 1 > _third)
            {
                var until = Math.Max(_third, messagePosition);
                var span = _ref.AsSpan();
                Signal[_middle - 1] = Signal[_middle - 1] + Signal[_middle] - span.ToArray().Last();

                var j = 3;
                for (int i = _middle - 2; i > until; i--)
                {
                    var reduce = span.Slice(_length - j, 2).ToArray();
                    Signal[i] = Signal[i] + Signal[i + 1] - reduce.Sum();
                    j += 2;
                }
            }

        }

        /// <summary>
        /// The rest is pretty hard work as it is just summing and subtract all digits
        /// </summary>
        private void TransformRest(int messagePosition)
        {
            var until = Math.Max(0, messagePosition);
            var span = _ref.AsSpan();

            //The first quarter now gets a little bit more complicated
            for (int i = _third; i >= until; i--)
            {
                var relevant = span.Slice(i).ToArray();
                var stackLength = i + 1;

                var positiv = GetPositivNumbers(relevant, stackLength);
                var negativ = GetNegativeNumbers(relevant, stackLength);

                Signal[i] = positiv.Sum() - negativ.Sum();
            }
        }

        /// <summary>
        /// Returns the las digit
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private int GetLasteDigit(int input)
        {
            return Math.Abs(input % 10);
        }

        /// <summary>
        /// Returns the set of numbers that are added up
        /// </summary>
        private IEnumerable<int> GetPositivNumbers(int[] relevant, int stackLength)
        {
            for (int i = 0; i < relevant.Length; i += stackLength * 4)
            {
                var max = Math.Min(stackLength, relevant.Length - i);
                for (int j = 0; j < max; j++)
                {
                    yield return relevant[i + j];
                }
            }
        }

        /// <summary>
        /// Returns the set of numbers that are subtracted up
        /// </summary>
        private IEnumerable<int> GetNegativeNumbers(int[] relevant, int stackLength)
        {
            for (int i = stackLength * 2; i < relevant.Length; i += stackLength * 4)
            {
                var max = Math.Min(stackLength, relevant.Length - i);
                for (int j = 0; j < max; j++)
                {
                    yield return relevant[i + j];
                }
            }
        }

    }
}
