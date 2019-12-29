using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.ViewModels
{
    public class ConsoleMessage
    {
        public string Message { get; set; }
        public string TimeStamp { get; set; }

        public ConsoleMessage(string msg)
        {
            TimeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");
            Message = msg;
        }

        public override string ToString()
        {
            if (TimeStamp == null || Message == null)
            {
                return string.Empty;
            }
            else
            {
                return $"{TimeStamp}:   {Message}";
            }
        }
    }
}
