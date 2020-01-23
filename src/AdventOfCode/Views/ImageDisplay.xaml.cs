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
        private List<List<TextBox>> _contents = new List<List<TextBox>>();

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

            for (int i = 0; i < rowCount; i++)
            {
                _contents.Add(new List<TextBox>());

                for (int j = 0; j < columnCount; j++)
                {
                    var content = new TextBox();
                    content.VerticalContentAlignment = VerticalAlignment.Center;
                    content.HorizontalContentAlignment = HorizontalAlignment.Center;

                    var (b, f) = GetColor(decodedImage[i][j]);

                    content.Background = b;
                    content.Foreground = f;

                    _contents[i].Add(content);

                    Grid.SetRow(content, i);
                    Grid.SetColumn(content, j);
                    ImageGrid.Children.Add(content);
                }
            }

            Content = ImageGrid;
        }

        /// <summary>
        /// Sets the text in a specific tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetText(int x, int y, string text)
        {
            _contents[y][x].Text = text;
        }

        /// <summary>
        /// Sets the color of one of the tiles
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colorCode"></param>
        public void SetColor(int x, int y, int colorCode)
        {
            var (b, f) = GetColor(colorCode);

            _contents[y][x].Background = b;
            _contents[y][x].Foreground = f;

        }

        private (Brush backGround, Brush foreGround) GetColor(int colorCode)
        {
            return colorCode switch
            {
                0 => (Brushes.Black, Brushes.White),
                1 => (Brushes.White, Brushes.Black),
                2 => (Brushes.Red, Brushes.Black),
                3 => (Brushes.Green, Brushes.Red),
                _ => (Brushes.Black, Brushes.White),
            };
        }
    }
}
