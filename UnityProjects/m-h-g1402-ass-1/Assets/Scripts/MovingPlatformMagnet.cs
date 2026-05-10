using UnityEngine;
public class MovingPlatformMagnet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }
     private void OnTriggerExit2D(Collider2D other)
     {
         if (other.gameObject.CompareTag("Player"))
         {
             other.transform.SetParent(null);
         }
     }
}
