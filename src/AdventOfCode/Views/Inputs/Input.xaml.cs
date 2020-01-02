using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdventOfCode.Views.Inputs
{
    /// <summary>
    /// Interaction logic for InputSelection.xaml
    /// </summary>
    public partial class Input : Window
    {
        public bool OkClicked = false;
        public string InputString;

        public Input()
        {
            InitializeComponent();
            txtBox.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OkClicked = true;
            InputString = txtBox.Text;
            Close();
        }
    }
}
