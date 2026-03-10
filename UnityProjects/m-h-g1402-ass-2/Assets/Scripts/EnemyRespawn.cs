using UnityEngine;

// Attach to Enemy alongside Health.cs
// Assign a SpawnPoint in the inspector - this is where
// the enemy will respawn, NOT where it died.
public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] private SpawnPoint respawnPoint;
    [SerializeField] private Health health;

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        // Teleport to respawn point
        transform.position = respawnPoint.transform.position;

        // Reset health
        health.ResetHealth();

        // Reset NavMesh Agent destination by disabling/enabling
        // This clears any stale pathfinding state
        var navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.ResetPath();
        }
    }
}