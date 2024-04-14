using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace TurnbasedGame.Units
{
    /// <summary>
    /// Manages and keeps track of a <see cref="Unit"/>'s stats.
    /// </summary>
    [System.Serializable]
    public class CharacterStats
    {
        #region Delegates/Events
        /// <summary>
        /// An event that will get called when a unit's stat values have changed. For example, the player
        /// has told their unit to attack another unit and we need to display any potential changes in health.
        /// </summary>
        public delegate void StatsChanged(CharacterStats stats);
        public event StatsChanged OnStatsChanged;
        #endregion

        // TODO: Refactor these into an array based on stat type. One of the reasons why is that we don't want these variables changed directly.
        /// <summary>
        /// How much damage this unit can do.
        /// </summary>
        [SerializeField] private int strength = 8;
        [JsonProperty] public int Strength
        {
            get { return strength; }
            private set { strength = value; }
        }

        /// <summary>
        /// How many tiles away this unit must be in order to attack another unit.
        /// 1 means the unit has to be on a tile next to their target.
        /// </summary>
        [SerializeField] private int attackRange = 1;
        [JsonProperty] public int AttackRange 
        {
            get { return attackRange; }
            private set { attackRange = value; }
        }

        /// <summary>
        /// Subtracts damage.
        /// </summary>
        [SerializeField] private int defense = 2;
        [JsonProperty] public int Defense 
        {
            get { return defense; }
            private set { defense = value; }
        }

        /// <summary>
        /// Subtracts chance for an attacker to hit this unit.
        /// </summary>
        [SerializeField] private int evasion = 10;
        [JsonProperty] public int Evasion
        { 
            get { return evasion; }
            private set { evasion = value; }
        }

        [SerializeField] private int maxHP = 10;
        [JsonProperty] public int MaxHP 
        { 
            get { return maxHP; } 
            private set { maxHP = value; }
        }
        public int CurrentHP { get; set; }

        [SerializeField] private int maxMovementPoints = 4;
        [JsonProperty] public int MaxMovementPoints 
        { 
            get { return maxMovementPoints; }
            private set { maxMovementPoints = value; }
        }

        public int CurrentMovementPointsRemaining { get;  private set; }

        public int OwnerId { get; private set; }

        /// <summary>
        /// Default constructor that just initializes the stats based on the values set in the inspector.
        /// </summary>
        public CharacterStats()
        {
            CurrentMovementPointsRemaining = maxMovementPoints;
            Strength = strength;
            AttackRange = attackRange;
            Defense = defense;
            Evasion = evasion;
            CurrentHP = maxHP;
        }
        
        [JsonConstructor]
        public CharacterStats(int aRange, int str ,int def, int evd, int mHP, int mMvtPts)
        {
            AttackRange = aRange;
            Strength = str;
            Defense = def;
            Evasion = evd;
            MaxHP = mHP;
            CurrentHP = mHP;
            MaxMovementPoints = mMvtPts;
            CurrentMovementPointsRemaining = mMvtPts;
        }

        /// <summary>
        /// Copy the stat values of the passed <see cref="CharacterStats"/>. Also used to initialize a character's
        /// stats from a data file.
        /// </summary>
        /// <param name="statsToCopy">Stats to copy.</param>
        public void CopyStatValues(CharacterStats statsToCopy)
        {
            AttackRange = statsToCopy.AttackRange;
            Strength = statsToCopy.Strength;
            Defense = statsToCopy.Defense;
            Evasion = statsToCopy.Evasion;
            MaxHP = statsToCopy.MaxHP;
            CurrentHP = statsToCopy.MaxHP;
            MaxMovementPoints = statsToCopy.MaxMovementPoints;
            CurrentMovementPointsRemaining = statsToCopy.MaxMovementPoints;
        }

        public void SetOwnerId(int ownerId)
        {
            OwnerId = ownerId;
        }

        #region Stat Methods
        #region Stat Methods To Be Refactored Into One Method
        // TODO: Make a generalized function when the StatTypes array has been implemented.
        public void TakeDamage(int amount)
        {
            // Subtract damage based on our armor
            int damageRemaining = amount - Defense;

            // Prevent damage taken from being negative
            // TODO: Should we allow at least one damage to be taken?
            damageRemaining = Mathf.Clamp(damageRemaining, 0, int.MaxValue);

            // Subtract the defender's health based on the damage
            CurrentHP -= damageRemaining;

            // Clamp the health value
            CurrentHP = Mathf.Clamp(CurrentHP, 0, MaxHP);

#if UNITY_EDITOR
            Debug.LogFormat("CharacterStats :: A unit took {0} damage.", damageRemaining);
#endif

            // Tell any listeners that this unit's HP has changed
            if (OnStatsChanged != null)
            {
                OnStatsChanged(this);
            }
        }

        public void ModifyMovementPoints(int newAmount)
        {
            CurrentMovementPointsRemaining = newAmount;

            // Tell any listeners
            if (OnStatsChanged != null)
            {
                OnStatsChanged(this);
            }
        }

        public void Heal(int amountToHeal)
        {
            CurrentHP += amountToHeal;
            CurrentHP = Mathf.Clamp(CurrentHP, 1, MaxHP);

            // Tell any listeners that this unit's HP has changed
            if (OnStatsChanged != null)
            {
                OnStatsChanged(this);
            }
        }
        #endregion

        /// <summary>
        /// Reset the relevent stats for the new turn. I.e: movement points, etc.
        /// </summary>
        public void RefreshStatsForNewTurn()
        {
            CurrentMovementPointsRemaining = MaxMovementPoints;

            // Tell any listeners that this unit has been refreshed
            if (OnStatsChanged != null)
            {
                OnStatsChanged(this);
            }
        }
        #endregion
    }
}
