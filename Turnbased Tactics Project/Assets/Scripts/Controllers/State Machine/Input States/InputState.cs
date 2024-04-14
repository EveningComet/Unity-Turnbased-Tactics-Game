using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Tiles;
using TurnbasedGame.Inputs;
using TurnbasedGame.Units;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Base class for a <see cref="State"/> that will control what to do based on
    /// a non-AI player's input.
    /// </summary>
    public abstract class InputState : State
    {
        /// <summary>
        /// The <see cref="GameTile"/> currently being looked at by the thing this state is controlling for.
        /// </summary>
        public GameTile CurrentTile { get { return owner.CurrentTileUnderCursor; } }

        /// <summary>
        /// The thing this <see cref="InputState"/> is controlling.
        /// </summary>
        protected MouseController owner;

        protected Unit CurrentlySelectedUnit { get { return owner.SelectionController.CurrentlySelectedUnit; } }
        protected bool IsMyTurn { get { return owner.MyPlayer.IsMyTurn; } }

        public InputState(MouseController newOwner)
        {
            this.owner = newOwner;
        }

        public override void Enter()
        {
            base.Enter();
            owner.OnTUCC += OnTileUnderCursorChanged;
        }

        public override void Exit()
        {
            base.Exit();
            owner.OnTUCC -= OnTileUnderCursorChanged;
        }

        /// <summary>
        /// Update the state.
        /// </summary>
        /// <param name="deltaTime">Delta time.</param>
        public virtual void UpdateState(float deltaTime)
        {
        }

        protected void HighlightTiles(List<GameTile> tilesToHighlight, bool movementRelated)
        {
            owner.BattleMapController.HighlightTiles(tilesToHighlight, movementRelated);
        }

        protected void UnhighlightTiles(List<GameTile> tilesToUnhighlight)
        {
            owner.BattleMapController.UnhighlightTiles(tilesToUnhighlight);
        }

        /// <summary>
        /// What to do when the tile under the cursor has changed.
        /// </summary>
        protected virtual void OnTileUnderCursorChanged(GameTile newTileUnderCursor)
        {

        }

        /// <summary>
        /// Check if the the battle is over so we can return to the overworld or whatever.
        /// </summary>
        protected void CheckIfBattleOver()
        {
            owner.BattleMapController.VictoryController.CheckIfBattleOver();

            if(owner.BattleMapController.VictoryController.IsGameOver() == true)
                owner.BattleMapController.EndBattle();
        }
    }
}
