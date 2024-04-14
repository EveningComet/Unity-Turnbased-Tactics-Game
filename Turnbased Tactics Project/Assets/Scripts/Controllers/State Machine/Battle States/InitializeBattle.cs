using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.GamePlayerCode;
using UnityEngine.Tilemaps;
using TurnbasedGame.Tiles;
using TurnbasedGame.Inputs;

namespace TurnbasedGame.StateMachine
{
    /// <summary>
    /// Responsible for doing everything related to setting up the map for the <see cref="BattleMapController"/>.
    /// </summary>
    public class InitializeBattle : BattleState
    {
        /// <summary>
        /// The <see cref="GameTileMapData"/> that will be set for the owner.
        /// </summary>
        private GameTileMapData gameTileMapData;

        public InitializeBattle(BattleMapController newOwner) : base(newOwner)
        {
        }

        public override void DoWork()
        {
            base.DoWork();

            SetupMap();
            TurnController.GeneratePlayers(2); // TODO: Generate players based on the Level.
            GeneratePlayerComponents();
            SpawnUnitsForMap();

            // Set up the mission
            owner.VictoryController.SetObjective(owner.CurrentLevel, TurnController.Players);

            // Switch to the default state
            owner.ChangeToState( new BattleInProgressState(owner) );
        }

        /// <summary>
        /// Checks the tiles placed inside the Unity Tilemap to easily help us generate map data.
        /// </summary>
        private void SetupMap()
        {
            if (owner.UnityTileMap == null)
            {
#if UNITY_EDITOR
                Debug.LogError("InitializeBattle :: Oy vey! The variable storing the Unity Tilemap is null!");
#endif
                return;
            }

            owner.CompressUnityTileMapBounds();

#if UNITY_EDITOR
            //Debug.LogFormat("InitializeBattle :: Current game map size is: {0}", owner.UnityTileMap.size);
#endif

            int width = owner.UnityTileMap.size.x;
            int height = owner.UnityTileMap.size.y;
            gameTileMapData = new GameTileMapData(width, height);

            // Generate the tile data
            GenerateTiles(width, height);

            // Now generate the tile neighbours
            GenerateNeighbours(width, height);

#if UNITY_EDITOR
            foreach(GameTile n in gameTileMapData.GetTile(0, 1).GetNeighbours())
            {
                Debug.LogFormat("InitializeBattle :: Tile (0,1) has neighbour: {0}.", n.ToString());
            }
#endif

            // Set the newly generated map
            owner.SetGameTileMapData(gameTileMapData);
        }

        private void GenerateTiles(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);
                    TileBase tb = owner.UnityTileMap.GetTile(position);

                    if (tb != null)
                    {
                        int movementCost = 1;

                        if (tb.name == "grass_14" || tb.name == "grass_13")
                        {
                            movementCost = 2;
                        }

                        GameTile t = new GameTile(x, y, owner.GetTileWorldSpaceCenter(x, y), movementCost);
                        gameTileMapData.SetTileData(x, y, t);

#if UNITY_EDITOR
                        //Debug.LogFormat("InitializeBattle :: Tile {0} has the name: {1}.", t.ToString(), tb.name);
                        //Debug.LogFormat("InitializeBattle :: Created data for tile {0}. Its movement cost is: {1}.", t.ToString(), movementCost);
#endif
                    }
                }
            }
        }

        /// <summary>
        /// Generate the tile neighbours.
        /// </summary>
        private void GenerateNeighbours(int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameTile tileToGenerateNeigboursFor = gameTileMapData.GetTile(x, y);
                    if (tileToGenerateNeigboursFor != null)
                        gameTileMapData.GenerateNeighboursFor(tileToGenerateNeigboursFor);
                }
            }
        }

        /// <summary>
        /// Generate relevant components for the players.
        /// </summary>
        private void GeneratePlayerComponents()
        {
            // Generate the AIController and set the AI needed for it
            AIController aiController = owner.gameObject.AddComponent<AIController>();
            var actionSetForAI = owner.ReadAIActionSet(owner.AIActionSetJsonFile);
            aiController.SetActionSet(actionSetForAI);
            aiController.Init(owner);
            owner.TurnController.SetAIController(aiController);

            // Generate the MouseController for the human player(s)
            int numPs = TurnController.Players.Length;
            for (int i = 0; i < numPs; i++)
            {
                if (TurnController.Players[i].PlayerType == PlayerType.NotAI)
                {
                    var mC = owner.gameObject.AddComponent<MouseController>();
                    mC.Initialize(TurnController.Players[i], owner, owner.SelectionController, owner.TheCamera);
                }
            }
        }

        /// <summary>
        /// Spawn units for the map. This should be called after the map and players have been generated.
        /// </summary>
        private void SpawnUnitsForMap()
        {
            // Spawn the units based on the level
            owner.VictoryController.SpawnLevelUnits(
                owner.TestPlayerUnitPrefab,
                owner.TestEnemyUnitPrefab,
                owner,
                owner.CurrentLevel
            );

#if UNITY_EDITOR
            Debug.Log("InitializeBattle :: Spawned units.");
#endif
        }
    }
}
