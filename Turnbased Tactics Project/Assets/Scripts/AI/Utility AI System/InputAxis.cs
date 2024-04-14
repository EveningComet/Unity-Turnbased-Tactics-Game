using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TurnbasedGame.AI
{
    /// <summary>
    /// Describes what a <see cref="Consideration"/> does.
    /// </summary>
    public struct InputAxis
    {
        /// <summary>
        /// What does this <see cref="InputAxis"/> need to get? (i.e: distance, target health, etc.)
        /// </summary>
        [JsonProperty("IAType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public IATypes IAType { get; private set; }
        
        public InputAxis(IATypes iaType)
        {
            this.IAType = iaType;
        }
        
        /// <summary>
        /// Should our <see cref="Consideration"/> end the calculation?
        /// Contains checks that will prevent the AI from doing stuff like healing their enemies.
        /// </summary>
        public bool EndEarly(AIContext context)
        {
            bool endEarly = false;
            switch (IAType)
            {
                case IATypes.DistanceBetweenUnits:
                    // Dissuade the AI from making a unit pathfind to itself
                    if (context.CurrentUnit.CurrentTile.Equals(context.TargetUnit.CurrentTile))
                        endEarly = true;

                    break;

                case IATypes.ApproachTarget:
                    // Dissuade the AI from making a unit approach itself
                    if (context.CurrentUnit.CurrentTile.Equals(context.TargetTile) == true)
                        endEarly = true;
                    break;

                case IATypes.AvoidTarget:
                    // Dissuade the AI from making a unit avoid itself
                    // TODO: What about avoiding pals and allies?
                    if (context.CurrentUnit.CurrentTile.Equals(context.TargetTile) == true)
                        endEarly = true;
                    break;

                case IATypes.AttackTarget:
                    // Dissuade the AI from attacking a friend
                    if (context.CurrentUnit.OwnerId == context.TargetUnit.OwnerId)
                        endEarly = true;
                    break;

                #region Ability IATypes
                case IATypes.ConsiderUsingBuff:
                    // Dissuade the AI from buffing enemies
                    // TODO: What about units of friends?
                    if (context.CurrentUnit.OwnerId != context.TargetUnit.OwnerId)
                        endEarly = true;
                    break;

                case IATypes.ConsiderUsingDebuff:
                    // Dissuade the AI from debuffing its own units
                    if (context.CurrentUnit.OwnerId == context.TargetUnit.OwnerId)
                        endEarly = true;
                    break;

                case IATypes.ConsiderUsingDamageAbility:
                    // Dissuade the AI from attacking their pals
                    if (context.CurrentUnit.OwnerId == context.TargetUnit.OwnerId ||
                        context.ChosenAbility.abilityType != Abilities.AbilityTypes.Damage)
                        endEarly = true;

                    break;

                case IATypes.ConsiderHealingOther:
                    /* Dissuade the AI from healing their enemies, using an ability that won't heal,
                     * or any healing ability that only targets the user */
                    // TODO: What about allies?
                    if (context.TargetUnit.OwnerId != context.CurrentUnit.OwnerId || 
                        context.ChosenAbility.abilityType != Abilities.AbilityTypes.Heal ||
                        context.ChosenAbility.targetingType == Abilities.AbilityTargetingTypes.Self)
                        endEarly = true;
                    
                    break;

                case IATypes.ConsiderHealingSelf:
                    // Don't use an ability that will hurt ourselves
                    if (context.ChosenAbility.abilityType != Abilities.AbilityTypes.Heal)
                        endEarly = true;
                    
                    break;
                #endregion

                default:
                    break;
            }
            return endEarly;
        }
    }
}
