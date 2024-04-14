using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// The <see cref="Ability"/> will get the tiles around itself.
    /// </summary>
    [CreateAssetMenu(fileName = "New ConstantRange", menuName = "TargetingLogic/ConstantRange")]
    public class ConstantRange : AbilityTargetingLogic
    {
        /// <summary>
        /// Whether or not the center tile (the tile of the activating unit) should be excluded.
        /// </summary>
        [Tooltip("Whether or not the center tile (the tile of the activating unit) should be excluded.")]
        public bool ignoreCenterTile = true;

        public override List<GameTile> GetTiles(Unit activator, GameTileMapData gameTileMapData)
        {
            if (ignoreCenterTile == true)
                return gameTileMapData.GetTilesWithinRange(activator.CurrentTile, this.range, true);
            else
                return gameTileMapData.GetTilesWithinRange(activator.CurrentTile, this.range, false);
        }
    }
}
