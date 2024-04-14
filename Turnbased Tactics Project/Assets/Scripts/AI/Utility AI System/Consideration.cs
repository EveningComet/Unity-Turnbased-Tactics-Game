using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TurnbasedGame.Tiles;
using TurnbasedGame.Abilities;

namespace TurnbasedGame.AI
{
    /// <summary>
    /// A thing that an <see cref="AIAction"/> must take into account. Stuff such as:
    /// "How far am I from my target?"
    /// "How much damage could I deal to an enemy?"
    /// </summary>
    /// <remarks>
    /// Anything passed in the <see cref="GetScore(AIContext)"/> method should never be modified.
    /// This class should only do stuff based on what is passed, not change what is passed.
    /// </remarks>
    public class Consideration
    {
        [Newtonsoft.Json.JsonProperty("inputAxis")]
        private InputAxis inputAxis;
        
        [Newtonsoft.Json.JsonProperty("curve")]
        private ResponseCurve curve;

        private const float zero = 0.0f;
        private const float one = 1.0f;

        [Newtonsoft.Json.JsonConstructor]
        public Consideration(InputAxis ia, ResponseCurve c)
        {
            this.inputAxis = ia;
            this.curve = c;
        }

        public float GetScore(AIContext context)
        {
            if(inputAxis.EndEarly(context) == true)
                return zero;

            // Get what we need
            float scoreToReturn = GetCalculatedValue(context);            

            // Clamp the input value and then calculate the curve
            scoreToReturn = Mathf.Clamp(scoreToReturn, zero, one); // TODO: Parameters for min and max range?
            scoreToReturn = curve.Calculate(scoreToReturn);

            // Return the curve, clamped
            return Mathf.Clamp(scoreToReturn, zero, one);
        }

        private float GetCalculatedValue(AIContext context)
        {
            switch (inputAxis.IAType)
            {
                case IATypes.DistanceBetweenUnits:
                    return GetDistanceBetweenUnits(context);

                case IATypes.ApproachTarget:
                    return CalculateForApproachingTarget(context);

                case IATypes.AvoidTarget:
                    return CalculateForAvoidingTarget(context);

                case IATypes.GetHealthOfTarget:
                    return GetNormarlized(
                        context.TargetUnit.MyStats.CurrentHP,
                        context.TargetUnit.MyStats.MaxHP
                    );

                case IATypes.AttackTarget:
                    return GetNormarlized(
                        context.CurrentUnit.MyStats.Strength,
                        context.TargetUnit.MyStats.CurrentHP
                    );

                case IATypes.ConsiderHealingOther:
                    return GetNormarlized(
                        context.TargetUnit.MyStats.MaxHP - context.TargetUnit.MyStats.CurrentHP,
                        context.TargetUnit.MyStats.MaxHP
                    );

                case IATypes.ConsiderUsingBuff:
                case IATypes.ConsiderUsingDebuff:
                case IATypes.ConsiderUsingDamageAbility:
                case IATypes.ConsiderHealingSelf:
                    return GetNormarlized(
                        GetAbilityOutput(context.ChosenAbility, context),
                        context.TargetUnit.MyStats.CurrentHP
                    );

                default:
                    return zero;
            }
        }

        #region Specific Calculations
        /// <summary>
        /// Get the distance, based on how far the current unit is from another unit.
        /// </summary>
        private float GetDistanceBetweenUnits(AIContext context)
        {
            float dist = GameTile.GetCubeDistance(context.CurrentUnit.CurrentTile, context.TargetUnit.CurrentTile);
            return context.CurrentUnit.MyStats.MaxMovementPoints / dist;
        }

        private float CalculateForApproachingTarget(AIContext context)
        {
            float dist = GameTile.GetCubeDistance(context.TargetUnit.CurrentTile, context.TargetTile);
            return context.CurrentUnit.MyStats.MaxMovementPoints / (dist + context.CurrentUnit.MyStats.MaxMovementPoints);
        }

        private float CalculateForAvoidingTarget(AIContext context)
        {
            float dist = GameTile.GetCubeDistance(context.TargetUnit.CurrentTile, context.TargetTile);
            return dist / context.TargetUnit.MyStats.MaxMovementPoints;
        }

        /// <summary>
        /// Go through an <see cref="Ability"/>'s <see cref="AbilityEffect"/>s
        /// and get their potential values.
        /// </summary>
        private int GetAbilityOutput(Ability a, AIContext context)
        {
            int sum = 0;
            var aEffects = a.effects;
            int numEffects = aEffects.Count;
            for (int i = 0; i < numEffects; i++)
            {
                sum += aEffects[i].Predict(context.CurrentUnit, context.TargetUnit.CurrentTile);
            }
            return sum;
        }

        private float GetNormarlized(float value1, float value2)
        {
            return value1 / value2;
        }
        #endregion

        #region Debug/Assistance
        public override string ToString()
        {
            return string.Format("Consideration type: {0}.\nCurve: {1}", inputAxis.IAType, curve.ToString());
        }
        #endregion
    }
}
