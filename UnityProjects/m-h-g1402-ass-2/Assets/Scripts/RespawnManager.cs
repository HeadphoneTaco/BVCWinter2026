using System.Collections;
using UnityEngine;
using YeetThePlayer;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    [SerializeField] private Health playerHealth;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpawnPoint activeSpawnPoint;
    [SerializeField] private GameObject ghostPrefab;

    [Space(10)]
    [SerializeField] private float deathDelay = 2f;

    private GameObject _activeGhost;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        playerHealth.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        playerHealth.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        StartCoroutine(DeathSequence());
    }

    private IEnumerator DeathSequence()
    {
        // Stop player movement
        playerController.enabled = false;

        // Play death animation if available
        if (playerAnimator != null)
        {
            playerAnimator.SetTrigger("Death");
        }

        // Spawn ghost and keep a reference to it
        if (ghostPrefab != null)
        {
            _activeGhost = Instantiate(ghostPrefab, playerController.transform.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(deathDelay);

        // Clean up ghost before respawning
        if (_activeGhost != null)
        {
            Destroy(_activeGhost);
        }

        // Teleport to spawn point
        playerController.transform.position = activeSpawnPoint.transform.position;

        // Reset health
        playerHealth.ResetHealth();
        
        // Reset Animation
        playerAnimator.SetTrigger("Respawn");

        // Re-enable player
        playerController.enabled = true;
    }

    public void SetSpawnPoint(SpawnPoint newSpawnPoint)
    {
        activeSpawnPoint = newSpawnPoint;
        Debug.Log($"Checkpoint set: {newSpawnPoint.gameObject.name}");
    }
}