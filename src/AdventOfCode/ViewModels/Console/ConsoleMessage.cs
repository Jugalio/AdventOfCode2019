using Extension.Wpf.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.ViewModels.Console
{
    public class ConsoleMessage: SimpleViewModelBase, IShowOnConsole
    {
        public string Message { get; set; }

        private string _timeStamp { get; set; }
        public string TimeStamp {
            get
            {
                return Message == null ? null : $"{_timeStamp}:";
            }
            set { _timeStamp = value; }
        }


        public ConsoleMessage(string msg)
        {
            TimeStamp = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff");
            Message = msg;
        }
    }
}
