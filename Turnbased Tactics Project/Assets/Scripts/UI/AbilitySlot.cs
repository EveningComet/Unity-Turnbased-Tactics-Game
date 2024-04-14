using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TurnbasedGame.Units;
using TurnbasedGame.Abilities;
using TurnbasedGame.UI.Tooltips;

namespace TurnbasedGame.UI
{
    /// <summary>
    /// Display's a <see cref="Unit"/>'s <see cref="Ability"/> to the player.
    /// </summary>
    public class AbilitySlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public delegate void OnActivated(Ability abilityToActivate);
        public event OnActivated OnActivatedAbility;

        [SerializeField] private Image slotIcon = null;
        private Ability slottedAbility = null;

        /// <remarks>
        /// Stored for convenience.
        /// </remarks>
        private Tooltip tT = null;

        public void SetTooltip(Tooltip tTip)
        {
            tT = tTip;
        }
        
        public void SetAbility(Ability newAbility)
        {
            slottedAbility = newAbility;
            slotIcon.sprite = slottedAbility.abilityIcon;
        }

        #region IPointer Functions
        public void OnPointerDown(PointerEventData eventData)
        {
            // TODO?
        }

        public void OnPointerUp(PointerEventData eventData)
        {
#if UNITY_EDITOR
            Debug.LogFormat("AbilitySlot :: Player clicked on ability: {0}.", slottedAbility.name);
#endif
            // Fire the ability
            if(OnActivatedAbility != null)
            {
                OnActivatedAbility(slottedAbility);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tT.Display(slottedAbility);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tT.Hide();
        }
        #endregion
    }
}
