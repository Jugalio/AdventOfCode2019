using AdventOfCode.Views.Inputs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace AdventOfCode.Challenges.IntCodeComputer
{
    public class PopupInput : BaseIntCodeComputerInput
    {
        private string _labelText;

        public PopupInput(string labelText)
        {
            _labelText = labelText;
        }

        public override void GetInput()
        {
            var th = new Thread(() =>
            {
                var input = new Input(_labelText);
                input.ShowDialog();
                if (input.OkClicked)
                {
                    int userInput = default;
                    var isInt = int.TryParse(input.InputString, out userInput);
                    AddNewInput(isInt ? userInput : default);
                }
                else
                {
                    throw new Exception("No input provided");
                }
            });
            th.SetApartmentState(ApartmentState.STA);
            th.Start();
        }
    }
}
