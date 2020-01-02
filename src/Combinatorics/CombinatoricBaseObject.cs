using System;
using System.Collections.Generic;
using System.Text;

namespace Combinatorics
{
    public abstract class CombinatoricBaseObject<T> : ICombinatoricObject<T>
    {
        protected IEnumerable<T> _selectFrom;

        public CombinatoricBaseObject(IEnumerable<T> selectFrom)
        {
            _selectFrom = selectFrom;
        }

        public abstract int GetCombinationCount(int k);

        public abstract IEnumerable<IEnumerable<T>> GetCombinations(int k);

        public int Faculty(int n)
        {
            int faculty = 1;
            for (int i = 2; i <= n; i++)
            {
                faculty *= i;
            }
            return faculty;
        }
    }
}
