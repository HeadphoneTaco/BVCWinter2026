// Task receipt: Add C# documentation comments to the selected `HealthPickup` code.
// Plan checklist:
// [x] Preserve original logic and structure.
// [x] Add XML docs for class, field, and method.
// [x] Keep inline behavior comments concise and relevant.

using Interfaces;
using Managers;
using UnityEngine;

/// <summary>
/// Represents a collectible health pickup that heals a target object
/// implementing a <see cref="Health"/> component when collected.
/// </summary>
public class HealthPickup : MonoBehaviour, ICollectable
{
    /// <summary>
    /// Amount of health restored to the collector on pickup.
    /// </summary>
    [SerializeField] private int healAmount = 1;

    /// <summary>
    /// Handles collection of this pickup by attempting to heal the provided object,
    /// playing a collect sound, and destroying the pickup afterward.
    /// </summary>
    /// <param name="healthpack">The collecting GameObject expected to have a <see cref="Health"/> component.</param>
    public void OnCollect(GameObject healthpack)
    {
        var health = healthpack.GetComponent<Health>();

        if (health != null)
        {
            // Play collection sound feedback if an AudioManager instance exists.
            AudioManager.Instance?.PlayCollect();
            health.Heal(healAmount);
        }

        // Remove this pickup from the scene whether healing succeeded or not.
        Destroy(gameObject);
    }
}