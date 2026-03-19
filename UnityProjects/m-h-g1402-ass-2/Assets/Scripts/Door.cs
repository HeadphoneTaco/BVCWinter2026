using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator;
    private static readonly int IsOpen = Animator.StringToHash("_isOpen");
    
    private bool _isOpen = false;

    public void Toggle()
    {
        _isOpen = !_isOpen;
        doorAnimator.SetBool(IsOpen, _isOpen);
    }
}