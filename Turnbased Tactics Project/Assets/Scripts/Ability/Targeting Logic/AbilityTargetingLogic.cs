using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Describes how an <see cref="Ability"/> gets its targets.
    /// </summary>
    public abstract class AbilityTargetingLogic : ScriptableObject
    {
        /// <summary>
        /// The number of tiles away from the activating unit that can be reached.
        /// </summary>
        [Tooltip("The number of tiles away from the activating unit that can be reached.")]
        public int range = 1;

        /// <summary>
        /// Gets the tiles for the <see cref="Ability"/>.
        /// </summary>
        /// <param name="activator">The activator of the ability. Not all <see cref="AbilityTargetingLogic"/> objects will need the activator,
        /// but let's make things easy for us and pass it anyway.</param>
        /// <param name="gameTileMapData">The thing storing the tile data.</param>
        public abstract List<GameTile> GetTiles(Unit activator, GameTileMapData gameTileMapData);
    }
}
