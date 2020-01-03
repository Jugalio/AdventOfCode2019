using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Challenges.Day8
{
    /// <summary>
    /// Class used to decode images encoded in space image format
    /// </summary>
    public class SpaceImageFormatDecoder
    {
        private int _width;
        private int _height;

        public List<List<List<int>>> LayeredImage = new List<List<List<int>>>();
        public List<List<int>> DecodedImage = new List<List<int>>();
        public List<int> EncodedImage;

        /// <summary>
        /// Creates a decoder for a given resolution
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public SpaceImageFormatDecoder(int width, int height, List<int> encodedImage)
        {
            _width = width;
            _height = height;
            EncodedImage = encodedImage;
        }

        /// <summary>
        /// Decodes a given image from an int List
        /// </summary>
        /// <param name="encodedImage"></param>
        public void Decode()
        {
            int index = 0;

            //First get the layered Image
            while(index < EncodedImage.Count)
            {
                AddNextLayer(index);
                index += _width * _height;
            }

            //Now decode it
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (j == 0)
                    {
                        DecodedImage.Add(new List<int>());
                    }
                    var list = DecodedImage[i];
                    list.Add(GetPositionDigit(i, j));
                }
            }
        }

        /// <summary>
        /// Get the digit for a given position
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int GetPositionDigit(int x, int y)
        {
            foreach(var layer in LayeredImage)
            {
                if (layer[x][y] != 2)
                {
                    return layer[x][y];
                }
            };
            return 2;
        }

        /// <summary>
        /// Adds the next layer to the decoded image
        /// </summary>
        /// <param name="index"></param>
        private void AddNextLayer(int index)
        {
            var nextLayer = new List<List<int>>();
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (j == 0)
                    {
                        nextLayer.Add(new List<int>());
                    }
                    var list = nextLayer[i];
                    list.Add(EncodedImage[i * _width + j + index]);
                }
            }
            LayeredImage.Add(nextLayer);
        }

    }
}
