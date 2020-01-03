using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AdventOfCode.Views
{
    /// <summary>
    /// Interaction logic for ImageDisplay.xaml
    /// </summary>
    public partial class ImageDisplay : Window
    {
        public ImageDisplay(List<List<int>> decodedImage)
        {
            InitializeComponent();

            var ImageGrid = new Grid();

            var rowCount = decodedImage.Count;
            var columnCount = decodedImage[0].Count;

            for (int i = 0; i < rowCount; i++)
            {
                var row = new RowDefinition();
                ImageGrid.RowDefinitions.Add(row);
            }

            for (int i = 0; i < columnCount; i++)
            {
                var column = new ColumnDefinition();
                ImageGrid.ColumnDefinitions.Add(column);
            }

            for(int i = 0; i< rowCount; i++)
            {
                for (int j = 0; j< columnCount; j++)
                {
                    var content = new Grid();

                    if (decodedImage[i][j] == 0)
                    {
                        content.Background = Brushes.Black;
                    }

                    Grid.SetRow(content, i);
                    Grid.SetColumn(content, j);
                    ImageGrid.Children.Add(content);
                }
            }

            Content = ImageGrid;
        }
    }
}
