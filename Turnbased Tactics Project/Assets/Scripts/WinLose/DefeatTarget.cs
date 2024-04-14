using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.GamePlayerCode;
using TurnbasedGame.Units;

namespace TurnbasedGame.VictoryControl
{
    /// <summary>
    /// Victory condition for when the player has to defeat a target unit- get a target unit's health to
    /// a specific value.
    /// </summary>
    public class DefeatTarget : WinLoseCondition
    {
        /// <summary>
        /// What the target <see cref="CharacterStats"/>'s HP has to be in order to win.
        /// </summary>
        private int targetHP;

        private CharacterStats targetStats = null;

        public DefeatTarget(Player[] players) : base(players)
        {

        }

        public override void CheckForGameOver()
        {
            base.CheckForGameOver();
        }

        public void SetTargetHP(int tHP)
        {
            targetHP = tHP;
        }

        public void SetTargetStats(CharacterStats newTargetStats)
        {
            targetStats = newTargetStats;
            targetStats.OnStatsChanged += OnStatsChanged;
        }

        public void UnregisterTargetStats()
        {
            targetStats.OnStatsChanged -= OnStatsChanged;
        }

        protected void OnStatsChanged(CharacterStats target)
        {
            if (target.CurrentHP <= targetHP)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.Log("DefeatTarget :: Target unit's HP has gotten to the desired value.");
#endif
                // Keep the unit from dying, if it should not die
                target.CurrentHP = targetHP;

                IsGameOver = true;

                // Set the winner as the player that's not the enemy
                foreach (Player p in Players)
                {
                    if (this.targetStats.OwnerId != p.PlayerId)
                    {
                        Winner = p;
                    }
                }

                // Unsub from the monitored stats
                UnregisterTargetStats();
            }
        }
    }
}
