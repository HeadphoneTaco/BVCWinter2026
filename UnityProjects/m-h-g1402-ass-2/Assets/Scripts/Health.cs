using System;
using Interfaces;
using Managers;
using UnityEngine;

/// <summary>
/// Tracks and manages an entity's health, supports damage/healing operations,
/// and raises events when health changes or reaches zero.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    /// <summary>
    /// Maximum health value this entity can have.
    /// </summary>
    [SerializeField] private int maxHealth = 5;

    /// <summary>
    /// Current health value, clamped between 0 and <see cref="maxHealth"/>.
    /// </summary>
    private int _currentHealth;

    /// <summary>
    /// Raised whenever health is updated.
    /// Arguments: (currentHealth, maxHealth).
    /// </summary>
    public event Action<int, int> OnHealthChanged; // (currentHealth, maxHealth)

    /// <summary>
    /// Raised when health reaches zero.
    /// </summary>
    public event Action OnDeath;

    /// <summary>
    /// Initializes current health to the configured maximum.
    /// </summary>
    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    /// <summary>
    /// Applies damage to this entity, clamps resulting health,
    /// raises health-changed notifications, plays hit audio,
    /// and raises death notification if health reaches zero.
    /// </summary>
    /// <param name="amount">Amount of damage to apply.</param>
    public void TakeDamage(int amount)
    {
        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, maxHealth);

        //Make sound
        AudioManager.Instance?.PlayHit();

        if (_currentHealth <= 0) OnDeath?.Invoke();
    }

    /// <summary>
    /// Restores health by the given amount and clamps to the maximum value,
    /// then raises the health-changed notification.
    /// </summary>
    /// <param name="amount">Amount of health to restore.</param>
    public void Heal(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    /// <summary>
    /// Resets current health to the maximum value and raises the health-changed notification.
    /// </summary>
    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, maxHealth);
    }

    /// <summary>
    /// Gets the current health value.
    /// </summary>
    /// <returns>The current health.</returns>
    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    /// <summary>
    /// Gets the maximum health value.
    /// </summary>
    /// <returns>The configured maximum health.</returns>
    public int GetMaxHealth()
    {
        return maxHealth;
    }
}