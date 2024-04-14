using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TurnbasedGame.Inputs;
using TurnbasedGame.Abilities;
using TurnbasedGame.Units;
using TurnbasedGame.UI.Tooltips;

namespace TurnbasedGame.UI
{
    /// <summary>
    /// Shows a selected unit's abilities. Meant to be used alongside the <see cref="QuickUnitInfoPanel"/>s.
    /// </summary>
    public class AbilityHotPanel : MonoBehaviour
    {
        [SerializeField] private AbilitySlot abySlotPrefab = null;

        /// <summary>
        /// Where the created <see cref="AbilitySlot"/>s will be childed.
        /// </summary>
        [SerializeField] private Transform abilitySlotContent = null;

        private MouseController myController = null;

        void OnDisable()
        {
            if (myController != null && myController.SelectionController.CurrentlySelectedUnit != null)
                myController.SelectionController.CurrentlySelectedUnit.OnUnitHasTakenAction -= OnUnitHasTakenAction;
        }

        public void SetMyController(MouseController newController)
        {
            myController = newController;
        }

        /// <summary>
        /// Display the abilities of the passed unit.
        /// </summary>
        public void Display(Unit currentlySelectedUnit)
        {
            // Get the currently selected unit's abilities
            List<Ability> abilitiesToDisplay = currentlySelectedUnit.MyAbilities.HeldAbilities;

            currentlySelectedUnit.OnUnitHasTakenAction += OnUnitHasTakenAction;

            // Display them
            foreach (Ability a in abilitiesToDisplay)
            {
                // Create a new slot
                var nas = Instantiate(abySlotPrefab, abilitySlotContent);

                /* Register to that slot, if the player controls the selected unit
                 * and the selected unit has not taken an action yet */
                if 
                (
                    myController.SelectionController.CurrentlySelectedUnit.OwnerId == myController.MyPlayer.PlayerId
                    &&
                    myController.SelectionController.CurrentlySelectedUnit.HasAlreadyTookActionThisTurn == false
                )
                {
                    nas.OnActivatedAbility += ActivateAbility;
                }

                // Otherwise, display the slot as unusable
                else
                {
                    DisplayAbilitySlotAsCannotBeUsed(nas);
                }

                nas.SetAbility(a);
                nas.SetTooltip(myController.TooltipController.TT);
            }
        }

        /// <summary>
        /// Clear all the displayed <see cref="AbilitySlot"/>s.
        /// </summary>
        public void Clear()
        {
            foreach (Transform child in abilitySlotContent.transform)
            {
                // Unregister from that slot
                var nas = child.GetComponent<AbilitySlot>();
                nas.OnActivatedAbility -= ActivateAbility;

                Destroy(child.gameObject);
            }
        }

        private void ActivateAbility(Ability aToActivate)
        {
            if (MayActivateAbility() == true)
                myController.ActivateTargetingForAbility(aToActivate);
        }

        /// <summary>
        /// Can we activate the ability?
        /// </summary>
        private bool MayActivateAbility()
        {
            if (myController.MyPlayer.IsMyTurn == false)
                return false;
            else if (myController.SelectionController.CurrentlySelectedUnit.HasAlreadyTookActionThisTurn == true)
                return false;
            return true;
        }

        #region AbilitySlot Usability Display
        /// <summary>
        /// Display the passed <see cref="AbilitySlot"/> as unusable by the player.
        /// </summary>
        private void DisplayAbilitySlotAsCannotBeUsed(AbilitySlot aSlot)
        {
            var img = aSlot.gameObject.GetComponent<Image>();
            DisplayAsCannotBeUsed(img);
        }

        /// <summary>
        /// Display the passed image as cannot be used.
        /// </summary>
        private void DisplayAsCannotBeUsed(Image img)
        {
            var displayColor = img.color;
            displayColor = Color.gray; // Change the display color
            displayColor.a = 0.5f; // Change the alpha (transparency)
            img.color = displayColor;
        }

        private void OnUnitHasTakenAction(Unit u)
        {
            Clear();
            Display(u);
        }
        #endregion
    }
}
