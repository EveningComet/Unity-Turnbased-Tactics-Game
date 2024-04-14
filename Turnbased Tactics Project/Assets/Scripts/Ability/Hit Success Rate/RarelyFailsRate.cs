using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Tiles;

namespace TurnbasedGame.Abilities
{
    /// <summary>
    /// Use for when failing is very rare.
    /// </summary>
    [CreateAssetMenu(fileName = "New RarelyFailsRate", menuName = "HitSuccessRates/RarelyFailsRate")]
    public class RarelyFailsRate : HitSuccessRate
    {
        public override int Calculate(GameTile targetTile)
        {
            return Final(0);
        }
    }
}
