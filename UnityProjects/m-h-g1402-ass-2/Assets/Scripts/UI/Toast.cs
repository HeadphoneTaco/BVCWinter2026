using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Displays short toast-style UI messages and exposes a global access point via a singleton instance.
    /// </summary>
    public class Toast : MonoBehaviour
    {
        /// <summary>
        /// Global instance of the <see cref="Toast"/> component.
        /// </summary>
        public static Toast Instance;

        /// <summary>
        /// Root GameObject for the toast UI that is shown/hidden.
        /// </summary>
        [SerializeField] private GameObject toastUI;

        /// <summary>
        /// Text component used to render the toast message.
        /// </summary>
        [SerializeField] private TMP_Text toastText;

        /// <summary>
        /// Initializes the singleton instance for this component.
        /// </summary>
        /// <remarks>
        /// If another instance already exists, the duplicate is destroyed.
        /// </remarks>
        private void Awake()
        {
            // Singleton pattern: keep only one active Toast instance.
            if (Instance != null && Instance != this) Destroy(gameObject);

            Instance = this;
        }

        /// <summary>
        /// Hides the toast UI at startup.
        /// </summary>
        private void Start()
        {
            toastUI.SetActive(false);
        }

        /// <summary>
        /// Shows the toast UI and updates its displayed text.
        /// </summary>
        /// <param name="textValue">Message content to display in the toast.</param>
        public void ShowToast(string textValue)
        {
            toastUI.SetActive(true);
            toastText.SetText(textValue);
        }

        /// <summary>
        /// Hides the toast UI.
        /// </summary>
        public void HideToast()
        {
            toastUI.SetActive(false);
        }
    }
}