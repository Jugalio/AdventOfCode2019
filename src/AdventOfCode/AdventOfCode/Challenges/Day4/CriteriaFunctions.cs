using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day4
{
    public static class CriteriaFunctions
    {
        /// <summary>
        /// Test if the string contains two adjacent chars which are the same
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool AdjacentEqual(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Test if the string contains two adjacent chars which are the same and not part of a larger group
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool AdjacentEqualNoGroup(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                var a = i == 0 ? -1 : input[i -1];
                var b = input[i];
                var c = input[i + 1];
                var d = i == input.Length - 2 ? -1 : input[i + 2];

                if (b == c && a != b && c != d)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Test if the string is a list of digits, that never decrease
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool NeverDecreasing(string input)
        {
            var intList = input.Select(c => c - 0);
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i + 1] - input[i] < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
