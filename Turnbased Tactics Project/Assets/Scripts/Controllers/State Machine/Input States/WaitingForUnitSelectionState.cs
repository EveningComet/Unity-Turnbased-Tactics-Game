using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Inputs;
using UnityEngine;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// The "default" input state. This state is for when the player is looking for a unit to select
    /// and/or looking around the map.
    /// </summary>
    public class WaitingForUnitSelectionState : InputState
    {
        public WaitingForUnitSelectionState(MouseController newOwner) : base(newOwner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            // Clear the selected unit if we have one and unregister any events
            if(owner.SelectionController.CurrentlySelectedUnit != null)
            {
                owner.SelectionController.UnregisterUnitFromPrimary(owner.SelectionController.CurrentlySelectedUnit);
                owner.SelectionController.DeselectUnit();
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void UpdateState(float deltaTime)
        {
            base.UpdateState(deltaTime);

            if (owner.InputController.EndTurnButton == true && IsMyTurn == true)
                owner.BattleMapController.TurnController.EndSubTurn();

            else if(owner.InputController.PressedLeftMouseButton == true)
            {
                // Try to select a unit
                var units = CurrentTile.Units;
                if(units.Length > 0)
                {
                    // Select the unit
                    owner.SelectionController.SelectUnit( units[0] );

                    // Register it to the primary QuickUnitInfoPanel
                    owner.SelectionController.RegisterUnitToPrimary( units[0] );
#if UNITY_EDITOR
                    Debug.LogFormat("WaitingForUnitSelectionState :: Player has selected {0}.", units[0].gameObject.name);
#endif
                    // Move to the UnitCommandSelectionState
                    owner.ChangeToState(new UnitCommandSelectionState(this.owner));
                }
            }
        }
    }
}
