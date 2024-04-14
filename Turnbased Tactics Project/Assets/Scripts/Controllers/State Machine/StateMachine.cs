using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Base class for something controlled by a <see cref="State"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="State"/> this machine cares about.</typeparam>
    public abstract class StateMachine<T> : MonoBehaviour where T: State
    {
        /// <summary>
        /// The current state inside this <see cref="StateMachine{T}"/>. The child classes
        /// control what data type this is.
        /// </summary>
        protected T currentState;

        /// <summary>
        /// Used to prevent state changes when already transitioning.
        /// </summary>
        private bool isTransitioning = false;

        /// <summary>
        /// Change to the passed state. Overridable in case the child classes need to do extra things.
        /// </summary>
        /// <param name="newState">The new state.</param>
        public virtual void ChangeToState(State newState)
        {
            // Don't enter a state we're currently in or if we're changing states
            if (currentState == newState || isTransitioning == true)
                return;

            isTransitioning = true;

            if (currentState != null)
                currentState.Exit();

            // Set our current state to the new state, but only as the type of state we care about
            currentState = newState as T;

            newState.Enter();

            isTransitioning = false;
        }
    }
}
