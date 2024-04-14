using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.GamePlayerCode;
using UnityEngine;

namespace TurnbasedGame.VictoryControl
{
    /// <summary>
    /// Victory condition where all enemies must be defeated.
    /// </summary>
    public class DefeatAllEnemies : WinLoseCondition
    {
        public DefeatAllEnemies(Player[] players) : base(players)
        {
        }

        public override void CheckForGameOver()
        {
            base.CheckForGameOver();
        }
    }
}
