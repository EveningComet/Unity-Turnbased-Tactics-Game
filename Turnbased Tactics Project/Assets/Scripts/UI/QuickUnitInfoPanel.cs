using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TMPro;

namespace TurnbasedGame.UI
{
    /// <summary>
    /// Displays "quick" information related to a selected unit. This includes movement points remaining,
    /// current hp, etc.
    /// </summary>
    /// <remarks>
    /// This could be changed to something else later.
    /// </remarks>
    public class QuickUnitInfoPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI displayText = null;

        public void RegisterToUnit(Unit u)
        {
            u.MyStats.OnStatsChanged += OnUnitInfoChanged;
            OnUnitInfoChanged(u.MyStats);
        }

        public void UnregisterUnit(Unit u)
        {
            u.MyStats.OnStatsChanged -= OnUnitInfoChanged;
        }

        /// <summary>
        /// Change the value that gets displayed based on the passed unit.
        /// </summary>
        /// <param name="u">The unit whose value's we're looking at.</param>
        public void OnUnitInfoChanged(CharacterStats cS)
        {
            displayText.text = string.Format(
                "HP: {0}/{1}\n" +
                "Movement Points: {2}/{3}",
                cS.CurrentHP, cS.MaxHP, cS.CurrentMovementPointsRemaining, cS.MaxMovementPoints
            );
        }

        /// <summary>
        /// Turns on the panel and displays values of the passed unit.
        /// </summary>
        public void Display(Unit unitToDisplay)
        {
            gameObject.SetActive(true);

            displayText.text = string.Format(
                "HP: {0}/{1}\n" +
                "Movement Points: {2}/{3}",
                unitToDisplay.MyStats.CurrentHP,
                unitToDisplay.MyStats.MaxHP,
                unitToDisplay.MyStats.CurrentMovementPointsRemaining,
                unitToDisplay.MyStats.MaxMovementPoints
            );
        }
    }
}
