using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TurnbasedGame.Units;
using TurnbasedGame.AI;

namespace TurnbasedGame.GamePlayerCode
{
    /// <summary>
    /// A player in the game world.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The id for this player.
        /// </summary>
        public int PlayerId { get; private set; }
        private bool isMyTurn;
        public bool IsMyTurn { get { return isMyTurn; } }

        public PlayerType PlayerType { get; private set; }

        private HashSet<Unit> myUnits;

        public Player(int id, bool isControlledByAI = false)
        {
            this.PlayerId = id;
            this.PlayerType = isControlledByAI == true ? PlayerType.AI : PlayerType.NotAI;

            myUnits = new HashSet<Unit>();
            isMyTurn = false;
        }

        public Unit[] GetUnits()
        {
            return myUnits.ToArray();
        }

        public void SetPlayerType(PlayerType newPlayerType)
        {
            PlayerType = newPlayerType;
        }

        public void AddUnit(Unit newUnit)
        {
            myUnits.Add(newUnit);
            newUnit.OnUnitDestroyed += OnUnitDestroyed;
        }

        public void RemoveUnit(Unit unitToRemove)
        {
            myUnits.Remove(unitToRemove);
            unitToRemove.OnUnitDestroyed -= OnUnitDestroyed;
        }

        private void OnUnitDestroyed(Unit unitThatHasDied)
        {
            RemoveUnit(unitThatHasDied);
        }

        /// <summary>
        /// Is it this player's turn?
        /// </summary>
        public void SetTurnStatus(bool status)
        {
            isMyTurn = status;
        }
    }
}