using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;

namespace TurnbasedGame.Pathfinding
{
    /// <summary>
    /// Used to request an <see cref="AStar"/> path.
    /// </summary>
    public static class PathRequester
    {
        /// <summary>
        /// Request an <see cref="AStar"/> path.
        /// </summary>
        /// <param name="map">The game map.</param>
        /// <param name="requestingUnit">Unit wanting the pathfinding.</param>
        /// <param name="start">Start tile.</param>
        /// <param name="end">End tile.</param>
        /// <param name="maxSize">The max size.</param>
        public static void RequestPath(GameTileMapData map, Unit requestingUnit, GameTile start, GameTile end, int maxSize = 10)
        {
            // Why pathfind if we're already there?
            if (start.Equals(end) == true)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogErrorFormat("PathRequester :: {0} is trying to pathfind to a tile it is already at. Bailing.", requestingUnit.gameObject.name);
#endif
                return;
            }

            // The desired tile already has a unit on it
            else if (end.Units.Length > 0)
            {
                // If the distance is 1 tile away, don't even bother with pathfinding
                if (GameTile.GetCubeDistance(start, end) == 1)
                    return;

                // Otherwise, let's try to find the "closest" tile that we could pathfind to
                else
                {
                    float smallestCost = float.MaxValue;

                    var neighbours = end.GetNeighbours();
                    int numNeighbours = neighbours.Length;
                    for (int i = 0; i < numNeighbours; i++)
                    {
                        float d = GameTile.GetCubeDistance(start, neighbours[i]);
                        if (d < smallestCost)
                        {
                            smallestCost = d;
                            end = neighbours[i];
                        }
                    }
                }
            }

            AStar aStar = new AStar();
            aStar.Calculate(map, requestingUnit, start, end, maxSize);
            requestingUnit.SetPath(aStar.ReturnPath);
#if UNITY_EDITOR
            UnityEngine.Debug.LogFormat("PathRequester :: Path length for {0} is: {1}.", requestingUnit.gameObject.name, aStar.ReturnPath.Count);
#endif
        }
    }
}