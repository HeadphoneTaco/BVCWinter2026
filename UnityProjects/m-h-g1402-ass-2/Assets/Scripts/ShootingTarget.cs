using Interfaces;
using UnityEngine;

/// <summary>
/// Represents a shootable target that can receive damage and toggle a linked door.
/// </summary>
//TODO:Make Generic trigger for uses other than just toggling a door.
[RequireComponent(typeof(Collider))]
public class ShootingTarget : MonoBehaviour, IDamageable
{
    /// <summary>
    /// The door controlled by this target. When the target is damaged, this door is toggled.
    /// </summary>
    [SerializeField] private Door linkedDoor;

    /// <summary>
    /// Handles incoming damage for this target.
    /// </summary>
    /// <param name="amount">
    /// The amount of damage received. This implementation ignores the value and only triggers the door toggle.
    /// </param>
    public void TakeDamage(int amount)
    {
        linkedDoor?.Toggle();
    }
}