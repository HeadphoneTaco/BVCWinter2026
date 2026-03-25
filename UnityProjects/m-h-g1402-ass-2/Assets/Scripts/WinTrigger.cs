using Managers;
using UnityEngine;

//TODO:Make the game have a win condition that isn't just a collision volume

/// <summary>
/// Trigger volume that ends the level with a win state when the player enters it.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WinTrigger : MonoBehaviour
{
    /// <summary>
    /// Ensures the attached collider is configured as a trigger at startup.
    /// </summary>
    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Called by Unity when another collider enters this trigger.
    /// If the entering object is tagged as <c>Player</c>, the win condition is triggered.
    /// </summary>
    /// <param name="other">The collider that entered this trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.TriggerWin();
    }
}