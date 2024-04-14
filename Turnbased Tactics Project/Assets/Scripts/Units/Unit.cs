using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Tiles;
using TurnbasedGame.Abilities;
using System.Linq;

namespace TurnbasedGame.Units
{
    /// <summary>
    /// A character that exists in the game world.
    /// </summary>
    [RequireComponent(typeof(UnitView))]
    public class Unit : MonoBehaviour
    {
        #region Unit/Character Stats/Data
        [SerializeField] private CharacterStats myStats = null;
        public CharacterStats MyStats { get { return myStats; } }

        /// <summary>
        /// The abilities belonging to this <see cref="Unit"/>.
        /// </summary>
        [SerializeField] private AbilityHolder myAbilities = null;
        public AbilityHolder MyAbilities { get { return myAbilities; } }
        #endregion

        #region Non-data variables
        [SerializeField] private UnitView unitView = null;
        public UnitView UnitView { get { return unitView; } }
        /// <summary>
        /// Stores the current tile for this <see cref="Unit"/>.
        /// </summary>
        public GameTile CurrentTile { get; private set; }

        // TODO: Should this be considered a stat or data?
        /// <summary>
        /// The id belonging to the player in charge of this unit.
        /// </summary>
        public int OwnerId { get; private set; }

        /// <summary>
        /// Has this unit already attacked or used an ability this turn?
        /// </summary>
        public bool HasAlreadyTookActionThisTurn { get; private set; }

        /// <summary>
        /// Stores the tile path for this unit. The first tile is the current tile.
        /// </summary>
        private List<GameTile> myPath = new List<GameTile>();
        #endregion

        #region Delegates/Events
        public delegate void UnitDestroyed(Unit unitThatHasBeenDestroyed);
        public event UnitDestroyed OnUnitDestroyed;

        public delegate void UnitHasTookAction(Unit unitThatHasTookAction);
        /// <summary>
        /// Event used to tell things when this unit has taken an action.
        /// </summary>
        public event UnitHasTookAction OnUnitHasTakenAction;
        #endregion

        public void SetOwnerId(int newId)
        {
            this.OwnerId = newId;
        }

        public void SetTile(GameTile newTile)
        {
            // Remove ourself from our tile if we already have one
            if(CurrentTile != null)
            {
                CurrentTile.RemoveUnit(this);
            }

            CurrentTile = newTile;

            // Add ourself to the new tile
            CurrentTile.AddUnit(this);
#if UNITY_EDITOR
            Debug.LogFormat("Unit :: {0} has a new current tile: {1}.", gameObject.name, newTile.ToString());
#endif
        }

        public void SetPath(Stack<GameTile> newPath)
        {
            myPath = newPath.ToList();
        }

        public float GetCostToPathfindTo(GameTile tileToCheck)
        {
            /* TODO: When we have unit traits/skills that allow them to do stuff like "forrests cost only 1 movement point for us",
             * we're going to need a method or something to check for that. */
            float finalCost = tileToCheck.MovementCost;
            return finalCost;
        }

        /// <summary>
        /// Returns true or false whether or not this unit is succesfully able to move.
        /// </summary>
        public bool AbleToMove()
        {
            // Stop when no longer able to move or there is no longer a path
            if(myStats.CurrentMovementPointsRemaining <= 0 || myPath.Count == 0)
            {
                return false;
            }

            GameTile newTile = myPath[1];

            int costToEnter = newTile.MovementCost;

            // We can't enter the hex this turn
            if (costToEnter > myStats.CurrentMovementPointsRemaining && myStats.CurrentMovementPointsRemaining < myStats.MaxMovementPoints)
            {
                return false;
            }

            // We're able to move at this point
            myPath.RemoveAt(0);

            /* The only tile we have left in the list, is the one we're moving to now, 
             * therefore we have no more path to follow. 
             * So let's just clear the queue completely to avoid confusion. */
            if (myPath.Count == 1)
            {
                myPath.Clear();
            }

            SetTile( newTile );

            // This lowers our movement amount. Return the highest amount we can move.
            myStats.ModifyMovementPoints( Mathf.Max(myStats.CurrentMovementPointsRemaining - costToEnter, 0) );

            return true;
        }

        /// <summary>
        /// Refresh certain unit values for the new turn.
        /// </summary>
        public void RefreshForNewTurn()
        {
            myStats.RefreshStatsForNewTurn();
            HasAlreadyTookActionThisTurn = false;

            if (OnUnitHasTakenAction != null)
                OnUnitHasTakenAction(this);
        }

        #region Combat Related Methods
        public void ConsumeAction()
        {
            HasAlreadyTookActionThisTurn = true;

            if (OnUnitHasTakenAction != null)
                OnUnitHasTakenAction(this);
        }

        /* It may be desired to put these methods into some kind of static class. */
        /// <summary>
        /// Used to display the chance for an attacker to hit the defender.
        /// </summary>
        public int GetChanceToHitToDisplay(Unit defender)
        {
            return 100 - defender.MyStats.Evasion;
        }

        public int CalculateHitChance()
        {
            // TODO: Currently like this so that the chance to hit can be displayed in the console. Just return the random value when we know things are working.
            int chance = UnityEngine.Random.Range(0, 101);
#if UNITY_EDITOR
            Debug.LogFormat("Unit :: For a normal attack, {0} got {1} from their chance to hit.", gameObject.name, chance);
#endif
            return chance;
        }

        public void Die()
        {
#if UNITY_EDITOR
            Debug.LogFormat("Unit :: {0} is kill.", gameObject.name);
#endif
            // Remove the unit from its tile
            CurrentTile.RemoveUnit(this);

            // Tell the owner of this unit that this unit has died
            if(OnUnitDestroyed != null)
            {
                OnUnitDestroyed(this);
            }

            Destroy(gameObject);
        }

        public bool IsDead()
        {
            return myStats.CurrentHP <= 0;
        }
        #endregion

        #region Ability Methods
        /// <summary>
        /// Have this <see cref="Unit"/> activate the passed ability (if they own it).
        /// </summary>
        /// <param name="abilityToActivate">The <see cref="Ability"/> to activate.</param>
        /// <param name="targetTiles">The tiles that will be targeted.</param>
        public void TriggerAbility(Ability abilityToActivate, List<GameTile> targetTiles)
        {
            if (HasAlreadyTookActionThisTurn == true)
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat("Unit :: {0} is trying to activate an ability when they have already taken an action for their turn. Just going to stop them.", this.gameObject.name);
                return;
#endif
            }

            if(myAbilities.Contains(abilityToActivate) == false)
            {
#if UNITY_EDITOR
                Debug.LogErrorFormat("Unit :: {0} is trying to use the ability {1}, which it does not own!", gameObject.name, abilityToActivate.GetTitle());
#endif
                return;
            }

            ConsumeAction();
            abilityToActivate.TriggerAbility(this, targetTiles);
#if UNITY_EDITOR
            Debug.LogFormat("Unit :: {0} is using ability ({1}).",
                gameObject.name,
                abilityToActivate.GetTitle()
            );
#endif
        }
        #endregion
    }
}
