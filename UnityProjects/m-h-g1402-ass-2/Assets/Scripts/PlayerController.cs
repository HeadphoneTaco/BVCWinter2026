using System;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages player character movement, aiming, jumping, and state transitions.
/// Supports two distinct movement modes: Explore and Aim.
/// Handles input processing, velocity calculations, gravity, and ground detection.
/// </summary>
public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// Reference to the main camera.
    /// </summary>
    [SerializeField] private Camera playerCamera;

    /// <summary>
    /// Movement speed when in Explore state.
    /// </summary>
    [SerializeField] private float moveSpeed = 30;

    /// <summary>
    /// Rate at which the player accelerates toward target velocity.
    /// </summary>
    [SerializeField] private float accelerationSpeed = 20f;

    /// <summary>
    /// Rate at which the player decelerates when no input is provided.
    /// </summary>
    [SerializeField] private float decelerationSpeed = 25f;

    /// <summary>
    /// Rotation speed for turning in Explore mode.
    /// </summary>
    [SerializeField] private float rotationSpeed = 10;

    /// <summary>
    /// Gravitational acceleration applied to the player's vertical velocity each frame.
    /// </summary>
    [SerializeField] public float gravity = -9.8f;

    /// <summary>
    /// Initial vertical velocity applied when the player jumps.
    /// </summary>
    [SerializeField] private float jumpVelocity = 10f;

    /// <summary>
    /// Movement speed when in Aim state.
    /// </summary>
    [SerializeField] private float moveSpeedAimed = 2;

    /// <summary>
    /// Rotation speed for turning in Aim mode (mouse look sensitivity).
    /// </summary>
    [SerializeField] private float rotationSpeedAimed = 2;

    /// <summary>
    /// Transform used as the target point for aiming direction calculations in Aim mode.
    /// </summary>
    [SerializeField] private Transform aimTrack;

    /// <summary>
    /// Maximum vertical offset for the aim track during aiming mode.
    /// </summary>
    [SerializeField] private float maxaimHeight;

    /// <summary>
    /// Minimum vertical offset for the aim track during aiming mode.
    /// </summary>
    [SerializeField] private float minaimHeight;

    /// <summary>
    /// Offset from the player's position where ground detection checks begin.
    /// </summary>
    [SerializeField] private Vector3 groundCheckOffset;

    /// <summary>
    /// Maximum distance to check for ground below the player.
    /// </summary>
    [SerializeField] private float groundCheckDistance;

    /// <summary>
    /// Radius of the sphere used for ground detection via sphere cast.
    /// </summary>
    [SerializeField] private float groundCheckRadius;

    /// <summary>
    /// Layer mask specifying which layers are considered ground.
    /// </summary>
    [SerializeField] private LayerMask groundLayer;

    /// <summary>
    /// Event invoked when the player successfully jumps.
    /// </summary>
    public event Action OnJumpEvent;

    /// <summary>
    /// Event invoked when the player's state changes between Explore and Aim modes.
    /// </summary>
    public event Action<PlayerState> OnStateUpdated;

    /// <summary>
    /// The raw input vector from movement controls (WASD or analog stick).
    /// </summary>
    private Vector2 _moveInput;

    /// <summary>
    /// The raw input vector from look/aim controls (mouse delta or analog stick).
    /// </summary>
    private Vector2 _lookInput;

    /// <summary>
    /// Cached camera forward direction, normalized and flattened to the horizontal plane.
    /// Used in Explore mode for camera-relative movement calculations.
    /// </summary>
    private Vector3 _camForward;

    /// <summary>
    /// Cached camera right direction, normalized and flattened to the horizontal plane.
    /// Used in Explore mode for camera-relative movement calculations.
    /// </summary>
    private Vector3 _camRight;

    /// <summary>
    /// The normalized direction the player should move in the current frame.
    /// Calculated differently depending on whether the player is in Explore or Aim mode.
    /// </summary>
    private Vector3 _moveDirection;

    /// <summary>
    /// Reference to the CharacterController component on this GameObject.
    /// Used to apply movement to the player in world space.
    /// </summary>
    private CharacterController _characterController;

    /// <summary>
    /// The target rotation the player interpolates toward during movement in Explore mode.
    /// </summary>
    private Quaternion _targetRotation;

    /// <summary>
    /// The current horizontal (X and Z) velocity of the player.
    /// Separate from vertical velocity to allow independent acceleration/deceleration on the ground plane.
    /// </summary>
    private Vector3 _currentHorizontalVelocity;

    /// <summary>
    /// The complete velocity vector of the player, including both horizontal and vertical components.
    /// Applied to the CharacterController each frame to move the player.
    /// </summary>
    private Vector3 _velocity;

    /// <summary>
    /// Whether the player is currently in contact with ground.
    /// Updated via sphere cast in FixedUpdate and used to gate jump input.
    /// </summary>
    private bool _isGrounded;

    /// <summary>
    /// The default local position of the aim track before any vertical aiming adjustments.
    /// Restored when transitioning from Aim mode back to Explore mode.
    /// </summary>
    private Vector3 _defaultAimTrackerPosition;

    /// <summary>
    /// Temporary storage for the aim track position while being updated during look adjustments.
    /// </summary>
    private Vector3 _tempAimTrackerPosition;

    /// <summary>
    /// The current player state, either Explore or Aim.
    /// Determines which movement calculation method is used and controls input response.
    /// </summary>
    private PlayerState _currentState;

    /// <summary>
    /// Checks whether the player is currently touching ground.
    /// </summary>
    /// <returns><see langword="true"/> if the player is grounded; otherwise, <see langword="false"/>.</returns>
    public bool IsGrounded()
    {
        return _isGrounded;
    }

    /// <summary>
    /// Retrieves the current velocity vector of the player.
    /// </summary>
    /// <returns>The player's current velocity.</returns>
    public Vector3 GetPlayerVelocity()
    {
        return _velocity;
    }

    /// <summary>
    /// Disables input processing for this player controller.
    /// </summary>
    public void DisableInput()
    {
        enabled = false;
    }


    #region Unity Functions

    /// <summary>
    /// Initializes the player controller by caching the character controller component,
    /// setting the initial state to Explore, and storing the default aim track position.
    /// </summary>
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        //Set default state
        _currentState = PlayerState.Explore;
        OnStateUpdated?.Invoke(_currentState);

        _defaultAimTrackerPosition = aimTrack.localPosition;
    }


    /// <summary>
    /// Updates player movement and velocity calculations every frame based on the current player state.
    /// Applies gravity and moves the character controller.
    /// </summary>
    private void Update()
    {
        if (_currentState == PlayerState.Explore)
        {
            CalculateMovementExplore();
            aimTrack.localPosition = _defaultAimTrackerPosition;
        }
        else if (_currentState == PlayerState.Aim)
        {
            CalculateMovementAim();
            UpdateAimTrack();
        }


        _characterController.Move(_velocity * Time.deltaTime);
    }

    /// <summary>
    /// Handles fixed-timestep physics updates, checking if the player is grounded
    /// and stabilizing vertical velocity when touching ground.
    /// </summary>
    private void FixedUpdate()
    {
        CheckGrounded();
        if (_isGrounded && _velocity.y < 0) _velocity.y = -0.2f;
    }

    #endregion


    #region Movement and Aiming

    /// <summary>
    /// Processes movement input from the player. Only accepts input when the game is in Playing state.
    /// </summary>
    /// <param name="value">Input value containing the movement direction as a Vector2.</param>
    public void OnMove(InputValue value)
    {
        //Guard against non-playing states
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        _moveInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Processes look/aim input from the player (mouse or analog stick).
    /// </summary>
    /// <param name="value">Input value containing the look direction as a Vector2.</param>
    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    /// <summary>
    /// Handles jump input. Only allows jumping when the player is grounded and the game is in Playing state.
    /// Invokes the <see cref="OnJumpEvent"/> when a jump occurs.
    /// </summary>
    public void OnJump()
    {
        //Guard against non-playing states
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        if (_isGrounded)
        {
            _velocity.y = jumpVelocity;
            OnJumpEvent?.Invoke();
        }
    }

    /// <summary>
    /// Handles aim mode toggling. Transitions between Explore and Aim states based on input press state.
    /// When entering Aim mode, aligns the player's rotation to the camera forward direction.
    /// Only processes input when the game is in Playing state.
    /// </summary>
    /// <param name="value">Input value with press state indicating whether aiming is active.</param>
    public void OnAim(InputValue value)
    {
        //Guard against non-playing states
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        _currentState = value.isPressed ? PlayerState.Aim : PlayerState.Explore;
        OnStateUpdated?.Invoke(_currentState);

        if (_currentState == PlayerState.Aim)
        {
            _camForward = playerCamera.transform.forward;
            _camForward.y = 0;
            _camForward.Normalize();
            transform.rotation = Quaternion.LookRotation(_camForward);
        }
    }

    /// <summary>
    /// Calculates movement in Explore mode where movement is relative to the camera direction.
    /// The player smoothly rotates toward the movement direction and accelerates/decelerates accordingly.
    /// Applies gravity to the vertical velocity.
    /// </summary>
    private void CalculateMovementExplore()
    {
        _camForward = playerCamera.transform.forward;
        _camRight = playerCamera.transform.right;
        _camForward.y = 0;
        _camRight.y = 0;
        _camForward.Normalize();
        _camRight.Normalize();

        _moveDirection = _camRight * _moveInput.x + _camForward * _moveInput.y;

        if (_moveDirection.sqrMagnitude > 0.01f)
        {
            _targetRotation = Quaternion.LookRotation(_moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
        }

        var targetHorizontalVelocity = _moveDirection * moveSpeed;

        var rate = _moveDirection.sqrMagnitude > 0.01f ? accelerationSpeed : decelerationSpeed;
        _currentHorizontalVelocity = Vector3.MoveTowards(
            _currentHorizontalVelocity,
            targetHorizontalVelocity,
            rate * Time.deltaTime
        );

        //Calculate gravity
        _velocity = _currentHorizontalVelocity + Vector3.up * _velocity.y;
        _velocity.y += gravity * Time.deltaTime;
    }

    //TODO:Move Explore and Aim to separate namespace for movement. Swimming and Climbing should be considered.
    /// <summary>
    /// Calculates movement in Aim mode where the player rotates based on horizontal look input
    /// and movement is relative to the player's facing direction.
    /// Applies gravity to the vertical velocity.
    /// </summary>
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

    /// <summary>
    /// Updates the aim track position based on vertical look input,
    /// constraining the vertical offset between min and max aim heights.
    /// </summary>
    private void UpdateAimTrack()
    {
        _tempAimTrackerPosition = aimTrack.localPosition;
        _tempAimTrackerPosition.y += _lookInput.y * rotationSpeedAimed * Time.deltaTime;
        _tempAimTrackerPosition.y = Mathf.Clamp(_tempAimTrackerPosition.y, minaimHeight, maxaimHeight);

        aimTrack.localPosition = _tempAimTrackerPosition;
    }

    /// <summary>
    /// Performs a sphere cast to detect if the player is touching ground.
    /// Uses the configured ground check offset, radius, and distance to determine ground contact.
    /// </summary>
    private void CheckGrounded()
    {
        _isGrounded = Physics.SphereCast(
            transform.position + groundCheckOffset,
            groundCheckRadius,
            Vector3.down,
            out var hit,
            groundCheckDistance,
            groundLayer
        );
    }

    #endregion

    /// <summary>
    /// Renders debug visualization of the ground detection sphere cast in the scene view.
    /// Displays the starting sphere, ending sphere, and a box representing the swept volume.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + groundCheckOffset, groundCheckRadius);
        Gizmos.DrawSphere(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance,
            groundCheckRadius);
        Gizmos.DrawCube(transform.position + groundCheckOffset + Vector3.down * groundCheckDistance / 2,
            new Vector3(1.5f * groundCheckRadius, groundCheckDistance, 1.5f * groundCheckRadius));
    }
}