using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using TurnbasedGame.Units;

namespace TurnbasedGame.Levels
{
    /// <summary>
    /// Stores game data for levels/maps.
    /// </summary>
    public class SpawnableData
    {
        public int TileX { get; private set; }
        public int TileY { get; private set; }

        /// <summary>
        /// Something like if a specified unit is the target for a mission/level/map.
        /// Does nothing if the owning <see cref="LevelData"/> does not have the relevant
        /// mission type.
        /// </summary>
        public bool IsTarget { get; private set; }
        public CharacterStats CharacterStats { get; private set; }

        [JsonConstructor]
        public SpawnableData(int tileX, int tileY, bool isTarget, CharacterStats characterStats)
        {
            this.TileX = tileX;
            this.TileY = tileY;
            this.IsTarget = isTarget;
            this.CharacterStats = characterStats;
        }

        /// <summary>
        /// Constructor for when just loading a position.
        /// </summary>
        /// <param name="tileX">X coord of tile.</param>
        /// <param name="tileY">Y coord of tile.</param>
        public SpawnableData(int tileX, int tileY)
        {
            this.TileX = tileX;
            this.TileY = tileY;
        }
    }
}
