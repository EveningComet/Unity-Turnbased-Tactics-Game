using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// For <see cref="Ability"/> objects that only target the user.
    /// </summary>
    [CreateAssetMenu(fileName = "SelfAbilityRange", menuName = "TargetingLogic/SelfAbilityRange")]
    public class SelfAbilityRange : AbilityTargetingLogic
    {
        public override List<GameTile> GetTiles(Unit activator, GameTileMapData gameTileMapData)
        {
            List<GameTile> retTiles = new List<GameTile>();
            retTiles.Add(activator.CurrentTile);
            return retTiles;
        }
    }
}
