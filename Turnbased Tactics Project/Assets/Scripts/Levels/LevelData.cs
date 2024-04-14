using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TurnbasedGame.Levels
{
    /// <summary>
    /// Stores data for a level/map.
    /// </summary>
    public class LevelData
    {
        public string LocalizationName { get; private set; }

        [JsonProperty("ObjectiveType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ObjectiveTypes ObjectiveType { get; private set; }

        /// <summary>
        /// Something like what health value a target unit needs to get to in order to complete
        /// the level. Does nothing if the <see cref="ObjectiveType"/> is not relevant.
        /// </summary>
        public int TargetValue { get; private set; }

        /// <summary>
        /// Contains the spawn points for the player's units.
        /// </summary>
        public SpawnableData[] PlayerSpawnables { get; private set; }

        /// <summary>
        /// Contains the enemy units to spawn.
        /// </summary>
        public SpawnableData[] SpawnableDatas { get; private set; }

        public LevelData(string localizationName, ObjectiveTypes objectiveType, int targetValue, SpawnableData[] playerSpawnables, SpawnableData[] spawnableDatas)
        {
            this.LocalizationName = localizationName;
            this.ObjectiveType = objectiveType;
            this.TargetValue = targetValue;
            this.PlayerSpawnables = playerSpawnables;
            this.SpawnableDatas = spawnableDatas;
        }
    }
}
