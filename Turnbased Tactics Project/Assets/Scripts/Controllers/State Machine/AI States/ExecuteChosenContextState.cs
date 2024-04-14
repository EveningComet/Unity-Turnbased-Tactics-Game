using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TurnbasedGame.Tiles;
using TurnbasedGame.AI;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// When the AI is waiting for a <see cref="Unit"/> to finish something like a combat animation,
    /// so that we can actually do the action of the chosen context.
    /// </summary>
    public class ExecuteChosenContextState : AIState
    {
        private Unit unitToMonitor = null;

        public ExecuteChosenContextState(AIController newOwner) : base(newOwner)
        {
        }

        public override void Exit()
        {
            base.Exit();
            unitToMonitor = null;
        }

        public override void Process()
        {
            base.Process();

            // Check if the unit can legally execute it's action
            if(MayExecuteAction(owner.ChosenContext) == true)
            {
                // Subscribe to the relevant event and make the unit do the animation
                unitToMonitor = owner.ChosenContext.CurrentUnit;
                unitToMonitor.UnitView.OnUnitFinishedAnimation += OnUnitFinishedAnimation;
                unitToMonitor.UnitView.Perform(owner.ChosenContext.TargetUnit.transform.localPosition);
            }

            // If not legal, then tell the AIController what to do next
            else
                owner.FinishActionExecution();
        }

        private void OnUnitFinishedAnimation(Unit unitThatHasFinishedAnimation)
        {
            ExecuteAction(owner.ChosenContext);

            // Unsub from the event
            unitToMonitor.UnitView.OnUnitFinishedAnimation -= OnUnitFinishedAnimation;

            // Tell the AIController what to do next
            owner.FinishActionExecution();
        }

        /// <summary>
        /// Finally execute the action.
        /// </summary>
        private void ExecuteAction(AIContext contextToExecute)
        {
            switch (contextToExecute.ChosenAction.ActionType)
            {
                case AIActionTypes.GenericAttack:
                    // Make our unit attack the target unit
                    Combat.CombatController.Engage(
                        contextToExecute.CurrentUnit,
                        contextToExecute.TargetUnit
                    );
                    break;
                
                case AIActionTypes.UseDamagingAbility:
                case AIActionTypes.UseHealingAbility:
                    // Get the needed tiles for the ability
                    var targetTiles = contextToExecute.ChosenAbility.aAOE.GetTilesInArea(
                        BMC,
                        contextToExecute.TargetUnit.CurrentTile
                    );

                    // Have the current unit use the ability
                    contextToExecute.CurrentUnit.TriggerAbility(
                        contextToExecute.ChosenAbility,
                        targetTiles
                    );
                    break;

                default:
#if UNITY_EDITOR
                Debug.LogErrorFormat("ExecuteChosenContextState :: Don't have anything for {0}.",
                    contextToExecute.ChosenAction.ActionType
                );
#endif
                    break;
            }
        }

        private bool MayExecuteAction(AIContext contextToCheck)
        {
            float d = GameTile.GetCubeDistance(
                contextToCheck.CurrentUnit.CurrentTile,
                contextToCheck.TargetUnit.CurrentTile
            );

            switch(contextToCheck.ChosenAction.ActionType)
            {
                case AIActionTypes.GenericAttack:
                    if(d <= contextToCheck.CurrentUnit.MyStats.AttackRange)
                        return true;
                    break;

                case AIActionTypes.UseHealingAbility:
                case AIActionTypes.UseDamagingAbility:
                    if(d <= contextToCheck.ChosenAbility.abilityTargetingLogic.range)
                        return true;
                    break;

                default:
#if UNITY_EDITOR
                Debug.LogErrorFormat("ExecuteChosenContextState :: Don't have anything to check for {0}.",
                    contextToCheck.ChosenAction.ActionType
                );
#endif
                    break;
            }

            return false;
        }
    }
}
