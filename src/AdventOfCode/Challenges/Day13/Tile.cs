using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Challenges.Day13
{
    public class Tile
    {

        public long X;
        public long Y;
        public TileId Id;

        public Tile(long x, long y, long id)
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
