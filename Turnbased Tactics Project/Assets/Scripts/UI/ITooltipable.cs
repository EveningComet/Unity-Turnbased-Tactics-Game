using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.UI.Tooltips
{
    /// <summary>
    /// "Tag" for something that can have things displayed in a tooltip.
    /// </summary>
    public interface ITooltipable
    {
        /// <summary>
        /// The name of the object.
        /// </summary>
        string GetTitle();

        /// <summary>
        /// The object's description.
        /// </summary>
        string GetDescription();
    }
}
