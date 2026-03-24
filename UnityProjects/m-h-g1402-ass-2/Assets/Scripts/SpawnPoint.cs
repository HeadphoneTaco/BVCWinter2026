using UnityEngine;

/// <summary>
/// Marks a GameObject's transform as a spawn location in the scene.
/// </summary>
public class SpawnPoint : MonoBehaviour
{
    /// <summary>
    /// Draws editor gizmos to visualize this spawn point in the Scene view.
    /// A green wire sphere marks the position, and a vertical line assists with visibility.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 2f);
    }
}