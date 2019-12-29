using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.DataReader
{
    public class TextFileReader : IInputParser
    {
        public string TextFilePath;
        private Action<string> _writeToConsole;

        public TextFileReader(string filePath, Action<string> writeToConsole)
        {
            TextFilePath = filePath;
            _writeToConsole = writeToConsole;
        }

        /// <summary>
        /// Reads the textfile
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetInputData()
        {
            var lines = new List<string>();

            using (StreamReader sw = new StreamReader(TextFilePath))
            {
                while (!sw.EndOfStream)
                {
                    lines.Add(sw.ReadLine());
                }
            }

            _writeToConsole("Done reading input file");
            return lines;
        }

        /// <summary>
        /// Reads the textfile and parses the int code included in it
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetIntCode()
        {
            var lines = GetInputData();
            var code = lines.SelectMany(line => line.Split(',')).Select(intValue => int.Parse(intValue));
            return code;
        }
    }
}
