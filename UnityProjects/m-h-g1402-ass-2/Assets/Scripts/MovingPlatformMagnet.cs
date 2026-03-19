using UnityEngine;
public class MovingPlatformMagnet : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }
     private void OnTriggerExit(Collider other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             other.transform.SetParent(null);
         }
     }
}
