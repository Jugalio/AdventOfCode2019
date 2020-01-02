using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Combinatorics
{
    /// <summary>
    /// Combinatorics class for ordered combinations with repetition allowed
    /// </summary>
    public class OrderedRepeat<T>: CombinatoricBaseObject<T>
    {
        /// <summary>
        /// Creates a combinatoric object
        /// </summary>
        /// <param name="selectFrom"></param>
        public OrderedRepeat(IEnumerable<T> selectFrom) : base(selectFrom) { }

        /// <summary>
        /// Returns the number of possible combinations if you select length objects
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public override int GetCombinationCount(int k)
        {
            return (int)Math.Pow(_selectFrom.Count(), k);
        }

        /// <summary>
        /// Get all combinations of length k
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public override IEnumerable<IEnumerable<T>> GetCombinations(int k)
        {
            IEnumerable<IEnumerable<T>> combs = new List<List<T>>() { new List<T>() };

            for (int i = 0; i < k; i++)
            {
                combs = AddNextObject(combs);
            }

            return combs;
        }

        /// <summary>
        /// Adds the next object to all combinations
        /// </summary>
        /// <param name="combinations"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<T>> AddNextObject(IEnumerable<IEnumerable<T>> combinations)
        {
            return combinations.SelectMany(c => _selectFrom.Select(nextObject => 
            {
                var newComb = c.ToList();
                newComb.Add(nextObject);
                return newComb;
            }));
        }

    }
}
