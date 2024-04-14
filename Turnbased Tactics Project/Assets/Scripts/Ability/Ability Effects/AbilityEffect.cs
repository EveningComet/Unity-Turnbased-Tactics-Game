using UnityEngine;
using TurnbasedGame.Units;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// A component that defines what an ability does.
    /// </summary>
    public abstract class AbilityEffect : ScriptableObject
    {
        /// <summary>
        /// Trigger this effect.
        /// </summary>
        /// <param name="activator">The activator of the <see cref="Ability"/> being passed. Not all <see cref="AbilityEffect"/>
        /// objects will need the activator, but let's make things easy for us and pass it anyway.</param>
        /// <param name="targetTile">The tile that will be effected.</param>
        public abstract void TriggerEffect(Unit activator, GameTile targetTile);

        /// <summary>
        /// Responsible for predicting how much damage to deal/heal. This value will be displayed in some kind of UI.
        /// </summary>
        /// <param name="activator">The <see cref="Unit"/> activating the <see cref="Ability"/>.</param>
        /// <param name="targetTile">The tile receiving the ability.</param>
        /// <remarks>
        /// NOTE: For <see cref="AbilityEffect"/>s that inflict a status effect, this should return 0,
        /// because those types are not changing the health.
        /// </remarks>
        public abstract int Predict(Unit activator, GameTile targetTile);
    }
}
