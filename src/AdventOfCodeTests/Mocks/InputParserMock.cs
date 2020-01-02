using AdventOfCode.DataReader;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeTests.Mocks
{
    public class InputParserMock : IInputParser
    {
        public IEnumerable<string> GetInputData()
        {
            return new List<string>();
        }

        public IEnumerable<int> GetIntCode()
        {
            return new List<int>();
        }
    }
}
