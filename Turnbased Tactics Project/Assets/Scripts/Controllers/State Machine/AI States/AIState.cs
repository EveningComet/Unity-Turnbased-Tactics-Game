using TurnbasedGame.AI;
using TurnbasedGame.Units;
using TurnbasedGame.GamePlayerCode;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Base class for a <see cref="State"/> related to the AI.
    /// </summary>
    /// <remarks>
    /// Base class for what an AI could be doing at a given point in time.
    ///</remarks>
    public abstract class AIState : State
    {
        protected AIActionSet CurrentActionSet { get { return owner.AIActionSet; } }
        protected Unit CurrentUnitProcessingFor { get { return owner.CurrentUnitProcessingFor; } }
        protected Player[] Players { get { return owner.Players; } }
        protected BattleMapController BMC { get { return owner.BMC; } }

        protected AIController owner;

        public AIState(AIController newOwner)
        {
            owner = newOwner;
        }

        /// <summary>
        /// Do the work that this <see cref="AIState"/> is responsible for.
        /// </summary>
        public virtual void Process()
        {

        }
    }
}
