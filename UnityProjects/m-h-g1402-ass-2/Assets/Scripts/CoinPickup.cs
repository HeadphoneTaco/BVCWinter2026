using Interfaces;
using Managers;
using UI;
using UnityEngine;

public class CoinPickup : MonoBehaviour, ICollectable
{
    public void OnCollect(GameObject collector)
    {
        FindObjectOfType<CoinUI>()?.AddCoin();
        Debug.Log("Coin Collected");
        //Make sound
        AudioManager.Instance?.PlayCollect();


        Destroy(gameObject);
    }
}
