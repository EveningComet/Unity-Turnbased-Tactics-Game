using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TurnbasedGame.AI;
using TurnbasedGame.Levels;

namespace TurnbasedGame.ReadWrite
{
    /// <summary>
    /// Responsible for reading and writing Json files for the game.
    /// </summary>
    public static class JsonReadWriter
    {
        #region Level Reading
        public static LevelData ReadLevel(TextAsset levelData)
        {
            return ReadLevel(levelData.text);
        }

        public static LevelData ReadLevel(string levelData)
        {
            // Ignore any null values
            var settings = new JsonSerializerSettings { 
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            LevelData lD = JsonConvert.DeserializeObject<LevelData>(
                levelData,
                settings
            );
#if UNITY_EDITOR
            Debug.LogFormat("JsonReadWriter :: Read level: {0}.", lD.LocalizationName);
#endif
            return lD;
        }
        #endregion
        #region AIActionSet Methods
        /// <summary>
        /// Read the passed text asset (a json file) and return the <see cref="AIActionSet"/>(s) contained within.
        /// </summary>
        public static AIActionSet ReadAIActionSetJsonFile(TextAsset textAsset)
        {
            return ReadAIActionSetJsonFile(textAsset.text);
        }

        /// <summary>
        /// Read the passed string (from a json file) and return the <see cref="AIActionSet"/>(s) contained within.
        /// </summary>
        public static AIActionSet ReadAIActionSetJsonFile(string fileText)
        {
            
            AIActionSet actionSet = JsonConvert.DeserializeObject<AIActionSet>(fileText);

#if UNITY_EDITOR
            Debug.LogFormat("JsonReadWriter :: Name of AIActionSet Object: {0}. Number of actions to consider: {1}" +
                "\nPrinting an action's consideration to the console to make sure we can see if it's good.\n{2}\n.",
                actionSet.DevelopmentName,
                actionSet.Actions.Length,
                actionSet.Actions[0].Considerations[0].ToString()
            );
#endif
            return actionSet;
        }

        #region Complex Breakdown
        //        public static AIActionSet BreakdownAndReturnComplexAIActionSet(string fileText)
        //        {
        //            var actionSetAsJObject = JObject.Parse(fileText);
        //            string developmentName = (string)actionSetAsJObject["DevelopmentName"];

        //            // Because the AIActionSet object is a bit complex, we need to break it down
        //            // Get the needed actions
        //            JToken actionsAsJToken = actionSetAsJObject["Actions"];
        //            List<AIAction> actions = new List<AIAction>();
        //            List<Consideration> considerations = new List<Consideration>();
        //            foreach (var actionToken in actionsAsJToken)
        //            {
        //                // Read the type of the action
        //                AIActionTypes actionType = GetActionType(actionToken["ActionType"].ToString());

        //                // Read the weight modifier
        //                float weightModifier = (float)actionToken["WeightModifier"];

        //                // Read the MovementHelper
        //                JToken movementHelperToken = actionToken["MovementHelper"];
        //                MovementHelper movementHelper = JsonConvert.DeserializeObject<MovementHelper>(movementHelperToken.ToString());

        //                // Read the considerations
        //                var considerationsAsToken = actionToken["Considerations"];
        //                foreach (var considerationToken in considerationsAsToken)
        //                {

        //                }

        //                // Create the AIAction
        //                AIAction action = new AIAction(actionType, movementHelper, considerations.ToArray(), weightModifier);

        //                actions.Add(action);
        //                considerations.Clear();
        //            }

        //            AIActionSet actionSet = new AIActionSet(developmentName, actions.ToArray());

        //#if UNITY_EDITOR
        //            Debug.LogFormat("JsonReadWriter :: Name of AIActionSet Object: {0}.", actionSet.DevelopmentName);
        //            Debug.LogFormat("JsonReadWriter :: Number of actions to consider: {0}.", actionSet.Actions.Length);
        //            Debug.LogFormat("JsonReadWriter :: Weight of some action is: {0}.",
        //                actionSet.Actions[UnityEngine.Random.Range(0, actionSet.Actions.Length)].WeightModifier);
        //            Debug.LogFormat("JsonReadWriter :: {0}.", actionSet.Actions[0].Considerations[0].ToString());
        //#endif

        //            return actionSet;
        //        }

        //        private static AIActionTypes GetActionType(string jsonString)
        //        {
        //            AIActionTypes returnType = AIActionTypes.MoveToAndAttack;
        //            switch (jsonString)
        //            {
        //                case "MoveToAndAttack":
        //                    returnType = AIActionTypes.MoveToAndAttack;
        //                    break;
        //                case "UseDamagingAbility":
        //                    returnType = AIActionTypes.UseDamagingAbility;
        //                    break;
        //            }
        //            return returnType;
        //        }
        #endregion
        #endregion
    }
}
