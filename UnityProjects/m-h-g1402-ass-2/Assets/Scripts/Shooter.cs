using UnityEngine;
using UnityEngine.InputSystem;

public class Shooter : MonoBehaviour
{
    [SerializeField] private InputAction shootInput;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject shootObject;
    
    [SerializeField] private float shootForce;

    private GameObject _projectile;
    
    private void OnEnable()
    {
        shootInput.Enable();
        shootInput.performed += Shoot;
    }

    private void OnDisable()
    {
        shootInput.performed -= Shoot;
    }

    private void Shoot(InputAction.CallbackContext context)
    {
        //Create a new yeetable object
        _projectile = Instantiate(shootObject, shootPoint.position, shootPoint.rotation);
        
        //Apply a force
        _projectile.GetComponent<Rigidbody>().AddForce(shootForce * shootPoint.forward);
    }
}
