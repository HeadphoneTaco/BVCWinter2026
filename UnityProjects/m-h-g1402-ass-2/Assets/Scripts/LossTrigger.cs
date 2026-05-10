using Managers;
using UnityEngine;

//TODO:Make the game actually get to the loss state

/// <summary>
/// Trigger volume that causes the game to enter a loss state when the player enters it.
/// </summary>
[RequireComponent(typeof(Collider))]
public class LossTrigger : MonoBehaviour
{
    /// <summary>
    /// Ensures the attached collider is configured as a trigger when the object starts.
    /// </summary>
    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    /// <summary>
    /// Called by Unity when another collider enters this trigger.
    /// If the entering collider belongs to the player, it triggers the loss condition.
    /// </summary>
    /// <param name="other">The collider that entered this trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) GameManager.Instance.TriggerLoss();
    }
}