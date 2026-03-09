using UnityEngine;

// Simply marks a position as a spawn point.
// Attach to any empty GameObject in the scene.
// Future checkpoints just need this component on them.
public class SpawnPoint : MonoBehaviour
{
    // Visualize spawn point in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
    }
}