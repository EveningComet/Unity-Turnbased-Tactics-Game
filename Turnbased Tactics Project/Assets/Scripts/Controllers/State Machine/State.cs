using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Base class for an object inside a state machine.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// What to do when entering this <see cref="State"/>.
        /// </summary>
        public virtual void Enter()
        {

        }

        /// <summary>
        /// What to do when exiting this <see cref="State"/>.
        /// </summary>
        public virtual void Exit()
        {

        }
    }
}
