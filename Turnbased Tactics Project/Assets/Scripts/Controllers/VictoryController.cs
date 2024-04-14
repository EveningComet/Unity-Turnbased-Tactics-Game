using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Levels;
using TurnbasedGame.GamePlayerCode;
using TurnbasedGame.Units;

namespace TurnbasedGame.VictoryControl
{
    /// <summary>
    /// Keeps track of the current mission/objective for a map/level.
    /// </summary>
    public class VictoryController
    {
        /// <summary>
        /// The current mission/task.
        /// </summary>
        private WinLoseCondition objective;

        private List<Unit> levelUnits = new List<Unit>();

        /// <summary>
        /// Set the type of "mision" or what needs to be done based on the passed <see cref="LevelData"/>.
        /// Should be called after the level's units have been spawned.
        /// </summary>
        public void SetObjective(LevelData levelData, Player[] players)
        {
            switch(levelData.ObjectiveType)
            {
                case ObjectiveTypes.DefeatTarget:

                    DefeatTarget dTarget = new DefeatTarget(players);

                    // Get the needed target unit's stats
                    SpawnableData[] sds = levelData.SpawnableDatas;
                    int numSDs = sds.Length;
                    for (int i = 0; i < numSDs; i++)
                    {
                        if(sds[i].IsTarget == true)
                        {
                            // Since the level's units are the level's spawnable datas, they share the same index
                            dTarget.SetTargetStats(levelUnits[i].MyStats);
                            dTarget.SetTargetHP(levelData.TargetValue);
                            break;
                        }
                    }

                    objective = dTarget;
                    break;

                case ObjectiveTypes.DefeatAllEnemies:
                default:
                    DefeatAllEnemies dAE = new DefeatAllEnemies(players);
                    objective = dAE;
                    break;
            }

#if UNITY_EDITOR
            Debug.LogWarningFormat("VictoryController :: Objective type is: {0}.", objective.GetType());
#endif
        }

        /// <summary>
        /// Spawn the unit's belonging to the level. These are not necessarily the same
        /// as the player's units.
        /// </summary>
        public void SpawnLevelUnits(GameObject playerUnitPrefab, GameObject enemyUnitPrefab, BattleMapController bMC, LevelData levelData)
        {
            // Spawn the player's units
            SpawnableData[] psd = levelData.PlayerSpawnables;
            int numPSpawns = psd.Length;
            for (int i = 0; i < numPSpawns; i++)
            {
                var spawnedUnit = TurnbasedGame.Factories.UnitFactory.Create(playerUnitPrefab, psd[i], bMC);
                bMC.TurnController.GetCurrentPlayer().AddUnit(spawnedUnit);
                spawnedUnit.SetOwnerId(0);
            }

            // Spawn the enemy unit's
            SpawnableData[] sds = levelData.SpawnableDatas;
            int numSpawns = sds.Length;
            for (int i = 0; i < numSpawns; i++)
            {
                SpawnLevelUnit(enemyUnitPrefab, sds[i], bMC);
            }
        }

        private void SpawnLevelUnit(GameObject enemyUnitPrefab, SpawnableData spawnedData, BattleMapController bMC)
        {
            // TODO: Generalize this method to spawn units that will belong to anyone, not one.
            // Create the unit, set the owner, etc.
            Unit spawnedUnit = Factories.UnitFactory.Create(enemyUnitPrefab, spawnedData, bMC);

            // TODO: Better setting of the owner.
            bMC.TurnController.GetPlayerAtIndex(1).AddUnit(spawnedUnit);
            spawnedUnit.SetOwnerId(1);

            levelUnits.Add(spawnedUnit);
        }

        public bool IsGameOver()
        {
            return objective.IsGameOver == true;
        }

        /// <summary>
        /// Update the "state" of the current mission/objective/whatever.
        /// </summary>
        public void CheckIfBattleOver()
        {
            objective.CheckForGameOver();
        }
    }
}
