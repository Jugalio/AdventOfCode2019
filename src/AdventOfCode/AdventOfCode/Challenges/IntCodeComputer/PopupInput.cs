using AdventOfCode.Views.Inputs;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class PopupInput : IReceiveInput
    {
        public string GetInput()
        {
            var input = new Input();
            input.ShowDialog();
            return input.OkClicked ? input.InputString : string.Empty;
        }
    }
}
