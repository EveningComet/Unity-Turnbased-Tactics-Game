using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Inputs;
using UnityEngine;
using TurnbasedGame.Abilities;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// State for when the player has activated an <see cref="Ability"/>.
    /// </summary>
    public class AbilityTargetState : TargetSelectionState
    {
        private List<GameTile> aoeTiles = new List<GameTile>();

        public AbilityTargetState(MouseController newOwner) : base(newOwner)
        {
        }

        public override void Enter()
        {
            base.Enter();

            // Get the tiles for the ability
            selectedTiles = owner.CurrentlyActivatedAbility.GetTargets(
                owner.SelectionController.CurrentlySelectedUnit,
                owner.BattleMapController.GetTileMapData()
            );

            // Highlight the tiles
            HighlightTiles(selectedTiles, false);
#if UNITY_EDITOR
            Debug.Log("AbilityTargetState :: Entered.");
#endif
        }

        public override void Exit()
        {
            base.Exit();
            aoeTiles.Clear();
#if UNITY_EDITOR
            Debug.Log("AbilityTargetState :: Exited.");
#endif
        }

        protected override void OnTileUnderCursorChanged(GameTile newTileUnderCursor)
        {
            var targets = newTileUnderCursor.Units;

            // TODO: Highlight AOE target tiles.

            // Only update the display if the tile is inside the selected tiles and has a unit
            if (targets.Length > 0 && selectedTiles.Contains(newTileUnderCursor))
            {
                // Cache the values so we don't have to keep rewriting them
                var target = targets[0];
                var abilityToActivate = owner.CurrentlyActivatedAbility;

                // Get the possible chance to succeed
                int successAmount = abilityToActivate.hitSuccessRate.Calculate(target.CurrentTile);

                int amountToShow = 0;

                // Get the potential damage/heal amount
                foreach (var ae in abilityToActivate.effects)
                {
                    amountToShow += ae.Predict(CurrentlySelectedUnit, newTileUnderCursor);
                }

                owner.SelectionController.ShowSecondary(target);
                owner.SelectionController.UpdateSuccessRateText(
                    successAmount,
                    amountToShow
                );
            }

            else
            {
                owner.SelectionController.HideSecondary();
                owner.SelectionController.HideSuccessIndicator();
            }
        }

        protected override void Activate()
        {
            base.Activate();

            if (selectedTiles.Contains(owner.CurrentTileUnderCursor))
            {
                // Get the AOE of the ability
                AbilityAOE aAOE = owner.CurrentlyActivatedAbility.aAOE;

                // Get the tile(s) that should be affected
                aoeTiles.AddRange(aAOE.GetTilesInArea(owner.BattleMapController, CurrentTile));

                // Allow the unit to have time to trigger animations
                CurrentlySelectedUnit.UnitView.Perform(CurrentTile.WorldSpaceCenter);
                CurrentlySelectedUnit.UnitView.OnUnitFinishedAnimation += OnUnitFinishedAnimation;
            }
        }

        private void OnUnitFinishedAnimation(Units.Unit u)
        {
            u.UnitView.OnUnitFinishedAnimation -= OnUnitFinishedAnimation;

            CurrentlySelectedUnit.TriggerAbility(
                owner.CurrentlyActivatedAbility,
                aoeTiles
            );

            // TODO: Find a better way to do this.
            CheckIfBattleOver();

            // TODO: because we're checking if the battle is over, this should no longer be like this.
            // Bail
            owner.ChangeToState(new UnitCommandSelectionState(this.owner));
        }
    }
}
