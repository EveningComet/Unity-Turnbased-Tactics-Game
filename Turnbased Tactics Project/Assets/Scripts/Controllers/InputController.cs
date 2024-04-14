using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnbasedGame.Inputs
{
    /// <summary>
    /// Manages input for a player.
    /// </summary>
    public class InputController
    {
        public bool PressedLeftMouseButton { get; private set; }
        public bool PressedRightMouseButton { get; private set; }
        public bool PressedCancelButton { get; private set; }
        public bool PressedAttackButton { get; private set; }
        public bool EndTurnButton { get; private set; }
        public Vector3 MousePos { get; private set; }

        public void UpdateInputs()
        {
            UpdateButtons();
            UpdateMouse();
        }

        private void UpdateButtons()
        {
            PressedLeftMouseButton = Input.GetButtonUp("Fire1");
            PressedRightMouseButton = Input.GetButtonUp("Fire2");
            PressedAttackButton = Input.GetButtonUp("Jump");

            // TODO: Replace this hard-coded input value.
            PressedCancelButton = Input.GetKeyDown(KeyCode.Escape);

            // TODO: Replace this hardcoded value.
            // The player pressed a button to end the turn, so do it
            EndTurnButton = Input.GetKeyUp(KeyCode.Return);
        }

        private void UpdateMouse()
        {
            MousePos = Input.mousePosition;
        }
    }
}
