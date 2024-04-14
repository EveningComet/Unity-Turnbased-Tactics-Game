using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;

namespace TurnbasedGame.Pathfinding
{
    public class AStar
    {
        /// <summary>
        /// Since the path will be returned in reverse (where the destination is the first), we can just use a
        /// stack.
        /// </summary>
        public Stack<GameTile> ReturnPath { get; private set; }

        public void Calculate(GameTileMapData map, Unit theUnit, GameTile start, GameTile end, int maxSize = 10)
        {
            ReturnPath = new Stack<GameTile>();

            PathfindingPriorityQueue<GameTile> openSet = new PathfindingPriorityQueue<GameTile>(maxSize);
            openSet.Enqueue(start, 0f); // Original tile has distance of 0

            // Tiles already checked
            HashSet<GameTile> closedSet = new HashSet<GameTile>();

            Dictionary<GameTile, GameTile> cameFrom = new Dictionary<GameTile, GameTile>();

            // Cost it takes to walk along tiles.
            // g_score is actual tiles it took to move.
            Dictionary<GameTile, float> gScore = new Dictionary<GameTile, float>();
            gScore[start] = 0f;

            // f_score is the estimated cost.
            Dictionary<GameTile, float> fScore = new Dictionary<GameTile, float>();
            fScore[start] = GameTile.GetAxialDistance(start, end);

            while (openSet.Count > 0)
            {
                GameTile current = openSet.Dequeue();

                // Bail if we already got to our destination
                if (current.Equals(end))
                {
                    ReconstructPath(cameFrom, current);
                    return;
                }

                closedSet.Add(current);
                foreach (GameTile neighbour in current.GetNeighbours())
                {
                    // Ignore the already completed neighbour or neighbour with a non friendly unit
                    // TODO: The check for unfriendly units should maybe check if the pathfinding unit's player is allied to the blocking unit.
                    if (closedSet.Contains(neighbour) == true || (neighbour.Units.Length > 0 && neighbour.Units[0].OwnerId != theUnit.OwnerId))
                    {
                        continue;
                    }

                    float tentativeGScore = gScore[current] + theUnit.GetCostToPathfindTo(current);

                    // If the neighbour is not already in the open set or the new score is better than the old score, we found a tile to use
                    if (openSet.Contains(neighbour) == false || tentativeGScore < gScore[neighbour])
                    {
                        // Either found a new tile or a cheaper route
                        cameFrom[neighbour] = current;
                        gScore[neighbour] = tentativeGScore;
                        fScore[neighbour] = gScore[neighbour] + GameTile.GetAxialDistance(neighbour, end);

                        openSet.EnqueueOrUpdate(neighbour, fScore[neighbour]);
                    }
                }
            }
        }

        private void ReconstructPath(Dictionary<GameTile, GameTile> cameFrom, GameTile current)
        {
            // Make sure to push the current node first, otherwise, our path will not be accurate!
            ReturnPath.Push(current);

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                ReturnPath.Push(current);
            }
        }
    }
}
