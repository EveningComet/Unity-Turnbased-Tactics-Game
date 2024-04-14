using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.GamePlayerCode;
using TurnbasedGame.Units;
using TurnbasedGame.AI;

namespace TurnbasedGame.Turns
{
    /// <summary>
    /// Controls the game turns. Also keeps track of the players in the game board.
    /// </summary>
    public class TurnController : MonoBehaviour
    {
        /// <summary>
        /// The players on the current game map.
        /// </summary>
        private Player[] players;
        public Player[] Players { get { return players; } }

        private int currentPlayerIndex = 0;

        public int CurrentTurnNumber { get; private set; }

        private Player currentPlayer = null;
        private AIController aiController;

        private void Start()
        {
            // NOTE: When loading from a game, this number should be something else.
            CurrentTurnNumber = 1;
#if UNITY_EDITOR
            Debug.LogFormat("TurnController :: Starting turn number: {0}.", CurrentTurnNumber);
#endif
        }

        public void SetAIController(AIController aIC)
        {
            aiController = aIC;
        }

        /// <summary>
        /// Generate the players based on the number of passed players.
        /// </summary>
        public void GeneratePlayers(int numberOfPlayers)
        {
            players = new Player[numberOfPlayers];
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i] = new Player(i, true);
            }

            // Set the first player to not be controlled by the ai
            // TODO: We probably want a better way of setting this. For now, this ok.
            players[0].SetPlayerType(PlayerType.NotAI);

            currentPlayerIndex = 0;

            // Learn about the current player
            currentPlayer = GetCurrentPlayer();
            currentPlayer.SetTurnStatus(true);
        }

        public Player GetCurrentPlayer()
        {
            return players[currentPlayerIndex];
        }

        public Player GetPlayerAtIndex(int playerIndex)
        {
            return players[playerIndex];
        }

        public void AdvanceToNextTurn()
        {
            CurrentTurnNumber++;
        }

        /// <summary>
        /// End the turn for the current player.
        /// </summary>
        public void EndSubTurn()
        {
#if UNITY_EDITOR
            Debug.LogFormat("TurnController :: Current player {0} has ended their turn.", currentPlayer.PlayerId);
#endif
            currentPlayer.SetTurnStatus(false);

            // Go to the next player
            AdvanceToNextPlayer();
        }

        private void AdvanceToNextPlayer()
        {
            // Prevent going out of the bounds of total players
            currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

            // We have started a new turn
            if (currentPlayerIndex == 0)
            {
                AdvanceToNextTurn();
#if UNITY_EDITOR
                Debug.LogFormat("TurnController :: Starting a new turn. Turn number: {0}.", CurrentTurnNumber);
#endif
            }

            // Set the next player
            currentPlayer = GetCurrentPlayer();
            currentPlayer.SetTurnStatus(true);

            // Reset the current player's units
            foreach (Unit u in currentPlayer.GetUnits())
            {
                u.RefreshForNewTurn();
            }

#if UNITY_EDITOR
            Debug.LogFormat("TurnController :: Starting turn number for player at index {0}, they're {1}.", currentPlayerIndex, currentPlayer.PlayerType.ToString());
#endif

            if (currentPlayer.PlayerType == PlayerType.AI)
                aiController.BeginProcess(currentPlayer, players);
        }
    }
}