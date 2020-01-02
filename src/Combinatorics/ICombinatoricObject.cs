using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics
{
    public interface ICombinatoricObject<T>
    {
        /// <summary>
        /// Returns the number of possible combinations if you select length objects
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        int GetCombinationCount(int k);

        /// <summary>
        /// Get all combinations of length k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        IEnumerable<IEnumerable<T>> GetCombinations(int k);
    }
}
