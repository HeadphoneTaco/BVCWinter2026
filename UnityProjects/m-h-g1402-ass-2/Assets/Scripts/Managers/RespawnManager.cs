using System.Collections;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Handles player death and respawn flow, including death effects, delay, and repositioning
    /// to the currently active spawn point.
    /// </summary>
    public class RespawnManager : MonoBehaviour
    {
        /// <summary>
        /// Singleton instance for global access to respawn behavior.
        /// </summary>
        public static RespawnManager Instance;

        /// <summary>
        /// Animator trigger hash used to play the respawn animation/state.
        /// </summary>
        private static readonly int Respawn = Animator.StringToHash("Respawn");

        /// <summary>
        /// Animator trigger hash used to play the death animation/state.
        /// </summary>
        private static readonly int Death = Animator.StringToHash("Death");

        /// <summary>
        /// Reference to the player's health component used to subscribe to death events and reset health.
        /// </summary>
        [SerializeField] private Health playerHealth;

        /// <summary>
        /// Reference to the player's controller, disabled during death and re-enabled after respawn.
        /// </summary>
        [SerializeField] private PlayerController playerController;

        /// <summary>
        /// Animator used to trigger death and respawn animations.
        /// </summary>
        [SerializeField] private Animator playerAnimator;

        /// <summary>
        /// Current spawn point where the player will be teleported on respawn.
        /// </summary>
        [SerializeField] private SpawnPoint activeSpawnPoint;

        /// <summary>
        /// Prefab instantiated at the player's death position for a temporary ghost effect.
        /// </summary>
        [SerializeField] private GameObject ghostPrefab;

        /// <summary>
        /// Delay in seconds between death and respawn.
        /// </summary>
        [Space(10)] [SerializeField] private float deathDelay = 2f;

        /// <summary>
        /// Runtime reference to the active spawned ghost effect so it can be cleaned up before respawn.
        /// </summary>
        private GameObject _activeGhost;

        /// <summary>
        /// Initializes the singleton instance and ensures only one manager exists.
        /// </summary>
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// Subscribes to the player's death event when this component is enabled.
        /// </summary>
        private void OnEnable()
        {
            playerHealth.OnDeath += HandleDeath;
        }

        /// <summary>
        /// Unsubscribes from the player's death event when this component is disabled.
        /// </summary>
        private void OnDisable()
        {
            playerHealth.OnDeath -= HandleDeath;
        }

        /// <summary>
        /// Responds to player death by playing audio and starting the respawn sequence.
        /// </summary>
        private void HandleDeath()
        {
            // Make sound
            AudioManager.Instance?.PlayDeath();

            StartCoroutine(DeathSequence());
        }

        /// <summary>
        /// Executes the full death-to-respawn flow:
        /// disable control, play death animation/effects, wait, reset position/health/animation, then re-enable control.
        /// </summary>
        private IEnumerator DeathSequence()
        {
            // Stop player movement
            playerController.enabled = false;

            // Play death animation if available
            if (playerAnimator != null) playerAnimator.SetTrigger(Death);

            // Spawn ghost and keep a reference to it
            if (ghostPrefab != null)
                _activeGhost = Instantiate(ghostPrefab, playerController.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(deathDelay);

            // Clean up ghost before respawning
            if (_activeGhost != null) Destroy(_activeGhost);

            // Teleport to spawn point
            playerController.transform.position = activeSpawnPoint.transform.position;

            // Reset health
            playerHealth.ResetHealth();

            // Reset Animation
            playerAnimator.SetTrigger(Respawn);

            // Re-enable player
            playerController.enabled = true;
        }

        /// <summary>
        /// Sets the active respawn checkpoint used by future respawns.
        /// </summary>
        /// <param name="newSpawnPoint">The checkpoint to use as the new active spawn point.</param>
        public void SetSpawnPoint(SpawnPoint newSpawnPoint)
        {
            activeSpawnPoint = newSpawnPoint;
            Debug.Log($"Checkpoint set: {newSpawnPoint.gameObject.name}");
        }
    }
}