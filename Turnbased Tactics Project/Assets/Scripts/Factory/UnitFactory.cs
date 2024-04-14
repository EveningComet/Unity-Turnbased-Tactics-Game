using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TurnbasedGame.Levels;

namespace TurnbasedGame.Factories
{
    /// <summary>
    /// Responsible for creating units.
    /// </summary>
    public static class UnitFactory
    {
        /// <summary>
        /// Create a <see cref="Unit"/> and place it in the game world.
        /// </summary>
        /// <param name="prefab">Prefab.</param>
        /// <param name="spawnedData">Holds where and what to spawn the <see cref="Unit"/> with.</param>
        /// <param name="bMC">Used to spawn place the unit's position.</param>
        /// <returns>A spawned unit in the game world.</returns>
        public static Unit Create(GameObject prefab, SpawnableData spawnedData, BattleMapController bMC)
        {
            GameObject spawnedUnitGO = GameObject.Instantiate(prefab);
            var spawnedUnit = spawnedUnitGO.GetComponent<Unit>();

            spawnedUnit.transform.position = bMC.GetTileWorldSpaceCenter(spawnedData.TileX, spawnedData.TileY);
            spawnedUnit.SetTile(
                bMC.GetTileMapData().GetTile(spawnedData.TileX, spawnedData.TileY)
            );

            if (spawnedData.CharacterStats != null)
                spawnedUnit.MyStats.CopyStatValues(spawnedData.CharacterStats);

            return spawnedUnit;
        }

        /// <summary>
        /// Create a unit with default stats, at the specified location.
        /// </summary>
        /// <returns>A unit with default stats, at the specified location.</returns>
        public static Unit Create(GameObject prefab, BattleMapController bMC, int tileX, int tileY)
        {
            GameObject spawnedUnitGO = GameObject.Instantiate(prefab);
            var spawnedUnit = spawnedUnitGO.GetComponent<Unit>();

            spawnedUnit.transform.position = bMC.GetTileWorldSpaceCenter(tileX, tileY);
            spawnedUnit.SetTile(
                bMC.GetTileMapData().GetTile(tileX, tileY)
            );

            return spawnedUnit;
        }
    }
}
