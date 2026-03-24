using UnityEngine;
using Interfaces;

/// <summary>
/// Controls projectile behavior, including lifetime expiration, collision handling,
/// and damage application to objects that implement <see cref="IDamageable"/>.
/// </summary>
/// <remarks>
/// Attach this component to any projectile prefab (for example: arrows or magic bolts)
/// and configure its damage and lifetime values in the Inspector.
/// </remarks>
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    /// <summary>
    /// Amount of damage dealt when this projectile hits a damageable target.
    /// </summary>
    [SerializeField] private int damage = 1;

    /// <summary>
    /// Maximum lifetime in seconds before the projectile is automatically destroyed.
    /// </summary>
    [SerializeField] private float lifetime = 5f;

    /// <summary>
    /// Schedules this projectile for automatic destruction after <see cref="lifetime"/> seconds.
    /// </summary>
    private void Start()
    {
        // Auto destroy if it hits nothing
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Handles trigger collisions by applying damage to valid targets and destroying the projectile.
    /// </summary>
    /// <param name="other">The collider this projectile entered.</param>
    /// <remarks>
    /// If the collided object implements <see cref="IDamageable"/>, damage is applied and the projectile is destroyed.
    /// The projectile is also destroyed on non-trigger collisions (for example, walls or ground),
    /// while trigger-only detection zones are ignored for this check.
    /// </remarks>
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Destroy(gameObject);
        }

        // Destroy on hitting anything solid (ground, walls etc.)
        // Ignore triggers so it doesn't destroy on detection zones
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}