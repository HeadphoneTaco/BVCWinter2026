using TMPro;
using UnityEngine;

// Attach to a UI GameObject with a TMP_Text component.
// Listens to the player's Health events and updates the display.
public class HealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private TMP_Text healthText;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHealthDisplay;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHealthDisplay;
    }

    private void Start()
    {
        // Set initial display
        UpdateHealthDisplay(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    private void UpdateHealthDisplay(int current, int max)
    {
        healthText.SetText($"HP: {current}/{max}");
    }
}