using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;

namespace TurnbasedGame.Tiles
{
    /// <summary>
    /// Stores data for a tile. Assumes that the tile is a pointy top hex.
    /// </summary>
    public class GameTile
    {
        // TODO: Terrain type.

        // Q + R + S = 0. Must equal zero.
        // S = -(Q + R)
        public readonly int Q; // Column
        public readonly int R; // Row
        public readonly int S;

        public static OffsetCoord[] HexDirections = {
            new OffsetCoord(1, 0),   // Right
            new OffsetCoord(1, 1),   // Top Right
            new OffsetCoord(0, 1),   // Top Left
            new OffsetCoord(-1, 0),  // Left
            new OffsetCoord(-1, -1), // Bottom Left
            new OffsetCoord(0, -1)   // Bottom Right
        };

        /// <summary>
        /// How much it costs to move into this tile.
        /// </summary>
        public int MovementCost { get; private set; }

        public Vector3 WorldSpaceCenter { get; }

        /// <summary>
        /// Stores this tile's neighbours. Not meant to be set directly.
        /// </summary>
        private GameTile[] neighbours;

        /// <summary>
        /// For convenience, used to keep track of all the units on this tile.
        /// </summary>
        private HashSet<Unit> units;
        public Unit[] Units
        {
            get { return units.ToArray(); }
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        public GameTile(int q, int r, Vector3 worldSpaceCenter, int movementCost = 1)
        {
            this.Q = q;
            this.R = r;
            this.S = -q - r;

            this.WorldSpaceCenter = worldSpaceCenter;
#if UNITY_EDITOR
            if(Q + R + S != 0)
            {
                throw new System.ArgumentException("GameTile :: Q + R + S != 0. This probably means that the map messed up during generation.");
            }
#endif
            this.MovementCost = movementCost;
            units = new HashSet<Unit>();
        }

        /// <summary>
        /// Set the neighbouring tiles for this tile. Should really only be called at map setup.
        /// </summary>
        public void SetNeighbours(GameTile[] newNeighbours)
        {
            this.neighbours = newNeighbours;
        }

        public GameTile[] GetNeighbours()
        {
            return this.neighbours;
        }

        /// <summary>
        /// Get the axial distance between two tiles.
        /// </summary>
        /// <param name="a">Tile a.</param>
        /// <param name="b">Tile b.</param>
        public static float GetAxialDistance(GameTile a, GameTile b)
        {
            var aC = TileCoordConverter.AxialToCube(a);
            var bC = TileCoordConverter.AxialToCube(b);
            return GetCubeDistance(aC, bC);
        }

        /// <summary>
        /// Get the cube distance between two cubes.
        /// </summary>
        /// <param name="a">Cube a.</param>
        /// <param name="b">Cube b.</param>
        public static float GetCubeDistance(CubeCoord a, CubeCoord b)
        {
            int dX = Mathf.Abs(a.X - b.X);
            int dY = Mathf.Abs(a.Y - b.Y);
            int dZ = Mathf.Abs(a.Z - b.Z);
            return Mathf.Max(dX, dY, dZ);
        }

        /// <summary>
        /// Get the cube distance between two tiles.
        /// </summary>
        /// <param name="a">Tile a.</param>
        /// <param name="b">Tile b.</param>
        public static float GetCubeDistance(GameTile a, GameTile b)
        {
            var aC = TileCoordConverter.OddRToCube(a);
            var bC = TileCoordConverter.OddRToCube(b);
            return GetCubeDistance(aC, bC);
        }

        /// <summary>
        /// Check if this tile has the same coordinates of the passed tile.
        /// </summary>
        public bool Equals(GameTile otherTile)
        {
            return this.Q == otherTile.Q && this.R == otherTile.R && this.S == otherTile.S;
        }

        public void AddUnit(Unit unitToAdd)
        {
            units.Add(unitToAdd);
        }

        public void RemoveUnit(Unit unitToRemove)
        {
            units.Remove(unitToRemove);
        }

        #region Overrides
        /// <summary>
        /// Returns the hex tile coordinates.
        /// </summary>
        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.Q, this.R, this.S);
        }
        #endregion
    }
}
