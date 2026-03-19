using UnityEngine;

public class Door : MonoBehaviour
{
    private bool _isOpen = false;

    public void Toggle()
    {
        _isOpen = !_isOpen;
        gameObject.SetActive(!_isOpen);
    }
}