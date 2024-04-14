using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


namespace TurnbasedGame.UI.Tooltips
{
    /// <summary>
    /// Displays things related to an <see cref="ITooltipable"/>.
    /// </summary>
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText = null;
        [SerializeField] private TextMeshProUGUI descriptionText = null;
        [SerializeField] private Vector3 offset = Vector3.zero;

        /// <summary>
        /// Display the relevant contents of the passed <see cref="ITooltipable"/>.
        /// </summary>
        /// <param name="tTip"><see cref="ITooltipable"/> to display.</param>
        public void Display(ITooltipable tTip)
        {
            titleText.text = tTip.GetTitle();
            descriptionText.text = tTip.GetDescription();

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Update our position based on the passed position, plus our offset.
        /// </summary>
        public void UpdatePosition(Vector3 newPos)
        {
            transform.position = newPos + offset;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            Clear();
        }

        /// <summary>
        /// Clears the contents of the texts.
        /// </summary>
        private void Clear()
        {
            titleText.text = "";
            descriptionText.text = "";
        }
    }
}
