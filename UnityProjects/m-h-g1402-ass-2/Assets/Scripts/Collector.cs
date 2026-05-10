using Interfaces;
using UnityEngine;

/// <summary>
/// Detects trigger collisions and forwards collection events to objects
/// that implement the <see cref="ICollectable"/> interface.
/// </summary>
public class Collector : MonoBehaviour
{
    /// <summary>
    /// Called by Unity when another collider enters this trigger collider.
    /// If the entering object is collectable, invokes its collect handler
    /// and passes this collector's <see cref="GameObject"/> as the collector.
    /// </summary>
    /// <param name="other">The collider that entered this trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        var otherCollectable = other.GetComponent<ICollectable>();

        if (otherCollectable != null) otherCollectable.OnCollect(gameObject);
    }
}