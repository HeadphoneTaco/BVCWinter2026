using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    public System.Action OnJump;
    public System.Action<float> OnMove;
    
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions(); //create an instance of the class PlayerInputActions
        _playerInputActions.Enable(); //Enable the input actions
    }

    private void OnEnable()
    {
        _playerInputActions.Player.Jump.performed += OnJumpPressed; //subscribe to jump action
        //_playerInputActions.Player.Horizontal.performed += OnMovement; //subscribe to horizontal movement action
    }
    
    private void OnDisable()
    {
        _playerInputActions.Player.Jump.performed -= OnJumpPressed; //unsubscribe to jump action
        //_playerInputActions.Player.Horizontal.performed -= OnMovement; //unsubscribe to horizontal movement action
    }
    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump"); //Log jump action for debugging
        OnJump?.Invoke(); //Invoke the OnJump action if there are any subscribers
    }

    private void OnMovement()
    {
        OnMove?.Invoke(_playerInputActions.Player.Horizontal.ReadValue<float>()); 
    }

    void Update()
    {
        OnMovement();
    }
}