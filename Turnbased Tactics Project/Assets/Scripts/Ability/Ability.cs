using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.UI.Tooltips;
using TurnbasedGame.Units;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Something that a <see cref="Unit"/> can activate on themselves.
    /// </summary>
    [CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
    public class Ability : ScriptableObject, ITooltipable
    {
        [SerializeField] private string localizationName = "New Ability";

        /// <summary>
        /// The <see cref="Ability"/>'s description.
        /// </summary>
        [SerializeField, TextArea]
        private string description = "";

        public Sprite abilityIcon = null;

        /// <summary>
        /// Specifies what targets, if any, around a target tile that should also be targeted
        /// by this ability's <see cref="AbilityEffect"/>s.
        /// </summary>
        [Tooltip("Specifies what targets, if any, around a target tile that should also be targeted by this" +
            "ability's effects. By default, an ability should use the SingleAbilityArea component.")]
        public AbilityAOE aAOE = null;

        /// <summary>
        /// The component for managing the success/failure rate.
        /// </summary>
        [Tooltip("The component for managing the success/failure rate.")]
        public HitSuccessRate hitSuccessRate = null;

        /// <summary>
        /// The effects tied to this ability.
        /// </summary>
        public List<AbilityEffect> effects = new List<AbilityEffect>();

        /// <summary>
        /// How this ability gets its targets.
        /// </summary>
        public AbilityTargetingLogic abilityTargetingLogic = null;

        [Header("AI Helper Tags & Organization")]
        /// <summary>
        /// Tag for an <see cref="Ability"/> that describes what kind of targeting it does. Helps the AI
        /// make decisions.
        /// </summary>
        [Tooltip("Tag for an ability that describes what kind of targeting it does. Helps the AI make decisions.")]
        public AbilityTargetingTypes targetingType;

        /// <summary>
        /// Tag that describes what the <see cref="Ability"/> does. Useful for helping the AI make decisions.
        /// </summary>
        [Tooltip("Tag that describes what the ability does. Useful for helping the AI make decisions.")]
        public AbilityTypes abilityType;

        /// <summary>
        /// Get the targets based on the attached <see cref="AbilityTargetingLogic"/> component.
        /// </summary>
        /// <param name="activator">Unit activating the ability.</param>
        /// <param name="gameTileMapData">Object storing the map.</param>
        public List<GameTile> GetTargets(Unit activator, GameTileMapData gameTileMapData)
        {
            return abilityTargetingLogic.GetTiles(activator, gameTileMapData);
        }

        public void TriggerAbility(Unit activator, GameTile targetTile)
        {
            int numEffects = effects.Count;
            for (int i = 0; i < numEffects; i++)
            {
                // Roll for chance to succeed
                AbilityEffect currentEffect = effects[i];
                
                // TODO: Each effect will need a hitrate.
                if(hitSuccessRate.RollForSuccess(targetTile) == true)
                {
                    currentEffect.TriggerEffect(activator, targetTile);
                }
            }
        }

        public void TriggerAbility(Unit activator, List<GameTile> targetTiles)
        {
            int numTargets = targetTiles.Count;
            for (int i = 0; i < numTargets; i++)
            {
                TriggerAbility(activator, targetTiles[i]);
            }
        }

        #region ITooltipable Implementation
        public string GetTitle()
        {
            return localizationName;
        }

        public string GetDescription()
        {
            return description;
        }
        #endregion
    }
}