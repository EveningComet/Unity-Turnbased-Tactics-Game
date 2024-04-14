using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TMPro;

namespace TurnbasedGame.UI
{
    /// <summary>
    /// Responsible for displaying the “output" of an attack or ability to the player. This includes the chance of
    /// success and the how much damage to deal/heal/etc.
    /// </summary>
    public class SuccessRateIndicator : MonoBehaviour
    {
        /// <summary>
        /// How much damage will be dealt/healed.
        /// </summary>
        [SerializeField] private TextMeshProUGUI damageText = null;

        /// <summary>
        /// The text that will display the success rate.
        /// </summary>
        [SerializeField] private TextMeshProUGUI percentText = null;

        public void Show(int percentToShow, int amountToShow)
        {
            percentText.text = string.Format("Success Rate:\n{0}%", percentToShow);
            damageText.text = string.Format("{0}", amountToShow);
        }
    }
}
