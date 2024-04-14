using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Only targets the passed tile. An example usage of this would be an ability 
    /// that functions like a generic attack.
    /// </summary>
    [CreateAssetMenu(fileName = "SingleAbilityArea", menuName = "Ability AOE/SingleAbilityArea")]
    public class SingleAbilityArea : AbilityAOE
    {
        public override List<GameTile> GetTilesInArea(BattleMapController bmc, GameTile targetTile)
        {
            List<GameTile> retTiles = new List<GameTile>();
            retTiles.Add(targetTile);
            return retTiles;
        }
    }
}
