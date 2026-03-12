using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    private static readonly int OpenParam = Animator.StringToHash("Open");

    public void Open()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger(OpenParam);
        }
        else
        {
            // Fallback if no animator - just disable the door object
            gameObject.SetActive(false);
        }
    }
}