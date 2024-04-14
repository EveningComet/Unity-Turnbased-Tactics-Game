using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// For an <see cref="Ability"/> whose success is based on something like a target's
    /// evasion stat.
    /// </summary>
    [CreateAssetMenu(fileName = "New GenericSuccessRate", menuName = "HitSuccessRates/GenericSuccessRate")]
    public class GenericSuccessRate : HitSuccessRate
    {
        public override int Calculate(GameTile targetTile)
        {
            // Get the target's evade stat
            int evasionValue = GetEvade(targetTile.Units[0]);
            return Final(evasionValue);
        }

        private int GetEvade(Unit targetUnit)
        {
            return Mathf.Clamp(targetUnit.MyStats.Evasion, 0, 100);
        }
    }
}
