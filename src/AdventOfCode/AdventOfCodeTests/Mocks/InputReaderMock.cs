using AdventOfCode.Challenges.IntCodeComputer;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCodeTests.Mocks
{
    public class InputReaderMock : IReceiveInput
    {
        public string returnValue;

        public string GetInput()
        {
            return returnValue;
        }
    }
}
