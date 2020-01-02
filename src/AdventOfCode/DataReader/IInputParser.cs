using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataReader
{
    public interface IInputParser
    {
        /// <summary>
        /// Reads an input daocument line seperated
        /// </summary>
        /// <returns>The lines as string in an enumaration</returns>
        IEnumerable<string> GetInputData();

        /// <summary>
        /// Reads the textfile and parses the int code included in it
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetIntCode();
    }
}
