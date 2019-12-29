using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.DataReader
{
    public static class InputParser
    {
        /// <summary>
        /// Gets the correct parser depending on the files extension
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IInputParser GetParser(string path, Action<string> writeToConsole)
        {
            var file = new FileInfo(path);

            switch (file.Extension)
            {
                case ".txt":
                    writeToConsole($"Use the TextfileReader for {file.Name}");
                    return new TextFileReader(path, writeToConsole);
                default:
                    throw new ArgumentException($"No parser known for the file exetnsion {file.Extension}");
            }
        }
    }
}
