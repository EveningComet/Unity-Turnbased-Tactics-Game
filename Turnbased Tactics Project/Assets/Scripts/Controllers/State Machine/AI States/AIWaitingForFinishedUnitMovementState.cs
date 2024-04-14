using TurnbasedGame.Units;

namespace TurnbasedGame.StateMachine
{
    public class AIWaitingForFinishedUnitMovementState : AIState
    {
        public AIWaitingForFinishedUnitMovementState(AIController newOwner) : base(newOwner)
        {}

        public override void Process()
        {
            base.Process();

            // When entering this state, we can assume that the AI needs to move their unit
            CurrentUnitProcessingFor.UnitView.MoveUnit();
            CurrentUnitProcessingFor.UnitView.OnUnitFinishedMoving += OnUnitFinishedMoving;
        }

        private void OnUnitFinishedMoving(Unit unitThatHasFinishedMoving)
        {
            // Unsubscribe from the event
            unitThatHasFinishedMoving.UnitView.OnUnitFinishedMoving -= OnUnitFinishedMoving;

            // Make the AI execute the action
            owner.ChangeToState(new ExecuteChosenContextState(this.owner));
        }
    }
}
