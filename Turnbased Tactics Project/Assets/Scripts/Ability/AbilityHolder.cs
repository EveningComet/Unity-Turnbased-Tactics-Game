using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Stores <see cref="Ability"/> objects for a <see cref="Units.Unit"/>.
    /// </summary>
    [System.Serializable]
    public class AbilityHolder
    {
        // TODO: We might want this to be a dictionary.
        [SerializeField] private List<Ability> heldAbilities = new List<Ability>();
        public List<Ability> HeldAbilities { get { return heldAbilities; } }

        public AbilityHolder()
        {
        }

        public AbilityHolder(List<Ability> abilities)
        {
            heldAbilities = abilities;
        }

        public void AddAbility(Ability newAbility)
        {
            if (heldAbilities.Contains(newAbility) == true)
                return;

            heldAbilities.Add(newAbility);
        }

        public void RemoveAbility(Ability abilityToRemove)
        {
            heldAbilities.Remove(abilityToRemove);
        }

        /// <summary>
        /// Are we holding the passed ability?
        /// </summary>
        public bool Contains(Ability a)
        {
            return heldAbilities.Contains(a);
        }
    }
}