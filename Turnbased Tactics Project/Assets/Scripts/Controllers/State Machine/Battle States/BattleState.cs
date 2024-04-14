using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Turns;

namespace TurnbasedGame.StateMachine
{
    public abstract class BattleState : State
    {
        protected TurnController TurnController { get { return owner.TurnController; } }

        protected BattleMapController owner;

        public BattleState(BattleMapController newOwner)
        {
            owner = newOwner;
        }

        /// <summary>
        /// Do the work associated for this <see cref="BattleState"/>.
        /// </summary>
        public virtual void DoWork()
        {
        }
    }
}
