using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Combat
{
    /// <summary>
    /// Responsible for actually dealing out the combat related methods.
    /// </summary>
    public static class CombatController
    {
        // This is only really here so that we can to the VictoryController
        private static BattleMapController bMC;

        public static void SetBMC(BattleMapController battleMapController)
        {
            bMC = battleMapController;
        }

        public static void RemoveBMC()
        {
            bMC = null;
        }

        public static void Engage(Unit engager, Unit receiver)
        {
            HandleAttack(engager, receiver);

            // Consume the engager's action
            engager.ConsumeAction();

            if (ShouldEndBattle() == true)
            {
                bMC.EndBattle();
                return;
            }

            if (receiver.IsDead() == true)
                receiver.Die();
        }

        public static void DealDamage(Unit receiver, int damageAmt)
        {
            receiver.MyStats.TakeDamage(damageAmt);

            if (ShouldEndBattle() == true)
            {
                bMC.EndBattle();
                return;
            }

            if (receiver.IsDead() == true)
            {
                receiver.Die();
            }
        }

        public static void Heal(Unit receiver, int healAmt)
        {
            receiver.MyStats.Heal(healAmt);
        }

        private static void HandleAttack(Unit engager, Unit receiver)
        {
            if (engager.CurrentTile.Equals(receiver.CurrentTile) == true)
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat("CombatController :: {0} is attacking and for some reason chose " +
                    "to attack themself. Bailing attack method.", engager.gameObject.name);
#endif
                return;
            }

            if (GameTile.GetCubeDistance(engager.CurrentTile, receiver.CurrentTile) > engager.MyStats.AttackRange
                || engager.HasAlreadyTookActionThisTurn == true)
                return;

            // Calculate the chance to hit
            int chance = engager.CalculateHitChance();
            if (chance >= receiver.MyStats.Evasion)
                receiver.MyStats.TakeDamage(engager.MyStats.Strength);
        }

        private static bool ShouldEndBattle()
        {
            // Check if we should end the battle
            bMC.VictoryController.CheckIfBattleOver();
            if (bMC.VictoryController.IsGameOver() == true)
                return true;

            return false;
        }
    }
}
