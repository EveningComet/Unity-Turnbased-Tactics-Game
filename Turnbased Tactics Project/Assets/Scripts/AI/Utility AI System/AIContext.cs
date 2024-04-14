using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Tiles;
using TurnbasedGame.Units;
using TurnbasedGame.Abilities;

namespace TurnbasedGame.AI
{
    /// <summary>
    /// This holds on to information that the AI will use to make a decision.
    /// Abstraction layer between the game code and AI code.
    /// </summary>
    public class AIContext
    {
        public Unit CurrentUnit { get; set; }
        public Unit TargetUnit { get; set; }
        public GameTile TargetTile { get; set; }
        public AIAction ChosenAction { get; set; }
        public Ability ChosenAbility { get; set; }
    }
}
