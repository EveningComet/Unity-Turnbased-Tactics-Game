using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// For abilities that will include tiles next to the target tile, as well as the target tile.
    /// </summary>
    [CreateAssetMenu(fileName = "New SpecifyAbilityArea", menuName = "Ability AOE/SpecifyAbilityArea")]
    public class SpecifyAbilityArea : AbilityAOE
    {
        /// <summary>
        /// The range of neighbouring tiles to add.
        /// </summary>
        public int aoeRadius = 1;

        public override List<GameTile> GetTilesInArea(BattleMapController bmc, GameTile targetTile)
        {
            List<GameTile> retTiles = bmc.GetTileMapData().GetTilesWithinRange(targetTile, aoeRadius, false);

            return retTiles;
        }
    }
}
