using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Takes a <see cref="Unit"/>'s strength stat to deal damage.
    /// </summary>
    [CreateAssetMenu(fileName = "New GenericDamageEffect", menuName = "Effects/GenericDamageEffect")]
    public class GenericDamageEffect : AbilityEffect
    {
        public override int Predict(Unit activator, GameTile targetTile)
        {
            Unit targetUnit = targetTile.Units[0];
            int amount = activator.MyStats.Strength - targetUnit.MyStats.Defense;
            return amount;
        }

        public override void TriggerEffect(Unit activator, GameTile targetTile)
        {
            if (targetTile.Units.Length > 0)
            {
                Combat.CombatController.DealDamage(targetTile.Units[0], activator.MyStats.Strength);
            }
        }
    }
}
