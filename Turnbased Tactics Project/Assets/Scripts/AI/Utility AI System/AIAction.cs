using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Units;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TurnbasedGame.AI
{
    /// <summary>
    /// Holds what a character should do.
    /// </summary>
    public class AIAction
    {
        /// <summary>
        /// Some <see cref="AIAction"/>s are more important than others.
        /// </summary>
        public float WeightModifier { get; private set; }

        [JsonProperty("ActionType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AIActionTypes ActionType { get; private set; }

        /// <summary>
        /// Helps choose where a <see cref="Unit"/> should move, if this <see cref="AIAction"/> is decided to be used.
        /// </summary>
        public MovementHelper MovementHelper { get; private set; }

        /// <summary>
        /// The things this <see cref="AIAction"/> needs to take into account. Things such as:
        /// "How far away am I from the target?"
        /// "Am I able to heal a friend this turn?"
        /// </summary>
        public Consideration[] Considerations { get; private set; }

        public AIAction(AIActionTypes actionType, Consideration[] considerations)
        {
            this.ActionType = actionType;
            this.Considerations = considerations;
            WeightModifier = 1.0f;
        }

        [JsonConstructor]
        public AIAction(AIActionTypes actionType, MovementHelper movementHelper, Consideration[] considerations, float weightModifier)
        {
            this.ActionType = actionType;
            this.MovementHelper = movementHelper;
            this.Considerations = considerations;
            this.WeightModifier = weightModifier;
        }

        /// <summary>
        /// Go through this <see cref="AIAction"/>'s <see cref="Consideration"/>s and get a score.
        /// </summary>
        public float GetFinalScore(AIContext context)
        {
            float finalScore = WeightModifier;
            int numConsiderations = Considerations.Length;
            float modifactionFactor = 1f - (1f / (float)numConsiderations);
            for (int i = 0; i < numConsiderations; i++)
            {
                // Get the score and then compensate it based on the number of considerations
                float score = Considerations[i].GetScore(context);
                float makeUpValue = (1f - score) * modifactionFactor;
                finalScore *= score + (makeUpValue * score);

                // If the value is 0, stop considering, because this action is not worth calculating
                if(finalScore <= 0f)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogWarningFormat("AIAction :: Consideration score was 0 for {0} on their action, {1}. " +
                        "Their target would have been {2}. Bailing early.\n{3}.",
                        context.CurrentUnit.gameObject.name,
                        ActionType.ToString(),
                        context.TargetUnit.gameObject.name,
                        Considerations[i].ToString()
                    );
#endif
                    finalScore = 0f;
                    break;
                }
            }
            return finalScore;
        }
    }
}
