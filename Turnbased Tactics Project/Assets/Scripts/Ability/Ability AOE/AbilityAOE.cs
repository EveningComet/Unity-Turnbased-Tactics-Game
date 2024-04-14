using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Specifies what, if any, tiles around a target tile will be targeted.
    /// </summary>
    public abstract class AbilityAOE : ScriptableObject
    {
        public abstract List<GameTile> GetTilesInArea(BattleMapController bmc, GameTile targetTile);
    }
}
