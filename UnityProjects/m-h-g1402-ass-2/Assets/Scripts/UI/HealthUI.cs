using TMPro;
using UnityEngine;

// Attach to a UI GameObject with a TMP_Text component.
// Listens to the player's Health events and updates the display.
namespace UI
{
    /// <summary>
    /// Displays the player's health in a TMP text element and keeps it synchronized
    /// with health change events from the assigned <see cref="Health"/> component.
    /// </summary>
    public class HealthUI : MonoBehaviour
    {
        /// <summary>
        /// Reference to the player's health component.
        /// and raises health change notifications.
        /// </summary>
        [SerializeField] private Health playerHealth;

        /// <summary>
        /// Text element used to render the formatted health string.
        /// </summary>
        [SerializeField] private TMP_Text healthText;

        /// <summary>
        /// Initializes the UI with the player's current health values at startup.
        /// </summary>
        private void Start()
        {
            // Set initial display
            UpdateHealthDisplay(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
        }

        /// <summary>
        /// Subscribes to health change events when this UI component becomes active.
        /// </summary>
        private void OnEnable()
        {
            playerHealth.OnHealthChanged += UpdateHealthDisplay;
        }

        /// <summary>
        /// Unsubscribes from health change events when this UI component is disabled.
        /// </summary>
        private void OnDisable()
        {
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }

        /// <summary>
        /// Updates the health text with the latest current and maximum health values.
        /// </summary>
        /// <param name="current">The player's current health.</param>
        /// <param name="max">The player's maximum health.</param>
        private void UpdateHealthDisplay(int current, int max)
        {
            healthText.SetText($"HP: {current}/{max}");
        }
    }
}