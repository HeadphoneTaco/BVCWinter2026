using System;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] private Camera playerCamera;
    [Space(10)]
    [Header("Player Movement")]
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private float accelerationSpeed = 20f;
    [SerializeField] private float decelerationSpeed = 25f;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] private float jumpVelocity = 10f;

    [Space(10)]
    [Header("Aiming")]
    [SerializeField] private float moveSpeedAimed = 2;
    [SerializeField] private float rotationSpeedAimed = 2;
    [SerializeField] private Transform aimTrack;
    [SerializeField] private float maxaimHeight;
    [SerializeField] private float minaimHeight;

    [Space(10)]
    [Header("Ground Check")]
    [SerializeField] private Vector3 groundCheckOffset;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    public event Action OnJumpEvent;
    public event Action<PlayerState> OnStateUpdated;
    
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private Vector3 _camForward;
    private Vector3 _camRight;
    private Vector3 _moveDirection;
    private CharacterController _characterController;
    private Quaternion _targetRotation;
    private Vector3 _currentHorizontalVelocity;
    private Vector3 _velocity;
    private bool _isGrounded;
    private Vector3 _defaultAimTrackerPosition;
    private Vector3 _tempAimTrackerPosition;
    
    private PlayerState _currentState;

    // Property
    public bool IsGrounded()
    {
        return _isGrounded;
    }

    public Vector3 GetPlayerVelocity()
    {
        return _velocity;
    }
    public void DisableInput()
    {
        enabled = false;
    }
    

  #region Unity Functions
    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        
        //Set default state
        _currentState = PlayerState.EXPLORE;
        OnStateUpdated?.Invoke(_currentState);
        
        _defaultAimTrackerPosition = aimTrack.localPosition;
    }
    

    // Update is called once per frame
    void Update()
    {
        if (_currentState == PlayerState.EXPLORE)
        {
            CalculateMovementExplore();
                aimTrack.localPosition = _defaultAimTrackerPosition;
        }
        else if (_currentState == PlayerState.AIM)
        {
            CalculateMovementAim();
            UpdateAimTrack();
        }
        
        
        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        CheckGrounded();
        if(_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -0.2f;
        }
    }
  #endregion    


  #region Movement and Aiming
    public void OnMove(InputValue value)
    {
        //Guard against non-playing states
        if (GameManager.Instance.CurrentState != GameState.Playing) return;
        
        _moveInput = value.Get<Vector2>();
    }
    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }
    public void OnJump()
    {
        //Guard against non-playing states
        if (GameManager.Instance.CurrentState != GameState.Playing) return;
        
        if(_isGrounded)
        {
            Debug.Log("JUMP");
            _velocity.y = jumpVelocity;
            OnJumpEvent?.Invoke();
        }
    }
    public void OnAim(InputValue value)
    {
        //Guard against non-playing states
        if (GameManager.Instance.CurrentState != GameState.Playing) return;
        
        _currentState = value.isPressed ? PlayerState.AIM : PlayerState.EXPLORE;
        OnStateUpdated?.Invoke(_currentState);
        
        if (_currentState == PlayerState.AIM)
        {
            _camForward = playerCamera.transform.forward;
            _camForward.y = 0;
            _camForward.Normalize();
            transform.rotation = Quaternion.LookRotation(_camForward);
        }
    }
    private void CalculateMovementExplore()
    {
        _camForward = playerCamera.transform.forward;
        _camRight = playerCamera.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();

        _moveDirection = _camRight * _moveInput.x + _camForward * _moveInput.y;

        if(_moveDirection.sqrMagnitude > 0.01f)
        {
            _targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }
        
        Vector3 targetHorizontalVelocity = _moveDirection * moveSpeed;
        
        float rate = _moveDirection.sqrMagnitude > 0.01f ? accelerationSpeed : decelerationSpeed;
        _currentHorizontalVelocity = Vector3.MoveTowards(
            _currentHorizontalVelocity, 
            targetHorizontalVelocity, 
            rate * Time.deltaTime
        );
        
        //Calculate gravity
        _velocity = _currentHorizontalVelocity + Vector3.up * _velocity.y;
        _velocity.y += gravity * Time.deltaTime;
    }

    private void CalculateMovementAim()
    {
    //Rotate player around Y-axis based on horizontal mouse movement
    transform.Rotate(Vector3.up, rotationSpeed * _lookInput.x * Time.deltaTime);
    
    //WASD relates to where the player is currently facing
    //Left/Right goes sideways, forward/back moves along the players facing direction
    _moveDirection = _moveInput.x * transform.right + _moveInput.y * transform.forward;
    
   
    _velocity = _velocity.y * Vector3.up + moveSpeedAimed * _moveDirection;
    _velocity.y += gravity * Time.deltaTime;
    }

    private void UpdateAimTrack()
    {
        _tempAimTrackerPosition = aimTrack.localPosition;
        _tempAimTrackerPosition.y += _lookInput.y * rotationSpeedAimed * Time.deltaTime;
        _tempAimTrackerPosition.y = Mathf.Clamp(_tempAimTrackerPosition.y, minaimHeight, maxaimHeight);
        
        aimTrack.localPosition = _tempAimTrackerPosition;
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.SphereCast(
            transform.position + groundCheckOffset,
            groundCheckRadius,
            Vector3.down,
            out RaycastHit hit,
            groundCheckDistance,
            groundLayer
        );
    }
  #endregion    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);
        Gizmos.DrawSphere(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance, groundCheckRadius);
        Gizmos.DrawCube(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance/2, 
            new Vector3(1.5f* groundCheckRadius, groundCheckDistance , 1.5f * groundCheckRadius) );
    }
}