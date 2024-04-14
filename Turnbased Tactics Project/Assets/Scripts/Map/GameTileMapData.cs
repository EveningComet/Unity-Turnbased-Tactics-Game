using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.Tiles
{
    /// <summary>
    /// Stores <see cref="GameTile"/> map data. Assumes they're hexes.
    /// </summary>
    public class GameTileMapData
    {
        /* TODO: With how we're handling things at the moment and how Unity's Tilemap is handling the 
         * generated tiles, we could end up getting null tiles. We might have to do numRows -1 and/or 
         * numColumns -1 when initializing, to avoid null tiles that should not even exist in the map to begin with. */
        private int numRows;    // x
        private int numColumns; // y

        /* NOTE: Because the tiles used in this game are hexes, it might be better to store them in a hashset for
         * less rectangular maps.*/
        private GameTile[,] tileData;

        public GameTileMapData(int rows, int columns)
        {
            this.numRows = rows;
            this.numColumns = columns;
            tileData = new GameTile[numRows, numColumns];
        }

        /// <summary>
        /// Set the tile at the passed coordinates.
        /// </summary>
        public void SetTileData(int row, int column, GameTile tileToSet)
        {
            tileData[row, column] = tileToSet;
        }

        /// <summary>
        /// Attempts to get a <see cref="GameTile"/> at the passed position.
        /// </summary>
        public GameTile GetTile(int row, int column)
        {
            try
            {
                return tileData[row, column];
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get tiles within range of the passed center tile.
        /// </summary>
        /// <param name="centerTile">Center tile.</param>
        /// <param name="range">The range of tiles around the center tile that will be gotten.</param>
        /// <param name="ignoreCenterTile">Whether or not the center tile should be excluded.</param>
        public List<GameTile> GetTilesWithinRange(GameTile centerTile, int range, bool ignoreCenterTile)
        {
            List<GameTile> results = new List<GameTile>();

            // Convert the passed center tile to a cube
            CubeCoord centerAsCube = TileCoordConverter.OddRToCube(centerTile);

            for (int dX = -range; dX <= range; dX++)
            {
                for (int dY = Mathf.Max(-range, -dX - range); dY <= Mathf.Min(range, -dX + range); dY++)
                {
                    // Convert the tile to cube coordinates
                    CubeCoord cube = CubeCoord.Add(centerAsCube, new CubeCoord(dX, dY));

                    // Get the offset coordinates so that we can find the tile
                    var axialCoord = TileCoordConverter.CubeToOddR(cube);

                    results.Add( GetTile(axialCoord.Column, axialCoord.Row) );
                }
            }

            // Remove the center tile, if we have to
            if (ignoreCenterTile == true)
                results.Remove(centerTile);

            // Ignore null tiles
            List<GameTile> returnTiles = new List<GameTile>();
            foreach (GameTile gT in results)
            {
                if (gT != null)
                {
                    returnTiles.Add(gT);
                }
            }

            return returnTiles;
        }
        
        /// <summary>
        /// Get the tiles in range that a unit can move to.
        /// </summary>
        public List<GameTile> GetTilesBasedOnUnitStatsFloodFil(GameTile centerTile, TurnbasedGame.Units.Unit unit)
        {
            // TODO
            return null;
        }

        /// <summary>
        /// Generate neighbours for the passed tile. Should only really be called at map setup.
        /// </summary>
        /// <param name="theTile">The tile whose neigbours will be generated.</param>
        public void GenerateNeighboursFor(GameTile theTile)
        {
            List<GameTile> neighbours = new List<GameTile>();
            
            // Convert the Tile to CubeCoord
            var tileAsCube = TileCoordConverter.OddRToCube(theTile);

            for(int i = 0; i < 6; i++)
            {
                // Get the cube direction
                CubeCoord dir = CubeCoord.CubeDirections[i];
                
                // Add the cubes
                CubeCoord finalCube = CubeCoord.Add(tileAsCube, dir);
                var axialCoord = TileCoordConverter.CubeToOddR(finalCube);

                // Finally get the tile
                neighbours.Add( GetTile(axialCoord.Column, axialCoord.Row) );
            }

            // Let's ignore the null tiles
            List<GameTile> neighbours2 = new List<GameTile>();
            foreach(GameTile t in neighbours)
            {
                if(t != null)
                {
                    neighbours2.Add(t);
                }
            }

            // Set the neighbours
            theTile.SetNeighbours( neighbours2.ToArray() );
        }
    }
}