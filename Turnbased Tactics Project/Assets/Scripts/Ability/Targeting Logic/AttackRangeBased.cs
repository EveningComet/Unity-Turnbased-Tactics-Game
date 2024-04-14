using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Get the target's based on the <see cref="Unit"/>'s attack range.
    /// </summary>
    [CreateAssetMenu(fileName = "New AttackRangeBased", menuName = "TargetingLogic/AttackRangeBased")]
    public class AttackRangeBased : AbilityTargetingLogic
    {
        public override List<GameTile> GetTiles(Unit activator, GameTileMapData gameTileMapData)
        {
            return gameTileMapData.GetTilesWithinRange(activator.CurrentTile, activator.MyStats.AttackRange, true);
        }
    }
}
