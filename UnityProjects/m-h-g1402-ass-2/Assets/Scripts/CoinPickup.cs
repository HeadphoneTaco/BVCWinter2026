using Interfaces;
using Managers;
using UnityEngine;

public class CoinPickup : MonoBehaviour, ICollectable
{
    public void OnCollect(GameObject collector)
    {
        Debug.Log("Coin Collected");
        //Make sound
        AudioManager.Instance?.PlayCollect();


        Destroy(gameObject);
    }
}
