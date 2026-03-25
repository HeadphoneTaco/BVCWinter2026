using UnityEngine;

namespace UI
{
    /// <summary>
    /// Controls cursor visibility and lock state for gameplay and UI interactions.
    /// </summary>
    public class MouseBehavior : MonoBehaviour
    {
        /// <summary>
        /// Initializes the cursor state when the component starts.
        /// </summary>
        /// <remarks>
        /// Hides and locks the cursor on game start.
        /// </remarks>
        private void Start()
        {
            ShowMouse(false);
        }

        /// <summary>
        /// Shows or hides the mouse cursor and updates its lock state accordingly.
        /// </summary>
        /// <param name="value">
        /// <see langword="true"/> to show and unlock the cursor for UI interaction;
        /// <see langword="false"/> to hide and lock the cursor for gameplay.
        /// </param>
        public void ShowMouse(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}