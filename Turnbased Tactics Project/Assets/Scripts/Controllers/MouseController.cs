using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TurnbasedGame.Tiles;
using TurnbasedGame.StateMachine;
using TurnbasedGame.Abilities;
using TurnbasedGame.GamePlayerCode;

namespace TurnbasedGame.Inputs
{
    // TODO: Find a better name for this class.
    /// <summary>
    /// Handles stuff related to the non-AI player.
    /// </summary>
    public class MouseController : StateMachine<InputState>
    {
        #region Delegates/Events
        /// <summary>
        /// Helps prevent the input control state classes from checking if they should update
        /// stuff related to the current tile under the cursor every frame.
        /// </summary>
        public delegate void TileUnderCursorChanged(GameTile newTileUnderCursor);
        public event TileUnderCursorChanged OnTUCC;
        #endregion

        private Camera cam = null;

        /* TODO: This is kind of "bad", but we need some way to get to the BattleMapController and some of its functions (i.e: 
         * center position of a tile for help with unit stuff). Maybe find a better way?*/
        public BattleMapController BattleMapController { get; private set; }

        public InputController InputController { get; private set; }

        /// <summary>
        /// The current <see cref="GameTile"/> under the mouse/cursor that is not null.
        /// </summary>
        public GameTile CurrentTileUnderCursor { get; private set; }

        public SelectionController SelectionController { get; private set; }

        public TooltipController TooltipController { get; private set; }

        public Ability CurrentlyActivatedAbility { get; private set; }

        /// <summary>
        /// The <see cref="Player"/> this is controlling for. This is used to prevent the non-AI players from being to able to do things
        /// like move units they should not control.
        /// </summary>
        public Player MyPlayer { get; private set; }

        /// <summary>
        /// Set up this <see cref="MouseController"/> for a non-AI player.
        /// </summary>
        public void Initialize(Player myPlayer, BattleMapController bMC, SelectionController sC, Camera camera)
        {
            this.MyPlayer = myPlayer;
            this.BattleMapController = bMC;
            this.SelectionController = sC;
            this.cam = camera;
            this.InputController = new InputController();

            // Initialize our tooltip
            TooltipController = new TooltipController(bMC.TooltipCanvasPrefab);

            // Setup our AbilityHotPanel
            SelectionController.SetAbilityHotPanelController(this);

            ChangeToState(new WaitingForUnitSelectionState(this));
        }

        // Update is called once per frame
        void Update()
        {
            InputController.UpdateInputs();

            /* If the mouse is over a UI element, don't do anything.
             * NOTE: Is ignoring ALL GUI objects desired?
             * Consider things like unit health bars, resource icons, etc...
             * Although, if those are set to NotInteractive or Not Block Raycasts,
             * maybe this will return false for them anyway.. */
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 mousePos = cam.ScreenToWorldPoint(InputController.MousePos);

            // Check for a tile under the mouse position
            var coord = BattleMapController.UnityTileMap.layoutGrid.WorldToCell(mousePos);
            var newTileUnderCursor = BattleMapController.GetGameTileAtWorldPosition(coord);

            if (newTileUnderCursor != null)
            {
                if (CurrentTileUnderCursor != null && CurrentTileUnderCursor.Equals(newTileUnderCursor) == false)
                {
                    CurrentTileUnderCursor = newTileUnderCursor;

                    // Fire the event
                    if (OnTUCC != null)
                        OnTUCC.Invoke(CurrentTileUnderCursor);
                }

                // Just set the tile otherwise
                else
                {
                    CurrentTileUnderCursor = newTileUnderCursor;
                }
            }

            // Update the current input state
            currentState.UpdateState(Time.deltaTime);
        }

        private void LateUpdate()
        {
            TooltipController.UpdateTooltipPosition(InputController.MousePos);
        }

        #region Ability Methods
        public void ActivateTargetingForAbility(Ability abilityToTargetFor)
        {
            // Have us keep track of the new ability
            CurrentlyActivatedAbility = abilityToTargetFor;

            ChangeToState(new AbilityTargetState(this));
        }

        public void ClearActivatedAbility()
        {
            CurrentlyActivatedAbility = null;
        }
        #endregion
    }
}