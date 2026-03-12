using System;
using UnityEngine;

public class MouseBehavior : MonoBehaviour
{
    private void Start()
    {
        ShowMouse(false);
    }

    public void ShowMouse(bool value)
    {
        Cursor.visible = value;
        Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
