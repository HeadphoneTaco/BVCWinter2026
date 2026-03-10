using UnityEngine;
using Interfaces;

// Attach to a child GameObject on the sword/weapon.
// Requires a Collider set to Is Trigger.
// Enable/disable via Animation Events on the attack animation:
//   - Add event at swing start: EnableHitbox
//   - Add event at swing end:   DisableHitbox
[RequireComponent(typeof(Collider))]
public class MeleeHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    private Collider _hitboxCollider;

    // Track already hit objects so one swing doesn't hit twice
    private Collider _lastHit;

    private void Awake()
    {
        _hitboxCollider = GetComponent<Collider>();
        _hitboxCollider.isTrigger = true;
        DisableHitbox();
    }

    // Called by Animation Event at start of swing
    public void EnableHitbox()
    {
        _lastHit = null;
        _hitboxCollider.enabled = true;
    }

    // Called by Animation Event at end of swing
    public void DisableHitbox()
    {
        _hitboxCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Don't hit the same target twice per swing
        if (other == _lastHit) return;

        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            _lastHit = other;
        }
    }
}