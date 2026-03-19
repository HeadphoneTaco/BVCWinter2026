using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ShootingTarget : MonoBehaviour, IDamageable
{
    [SerializeField] private Door linkedDoor;

    public void TakeDamage(int amount)
    {
        linkedDoor?.Toggle();
    }
}