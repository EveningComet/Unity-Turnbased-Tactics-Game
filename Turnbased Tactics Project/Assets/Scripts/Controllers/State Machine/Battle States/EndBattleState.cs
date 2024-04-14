using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Handles what to do for the <see cref="BattleMapController"/> when the battle(level) is over.
    /// </summary>
    public class EndBattleState : BattleState
    {
        public EndBattleState(BattleMapController newOwner) : base(newOwner)
        {
        }

        public override void DoWork()
        {
            base.DoWork();

#if UNITY_EDITOR
            Debug.LogWarning("EndBattleState :: We don't know what to do when a battle is over, so just restart the game.");
#endif
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
