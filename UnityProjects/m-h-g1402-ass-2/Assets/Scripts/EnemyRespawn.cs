using UnityEngine;
using UnityEngine.AI;

// Attach to Enemy alongside Health.cs
// Assign any Transform as the respawn position in the inspector
public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
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
        transform.position = respawnPoint.position;
        health.ResetHealth();

        NavMeshAgent navAgent = GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.ResetPath();
        }
    }
}