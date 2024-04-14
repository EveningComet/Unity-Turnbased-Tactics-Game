using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.GamePlayerCode;
using TurnbasedGame.Units;

namespace TurnbasedGame.VictoryControl
{
    /// <summary>
    /// Base class for determining whether or not a player has won or failed a level/mission/map/etc.
    /// </summary>
    public abstract class WinLoseCondition
    {
        public Player[] Players { get; private set; }
        public Player Winner { get; protected set; }

        public bool IsGameOver { get; protected set; }

        public WinLoseCondition(Player[] players)
        {
            this.Players = players;
            IsGameOver = false;
        }

        public virtual void CheckForGameOver()
        {
            Dictionary<int, Player> defeatedPlayers = new Dictionary<int, Player>();
            
            // Loop through the units of all players
            foreach(Player p in Players)
            {
                if (IsPartyDefeated(p) == true)
                    defeatedPlayers.Add(p.PlayerId, p);
            }

            // The last player standing is the winner
            foreach(Player p in Players)
            {
                if(defeatedPlayers.ContainsKey(p.PlayerId) == false && defeatedPlayers.Count == Players.Length - 1)
                {
                    Winner = p;
                    IsGameOver = true;
                }
            }
        }

        /// <summary>
        /// Are all the <see cref="Unit"/>s of this <see cref="Player"/> defeated?
        /// </summary>
        private bool IsPartyDefeated(Player playerToCheck)
        {
            foreach (Unit unit in playerToCheck.GetUnits())
            {
                // TODO: Check if a unit has been knocked out.
                if (unit.IsDead() == false)
                    return false;
            }
            return true;
        }
    }
}
