using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private InputAction shootInput;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform aimTrack;
    [SerializeField] private GameObject shootObject;
    [SerializeField] private float shootForce;

    private GameObject _projectile;
    private Vector3 _shootDirection;
    private PlayerState _currentState;
    private PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;
        _playerController.OnStateUpdated += StateUpdate;
    }

    void StateUpdate(PlayerState state)
    {
        _currentState = state;
    }
    
    private void OnDisable()
    {
        shootInput.performed -= Shoot;
        _playerController.OnStateUpdated -= StateUpdate;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        if (_currentState != PlayerState.AIM) return;
        
        //Calculate the direction
        _shootDirection = aimTrack.position - shootPoint.position;
        _shootDirection.Normalize();
        
        //Create a new yeet-able object
        _projectile = Instantiate(shootObject, shootPoint.position, Quaternion.LookRotation(_shootDirection));
        
        //Apply a force
        _projectile.GetComponent<Rigidbody>().AddForce(shootForce * _shootDirection, ForceMode.Impulse);
    }
}
