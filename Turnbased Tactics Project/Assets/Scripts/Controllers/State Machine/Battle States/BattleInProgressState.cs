using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// The default <see cref="BattleState"/> for the <see cref="BattleMapController"/>
    /// after generating the map.
    /// </summary>
    public class BattleInProgressState : BattleState
    {
        public BattleInProgressState(BattleMapController newOwner) : base(newOwner)
        {
        }
    }
}
