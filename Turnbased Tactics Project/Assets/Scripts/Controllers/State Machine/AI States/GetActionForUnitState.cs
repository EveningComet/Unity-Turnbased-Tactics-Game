using System.Linq;
using System.Collections.Generic;
using TurnbasedGame.AI;
using TurnbasedGame.Units;
using TurnbasedGame.GamePlayerCode;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// State for when the AI needs to get what their unit should do.
    /// </summary>
    public class GetActionForUnitState : AIState
    {
        public GetActionForUnitState(AIController newOwner) : base(newOwner)
        {}

        public override void Process()
        {
            base.Process();
            ProcessUnit();
        }

        private void ProcessUnit()
        {
            AIContext bestContext = GetBestContextForUnit(Players, CurrentUnitProcessingFor);
            owner.ChosenContext = bestContext;
            SetBestTileToMoveTo(bestContext);
            
            // Finally, make the the unit actually move
            owner.ChangeToState( new AIWaitingForFinishedUnitMovementState(this.owner) );
        }

        private AIContext GetBestContextForUnit(Player[] players, Unit unitToDecideFor)
        {
            // Keeps track of the scores, alongside the context
            List<ContextScore> contextScores = new List<ContextScore>();

            // Loop through our enemy units
            int numPlayers = players.Length;
            for (int j = 0; j < numPlayers; j++)
            {
                // Get the other player's units and loop through them
                var otherUnits = players[j].GetUnits();
                int numUnitsOfOther = otherUnits.Length;
                for (int k = 0; k < numUnitsOfOther; k++)
                {
                    // Get the ContextScores from our ActionSet
                    Unit otherUnit = otherUnits[k];
                    contextScores.AddRange(
                        CurrentActionSet.GetContextScores(unitToDecideFor, otherUnit)
                    );
                }
            }

            // Sort the stored contexts based on smallest to largest
            contextScores = contextScores.OrderBy(scoredContexts => scoredContexts.Score).ToList();

            // Get the highest score and do it
            // TODO: Some randomization that will make the AI pick one of the best values.
            AIContext bestThingToDo = contextScores.Last().StoredContext;
            return bestThingToDo;
        }

        /// <summary>
        /// Set the best tile for a unit to move to based on the <see cref="MovementHelper"/>
        /// of the context's chosen action.
        /// </summary>
        private void SetBestTileToMoveTo(AIContext context)
        {
            context.TargetTile = context.ChosenAction.MovementHelper.GetBestTileToMoveTo(
                context.CurrentUnit,
                context.TargetUnit,
                BMC.GetTileMapData()
            );

            Pathfinding.PathRequester.RequestPath(
                BMC.GetTileMapData(),
                context.CurrentUnit,
                context.CurrentUnit.CurrentTile,
                context.TargetTile
            );
        }
    }
}
