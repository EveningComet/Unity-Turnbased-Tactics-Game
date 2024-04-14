using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.Tiles
{
    /// <summary>
    /// Contains functions for converting between the different Tile coordinates.
    /// </summary>
    public static class TileCoordConverter
    {
        public static OffsetCoord CubeToAxial(CubeCoord cube)
        {
            int q = cube.X;
            int r = cube.Z;
            return new OffsetCoord(q, r);
        }

        public static CubeCoord AxialToCube(GameTile t)
        {
            int x = t.Q;
            int z = t.R;
            int y = -x - z;
            return new CubeCoord(x, y, z);
        }

        /// <summary>
        /// Convert the passed <see cref="GameTile"/> to cube coordinates.
        /// </summary>
        public static CubeCoord OddRToCube(GameTile t)
        {
            CubeCoord cube = CubeCoord.Zero;
            cube.X = t.Q - ((t.R - (t.R & 1)) / 2);
            cube.Z = t.R;
            cube.Y = -cube.X - cube.Z;
            return cube;
        }

        /// <summary>
        /// Convert the passed cube coordinates to offset coordinates.
        /// </summary>
        public static OffsetCoord CubeToOddR(CubeCoord cube)
        {
            int x = cube.X + ((cube.Z - (cube.Z & 1)) / 2);
            int y = cube.Z;
            return new OffsetCoord(x, y);
        }
    }
}
