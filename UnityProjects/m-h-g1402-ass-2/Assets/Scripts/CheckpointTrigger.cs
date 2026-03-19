using Managers;
using UnityEngine;

// Attach to a trigger volume next to a SpawnPoint.
// When the player walks through, updates the active spawn point.
[RequireComponent(typeof(Collider))]
public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private SpawnPoint spawnPoint;

    private void Start()
    {
        // Make sure collider is a trigger
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RespawnManager.Instance.SetSpawnPoint(spawnPoint);
        }
    }
}