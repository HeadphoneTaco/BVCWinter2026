using Interfaces;
using Managers;
using UnityEngine;

public class HealthPickup : MonoBehaviour, ICollectable
{
    [SerializeField] private int healAmount = 1;

    public void OnCollect(GameObject healthpack)
    {
        var health = healthpack.GetComponent<Health>();

        if (health != null)
        {
            Debug.Log("Health Collected: " + healAmount);
            //Make sound
            AudioManager.Instance?.PlayCollect();
            health.Heal(healAmount);
        }

        Destroy(gameObject);
    }
}
