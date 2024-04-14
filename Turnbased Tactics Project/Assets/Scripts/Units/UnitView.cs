using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Abilities;

namespace TurnbasedGame.Units
{
    /// <summary>
    /// Responsible for handling a <see cref="Unit"/>'s visuals.
    /// </summary>
    public class UnitView : MonoBehaviour
    {
        #region Delegates/Events
        /// <summary>
        /// Really just used to help the AI execute a decided unit action.
        /// </summary>
        public delegate void UnitFinishedMoving(Unit u);
        public event UnitFinishedMoving OnUnitFinishedMoving;

        /// <summary>
        /// Used to tell the AI it's time to actually deal damage/heal/etc.
        /// </summary>
        public delegate void UnitFinishedAnimation(Unit u);
        public event UnitFinishedAnimation OnUnitFinishedAnimation;
        #endregion

        [SerializeField] private float movementSpeed = 2f;
        private float waitForFinishedMovementTime = 0.5f;

        /// <summary>
        /// Visual generic attack speed.
        /// </summary>
        private float nudgeSpeed = 3f;
        private float waitForFinishedNudgeTime = 0.1f;

        private bool isTransitioning = false;
        public bool IsTransitioning { get { return isTransitioning; } }

        private Unit myUnit;

        private void Start()
        {
            myUnit = GetComponent<Unit>();
        }

        /// <summary>
        /// Make the <see cref="Unit"/> move through its pathfound tiles.
        /// </summary>
        public void MoveUnit()
        {
            if (isTransitioning == true)
                return;

#if UNITY_EDITOR
            Debug.LogFormat("UnitView :: Called to move for {0}.", gameObject.name);
#endif

            StartCoroutine( TraverseTiles() );
        }

        private IEnumerator TraverseTiles()
        {
            // Set the transitioning here because we want this coroutine to finish first
            isTransitioning = true;
            while (myUnit.AbleToMove() == true)
            {
                var newTilePos = myUnit.CurrentTile.WorldSpaceCenter;
                yield return MoveUnitToNewTile(newTilePos);
            }
            yield return new WaitForSeconds(waitForFinishedMovementTime);
            isTransitioning = false;

            if (OnUnitFinishedMoving != null)
                OnUnitFinishedMoving.Invoke(myUnit);
        }

        private IEnumerator MoveUnitToNewTile(Vector3 newTilePosition)
        {
            while(transform.position != newTilePosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, newTilePosition, Time.deltaTime * movementSpeed);
                yield return 0;
            }
        }

        #region  Action Visual Methods
        public void Perform(Vector3 targetPos)
        {
            StartCoroutine( PerformAction(targetPos) );
        }

        public void Perform()
        {
            StartCoroutine( PerformAction() );
        }

        private IEnumerator PerformAction()
        {
            yield return 0;

            if (OnUnitFinishedAnimation != null)
                OnUnitFinishedAnimation.Invoke(this.myUnit);
        }

        private IEnumerator PerformAction(Vector3 targetPos)
        {
            yield return Nudge(targetPos);

            if (OnUnitFinishedAnimation != null)
                OnUnitFinishedAnimation.Invoke(this.myUnit);
        }

        private IEnumerator Nudge(Vector3 targetPos)
        {
            // Grab our original position
            Vector3 originalPos = transform.localPosition;

            // "Nudge" towards the target
            while(transform.localPosition != targetPos)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, targetPos, Time.deltaTime * nudgeSpeed);
                yield return 0;
            }

            // Return to our original position
            while(transform.localPosition != originalPos)
            {
                transform.localPosition = Vector2.MoveTowards(transform.localPosition, originalPos, Time.deltaTime * nudgeSpeed);
                yield return 0;
            }

            // Wait a tiny bit
            yield return new WaitForSeconds(waitForFinishedNudgeTime);
        }

        private IEnumerator PerformAction(Ability abilityToPerform)
        {
            // TODO: Play animation.
            yield return 0;
        }
        #endregion
    }
}
