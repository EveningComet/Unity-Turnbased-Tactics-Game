using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    [CreateAssetMenu(fileName = "New BaseDamageEffect", menuName = "Effects/BaseDamageEffect")]
    public class BaseDamageEffect : AbilityEffect
    {
        public int damage = 20;

        /// <summary>
        /// Predict how much damage will be done.
        /// </summary>
        public override int Predict(Unit activator, GameTile targetTile)
        {
            Unit targetUnit = targetTile.Units[0];
            int amount = damage - targetUnit.MyStats.Defense;
            return amount;
        }

        public override void TriggerEffect(Unit activator, GameTile targetTile)
        {
            if (targetTile.Units.Length > 0)
            {
                Combat.CombatController.DealDamage(targetTile.Units[0], damage);
            }
        }
    }
}
