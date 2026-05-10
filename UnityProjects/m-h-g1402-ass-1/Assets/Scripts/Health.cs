using UnityEngine;

    public class Health :MonoBehaviour, ICollectable
    {
        public void OnCollect()
        {
            Debug.Log("Health Collected");
            Destroy(gameObject);
        }
    }
