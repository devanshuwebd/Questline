using UnityEngine;

namespace Player
{
    public class PlayerCameraController : MonoBehaviour
    {
        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}