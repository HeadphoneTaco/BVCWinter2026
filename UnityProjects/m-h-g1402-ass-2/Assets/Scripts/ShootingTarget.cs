using UnityEngine;

[RequireComponent(typeof(Health))]
public class ShootingTarget : MonoBehaviour
{
    [SerializeField] private Door linkedDoor;
    private Health _health;

    private void Awake()
    {
        _health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        _health.OnDeath += OnTargetDestroyed;
    }

    private void OnDisable()
    {
        _health.OnDeath -= OnTargetDestroyed;
    }

    private void OnTargetDestroyed()
    {
        linkedDoor?.Open();
        gameObject.SetActive(false);
    }
}