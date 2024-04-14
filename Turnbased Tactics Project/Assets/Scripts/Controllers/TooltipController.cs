using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.UI.Tooltips;

namespace TurnbasedGame
{
    /// <summary>
    /// Controls a <see cref="Tooltip"/> for a player.
    /// </summary>
    public class TooltipController
    {
        /// <summary>
        /// The currently associated <see cref="Tooltip"/>.
        /// </summary>
        public Tooltip TT { get; private set; }

        public TooltipController(GameObject toolTipCanvasPrefab)
        {
            GameObject tCanvas = GameObject.Instantiate(toolTipCanvasPrefab);
            TT = tCanvas.GetComponentInChildren<Tooltip>();
            TT.Hide();
        }

        public void UpdateTooltipPosition(Vector3 mousePos)
        {
            TT.UpdatePosition(mousePos);
        }
    }
}