using Interfaces;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    // Called directly from the Visual Scripting graph
    // Damage is passed in from the graph, keeping this script generic
    public void Attack(GameObject target, int damage)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage);
        Debug.Log("Damage Dealt: " + damage);
    }
}