using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Combinatorics
{
    public class OrderedNoRepeat<T> : CombinatoricBaseObject<T>
    {
        /// <summary>
        /// Creates a combinatoric object
        /// </summary>
        /// <param name="selectFrom"></param>
        public OrderedNoRepeat(IEnumerable<T> selectFrom) : base(selectFrom) { }

        public override int GetCombinationCount(int k)
        {
            var n = _selectFrom.Count();
            return (int)Faculty(n) / Faculty(n - k);
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
            return combinations.SelectMany(c => _selectFrom.Where(o => !c.Contains(o)).Select(nextObject =>
            {
                var newComb = c.ToList();
                newComb.Add(nextObject);
                return newComb;
            }));
        }
    }
}
