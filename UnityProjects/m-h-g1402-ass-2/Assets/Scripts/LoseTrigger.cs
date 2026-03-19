using Managers;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class LoseTrigger : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TriggerLose();
        }
    }
}