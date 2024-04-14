using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;

namespace TurnbasedGame.AI
{
    /// <summary>
    /// Stores a container of <see cref="AIAction"/>s.
    /// </summary>
    public class AIActionSet
    {
        public string DevelopmentName { get; private set; }

        public AIAction[] Actions { get; private set; }

        public AIActionSet(string developmentName, AIAction[] actions)
        {
            this.DevelopmentName = developmentName;
            this.Actions = actions;
        }

        /// <summary>
        /// Return a list of <see cref="ContextScore"/>s based on the stored <see cref="AIAction"/>s.
        /// </summary>
        public List<ContextScore> GetContextScores(Unit unitToDecideFor, Unit targetUnit)
        {
            List<ContextScore> contextScoresToReturn = new List<ContextScore>();
            int numActions = Actions.Length;
            for (int i = 0; i < numActions; i++)
            {
                AIAction currentAction = Actions[i];
                switch(currentAction.ActionType)
                {
                    case AIActionTypes.UseDamagingAbility:
                    case AIActionTypes.UseBuffAbility:
                    case AIActionTypes.UseDebuffAbility:
                    case AIActionTypes.UseHealingAbility:
                        var abilities = unitToDecideFor.MyAbilities.HeldAbilities;
                        int numAbilities = abilities.Count;
                        for (int j = 0; j < numAbilities; j++)
                        {
                            var abilityToCheck = abilities[j];
                            AIContext newContextWithAbility = new AIContext
                            {
                                CurrentUnit = unitToDecideFor,
                                TargetUnit = targetUnit,
                                ChosenAction = currentAction,
                                ChosenAbility = abilityToCheck
                            };

                            float scoreForContextWithAbility = currentAction.GetFinalScore(newContextWithAbility);
                            ContextScore scoredContextWithAbility = new ContextScore
                            {
                                Score = scoreForContextWithAbility,
                                StoredContext = newContextWithAbility
                            };

                            contextScoresToReturn.Add(scoredContextWithAbility);
                        }
                        break;

                    // Otherwise, we don't have to check for any abilities
                    default:
                        AIContext newContext = new AIContext
                        {
                            CurrentUnit = unitToDecideFor,
                            TargetUnit = targetUnit,
                            ChosenAction = currentAction
                        };

                        float score = currentAction.GetFinalScore(newContext);
                        ContextScore scoredContext = new ContextScore
                        {
                            Score = score,
                            StoredContext = newContext
                        };

                        contextScoresToReturn.Add(scoredContext);
                        break;
                }
            }

            return contextScoresToReturn;
        }
    }
}
