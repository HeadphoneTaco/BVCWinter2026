using Enums;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles projectile firing based on player input and state.
/// Computes a direction from <see cref="shootPoint"/> toward <see cref="aimTrack"/>,
/// spawns a projectile prefab, applies impulse force, and plays the shoot sound.
/// </summary>
public class Shooter : MonoBehaviour
{
    /// <summary>
    /// Input action used to trigger shooting.
    /// </summary>
    [SerializeField] private InputAction shootInput;

    /// <summary>
    /// World-space origin where projectiles are spawned.
    /// </summary>
    [SerializeField] private Transform shootPoint;

    /// <summary>
    /// Transform used as the aiming target to determine shot direction.
    /// </summary>
    [SerializeField] private Transform aimTrack;

    /// <summary>
    /// Projectile prefab to instantiate when firing.
    /// </summary>
    [SerializeField] private GameObject shootObject;

    /// <summary>
    /// Impulse force magnitude applied to the projectile rigidbody.
    /// </summary>
    [SerializeField] private float shootForce;

    /// <summary>
    /// Reference to the most recently spawned projectile instance.
    /// </summary>
    private GameObject _projectile;

    /// <summary>
    /// Reference to the normalized direction used for the current shot.
    /// </summary>
    private Vector3 _shootDirection;

    /// <summary>
    /// Reference to player state.
    /// </summary>
    private PlayerState _currentState;

    /// <summary>
    /// Reference to the player controller.
    /// </summary>
    private PlayerController _playerController;

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    /// <summary>
    /// Subscribes to events when the component is enabled.
    /// </summary>
    private void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;
        _playerController.OnStateUpdated += StateUpdate;
    }

    /// <summary>
    /// Unsubscribes from events when the component is disabled.
    /// </summary>
    private void OnDisable()
    {
        shootInput.performed -= Shoot;
        _playerController.OnStateUpdated -= StateUpdate;
    }

    /// <summary>
    /// Updates the cached player state.
    /// </summary>
    /// <param name="state">The player's newly reported state.</param>
    private void StateUpdate(PlayerState state)
    {
        _currentState = state;
    }

    /// <summary>
    /// Handles shoot input by spawning and launching a projectile while aiming.
    /// </summary>
    /// <param name="context">
    /// Input callback data for the performed action. The payload is not used directly.
    /// </param>
    private void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.Aim) return;

        //Calculate the direction in which to yeet the object
        _shootDirection = aimTrack.position - shootPoint.position;
        _shootDirection.Normalize();

        //Create a new yeet-able object
        _projectile = Instantiate(shootObject, shootPoint.position, Quaternion.LookRotation(_shootDirection));

        //Yeet the object by applying a force
        _projectile.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);

        //Make a sound
        AudioManager.Instance?.PlayShoot();
    }
}