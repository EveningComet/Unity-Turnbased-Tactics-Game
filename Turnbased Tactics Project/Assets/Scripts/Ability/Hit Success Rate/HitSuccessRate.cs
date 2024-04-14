using UnityEngine;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// An component for an <see cref="Ability"/> that is reponsible
    /// for managing the success/failure rate.
    /// </summary>
    public abstract class HitSuccessRate : ScriptableObject
    {
        /// <summary>
        /// Get the success rate.
        /// </summary>
        public abstract int Calculate(GameTile targetTile);

        public bool RollForSuccess(GameTile targetTile)
        {
            int chance = UnityEngine.Random.Range(0, 101);
            int amount = Calculate(targetTile);
            return chance <= amount;
        }

        protected int Final(int value)
        {
            return 100 - value;
        }
    }
}
