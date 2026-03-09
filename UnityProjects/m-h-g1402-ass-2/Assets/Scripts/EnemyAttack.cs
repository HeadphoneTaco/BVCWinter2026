using Interfaces;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;

    // Called directly from the Visual Scripting graph
    public void Attack(GameObject target)
    {
        IDamageable damageable = target.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage);
        
        Debug.Log("Damage Done");
    }
}