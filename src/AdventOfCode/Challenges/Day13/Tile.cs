using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.Day13
{
    public class Tile
    {

        public int X;
        public int Y;
        public TileId Id;

        public Tile(int x, int y, long id)
        {
            X = x;
            Y = y;
            Id = (TileId)id;
        }

    }

    public enum TileId
    {
        Empty,
        Wall,
        Block,
        HorizontalPaddle,
        Ball
    }
}
