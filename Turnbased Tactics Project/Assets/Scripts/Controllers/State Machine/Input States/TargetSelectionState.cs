using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Inputs;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Base class for controlling input related to abilities and/or generic attacks.
    /// </summary>
    public abstract class TargetSelectionState : InputState
    {
        /// <summary>
        /// Stores the <see cref="GameTile"/>s being targeted.
        /// </summary>
        protected List<GameTile> selectedTiles;

        public TargetSelectionState(MouseController newOwner) : base (newOwner)
        {

        }

        public override void Enter()
        {
            base.Enter();

            // Show the relevant menus
            owner.SelectionController.ShowPrimary(owner.SelectionController.CurrentlySelectedUnit);
            owner.SelectionController.ShowAbilityHotPanel(owner.SelectionController.CurrentlySelectedUnit);

            // Call the method for safety reasons
            OnTileUnderCursorChanged(owner.CurrentTileUnderCursor);
        }

        public override void Exit()
        {
            base.Exit();

            // Hide the relevant menus
            owner.SelectionController.HidePrimary();
            owner.SelectionController.HideSecondary();
            owner.SelectionController.HideSuccessIndicator();
            owner.SelectionController.HideAbilityHotPanel();

            // Unhighlight targets
            UnhighlightTiles(selectedTiles);
        }

        public override void UpdateState(float deltaTime)
        {
            base.UpdateState(deltaTime);

            // Go back to the UnitCommandSelectionState
            if (owner.InputController.PressedCancelButton == true)
            {
                owner.ChangeToState(new UnitCommandSelectionState(this.owner));
                return; // Bail to prevent weird stuff from happening
            }

            // Do the attack/ability/whatever
            if (owner.InputController.PressedLeftMouseButton == true)
            {
                Activate();
            }
        }

        /// <summary>
        /// The player has decided to execute an attack/ability/whatever, so do it.
        /// Meant to be overridden by child classes.
        /// </summary>
        protected virtual void Activate()
        {

        }
    }
}
