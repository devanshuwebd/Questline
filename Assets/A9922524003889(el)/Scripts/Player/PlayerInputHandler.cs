using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [Header("Input Actions")]
        public InputActionReference moveAction;
        public InputActionReference jumpAction;
        public InputActionReference sprintAction;

        public Vector2 MoveInput { get; private set; }
        public bool JumpTriggered { get; private set; }
        public bool IsSprinting { get; private set; }

        private void OnEnable() {
            if (moveAction) moveAction.action.Enable();
            if (jumpAction) jumpAction.action.Enable();
            if (sprintAction) sprintAction.action.Enable();
        }

        private void OnDisable() {
            if (moveAction) moveAction.action.Disable();
            if (jumpAction) jumpAction.action.Disable();
            if (sprintAction) sprintAction.action.Disable();
        }

        private void Update() {
            if (moveAction) MoveInput = moveAction.action.ReadValue<Vector2>();
            if (jumpAction) JumpTriggered = jumpAction.action.WasPressedThisFrame();
            if (sprintAction) IsSprinting = sprintAction.action.IsPressed();
        }
    }
}