using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Inputs;
using UnityEngine;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// The state for when the player has selected a unit.
    /// </summary>
    public class UnitCommandSelectionState : InputState
    {
        private List<GameTile> tilesAvailableForMovement = new List<GameTile>();

        public UnitCommandSelectionState(MouseController newOwner) : base(newOwner)
        {
        }

        public override void Enter()
        {
            base.Enter();

#if UNITY_EDITOR
            Debug.Log("UnitCommandSelectionState :: Entered.");
#endif
            owner.SelectionController.ShowPrimary(owner.SelectionController.CurrentlySelectedUnit);
            owner.SelectionController.ShowAbilityHotPanel(owner.SelectionController.CurrentlySelectedUnit);

            CurrentlySelectedUnit.OnUnitHasTakenAction += OnNewTurn;
            CurrentlySelectedUnit.UnitView.OnUnitFinishedMoving += OnUnitFinishedMoving;
            OnUnitFinishedMoving(CurrentlySelectedUnit);
        }

        public override void Exit()
        {
            base.Exit();
#if UNITY_EDITOR
            Debug.Log("UnitCommandSelectionState :: Exited.");
#endif
            CurrentlySelectedUnit.OnUnitHasTakenAction -= OnNewTurn;
            CurrentlySelectedUnit.UnitView.OnUnitFinishedMoving -= OnUnitFinishedMoving;
            UnhighlightTiles(tilesAvailableForMovement);
            tilesAvailableForMovement.Clear();

            owner.SelectionController.HidePrimary();
            owner.SelectionController.HideAbilityHotPanel();
        }

        public override void UpdateState(float deltaTime)
        {
            base.UpdateState(deltaTime);

            // Prevent the player from doing stuff with their unit's if it's not their turn
            if (IsMyTurn == false)
                return;

            // Exit this state if the player pressed the cancel button
            if (owner.InputController.PressedCancelButton == true)
            {
                owner.ChangeToState(new WaitingForUnitSelectionState(this.owner));
                return; // Bail to prevent errors
            }

            else if (owner.InputController.EndTurnButton == true && IsMyTurn == true)
                owner.BattleMapController.TurnController.EndSubTurn();

            if (CurrentlySelectedUnit.OwnerId == owner.MyPlayer.PlayerId)
            {

                if (owner.InputController.PressedAttackButton == true
                    && CurrentlySelectedUnit.HasAlreadyTookActionThisTurn == false)
                {
                    // TODO: Refactor this.
                    //owner.ChangeToState(new UnitAttackTargetSelectionState(this.owner));
                }

                // Pathfind for the unit
                else if (owner.InputController.PressedRightMouseButton == true
                    && CurrentlySelectedUnit.UnitView.IsTransitioning == false)
                {
                    UnhighlightTiles(tilesAvailableForMovement);

                    // Get the path for the unit
                    Pathfinding.PathRequester.RequestPath(
                        owner.BattleMapController.GetTileMapData(),
                        CurrentlySelectedUnit,
                        CurrentlySelectedUnit.CurrentTile,
                        this.CurrentTile
                    );

                    // Tell the unit mover to move the unit
                    CurrentlySelectedUnit.UnitView.MoveUnit();
                }
            }
        }

        private void OnUnitFinishedMoving(Unit u)
        {
            // Unhighlight the currently highlighted tiles, if there are any
            UnhighlightTiles(tilesAvailableForMovement);
            tilesAvailableForMovement.Clear();

            // Highlight the tiles based on how much movement points the unit has
            tilesAvailableForMovement.AddRange(
                owner.BattleMapController.GetTileMapData().GetTilesWithinRange(
                    u.CurrentTile,
                    u.MyStats.CurrentMovementPointsRemaining,
                    true
                )
            );
            FilterMovementTiles();
            HighlightTiles(tilesAvailableForMovement, true);
        }

        private void OnNewTurn(Unit u)
        {
            // TODO: This is kind of bad for performance. Fix this.
            OnUnitFinishedMoving(u);
        }

        /// <summary>
        /// Get rid of tiles that contain things like units.
        /// </summary>
        private void FilterMovementTiles()
        {
            for (int i = tilesAvailableForMovement.Count - 1; i >= 0; i--)
            {
                if(tilesAvailableForMovement[i].Units.Length > 0)
                {
                    tilesAvailableForMovement.RemoveAt(i);
                }
            }
        }
    }
}
