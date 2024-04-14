using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.StateMachine;
using TurnbasedGame.Units;
using TurnbasedGame.GamePlayerCode;
using TurnbasedGame.AI;

/// <summary>
/// Responsible for taking an AI <see cref="Player"/> and deciding what to do with their <see cref="Unit"/>s.
/// </summary>
/// <remarks>
/// This class does not directly do anything. It's <see cref="AIState"/>s actually do most of the work.
/// </remarks>
public class AIController : StateMachine<AIState>
{
    public bool AIHasFinishedTurn { get; private set; }
    public Unit CurrentUnitProcessingFor { get; private set; }
    public Player[] Players { get; private set; }
    public AIActionSet AIActionSet { get; private set; }

    /// <summary>
    /// Stores what the current unit being processed should do. This should
    /// get set through <see cref="GetActionForUnitState"/>.
    /// </summary>
    public AIContext ChosenContext { get; set; }
    public BattleMapController BMC { get; private set; }

    private int unitsLeftToProcess;
    private bool mayProcessAgain = false;

    public void Init(BattleMapController battleMapController)
    {
        BMC = battleMapController;
        AIHasFinishedTurn = false;
        Players = BMC.TurnController.Players;
        ChangeToState(new AIIdleState(this));
    }

    public void SetActionSet(AIActionSet newSet)
    {
        AIActionSet = newSet;
    }

    public void BeginProcess(Player aiPlayer, Player[] players)
    {
        AIHasFinishedTurn = false;
        StartCoroutine(Process(aiPlayer, players));
    }

    private IEnumerator Process(Player aiPlayer, Player[] players)
    {
        // Loop through our units
        var ourUnits = aiPlayer.GetUnits();
        int numOfOurUnits = ourUnits.Length;
        unitsLeftToProcess = numOfOurUnits;
        for (int i = 0; i < numOfOurUnits; i++)
        {
            // Store the current unit we're deciding for
            CurrentUnitProcessingFor = ourUnits[i];
            ChangeToState(new GetActionForUnitState(this));
            yield return new WaitUntil(() => mayProcessAgain == true);
        }
    }

    /// <summary>
    /// After finishing a unit's action (i.e: attacking, healing, etc.), see what we should do next.
    /// </summary>
    public void FinishActionExecution()
    {
        unitsLeftToProcess -= 1;

        // Check if we should end the turn
        BMC.VictoryController.CheckIfBattleOver();
        if (BMC.VictoryController.IsGameOver() == true)
        {
            BMC.EndBattle();
            EndTurn();
        }

        // End our turn
        else if (unitsLeftToProcess == 0)
        {
            EndTurn();
        }

        // Otherwise, we still have units left to process
        else
        {
            CleanUpValues();
            mayProcessAgain = true;
        }
    }

    public void CleanUpValues()
    {
        mayProcessAgain = false;
        CurrentUnitProcessingFor = null;
        ChosenContext = null;
    }

    private void EndTurn()
    {
        CleanUpValues();
        StopAllCoroutines();
        AIHasFinishedTurn = true;
        ChangeToState(new AIIdleState(this));
        BMC.TurnController.EndSubTurn();
    }

    #region AI State Machine Methods
    public override void ChangeToState(State newState)
    {
        // Do the normal state changing things
        base.ChangeToState(newState);

        // Now do this version's specific things
        currentState.Process();
    }
    #endregion
}
