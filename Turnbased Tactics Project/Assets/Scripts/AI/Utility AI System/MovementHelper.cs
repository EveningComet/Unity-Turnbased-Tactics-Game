using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;

namespace TurnbasedGame.AI
{
    /// <summary>
    /// Used to help find the best <see cref="GameTile"/> an AI should move a <see cref="Unit"/>.
    /// </summary>
    public struct MovementHelper
    {
        [Newtonsoft.Json.JsonProperty("movementConsiderations")]
        private Consideration[] movementConsiderations;

        [Newtonsoft.Json.JsonConstructor]
        public MovementHelper(Consideration[] considerations)
        {
            movementConsiderations = considerations;
        }

        public GameTile GetBestTileToMoveTo(Unit unitToFindTileFor, Unit targetUnit, GameTileMapData tileMapData)
        {
            List<GameTile> tilesToCheck = tileMapData.GetTilesWithinRange(
                unitToFindTileFor.CurrentTile,
                unitToFindTileFor.MyStats.CurrentMovementPointsRemaining,
                true
            );
            int numTiles = tilesToCheck.Count;
            List<ContextScore> scoredContexts = new List<ContextScore>();

            for (int t = 0; t < numTiles; t++)
            {
                GameTile tileToCheck = tilesToCheck[t];

                // Ignore the tile if it has someone on it
                if (tileToCheck.Units.Length > 0)
                    continue;

                AIContext contextToConsider = new AIContext
                {
                    CurrentUnit = unitToFindTileFor,
                    TargetUnit = targetUnit,
                    TargetTile = tileToCheck
                };

                float tileScore = GetScoreForTile(contextToConsider);
                ContextScore cs = new ContextScore
                {
                    Score = tileScore,
                    StoredContext = contextToConsider
                };
                scoredContexts.Add(cs);
            }

            // Sort the stored contexts based on a score of smallest to largest and return the best scoring tile
            scoredContexts = scoredContexts.OrderBy(scoredC => scoredC.Score).ToList();
            return scoredContexts.Last().StoredContext.TargetTile;
        }

        private float GetScoreForTile(AIContext myContext)
        {
            float finalScore = 1f;
            int numConsiderations = movementConsiderations.Length;
            float modificationFactor = 1f - (1f / (float)numConsiderations);
            for (int i = 0; i < numConsiderations; i++)
            {
                float score = movementConsiderations[i].GetScore(myContext);
                float makeUpValue = (1f - score) * modificationFactor;
                finalScore *= score + (makeUpValue * score);

                if (finalScore == 0f)
                    return 0f;
            }
            return finalScore;
        }
    }
}
