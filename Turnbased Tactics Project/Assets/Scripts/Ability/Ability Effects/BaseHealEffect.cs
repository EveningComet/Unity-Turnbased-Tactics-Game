using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    [CreateAssetMenu(fileName = "New BaseHealEffect", menuName = "Effects/BaseHealEffect")]
    public class BaseHealEffect : AbilityEffect
    {
        public int healAmount = 20;

        public override int Predict(Unit activator, GameTile targetTile)
        {
            return healAmount;
        }

        public override void TriggerEffect(Unit activator, GameTile targetTile)
        {
            if (targetTile.Units.Length > 0)
                Combat.CombatController.Heal(targetTile.Units[0], healAmount);
        }
    }
}
