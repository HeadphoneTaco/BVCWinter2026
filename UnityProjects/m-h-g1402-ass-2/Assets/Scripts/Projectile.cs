using UnityEngine;
using Interfaces;

// Attach to any projectile prefab - arrow, magic, etc.
// Set values per prefab in the inspector.
// Eg: Damage 1 unit, Lifetime 5 seconds 
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        // Auto destroy if it hits nothing
        Destroy(gameObject, lifetime);
    }

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