using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Centralized audio controller responsible for playing gameplay music and common sound effects.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// Global singleton instance used to access audio playback methods.
        /// </summary>
        public static AudioManager Instance;

        /// <summary>
        /// Audio source used for one-shot sound effects.
        /// </summary>
        [Header("SFX")] [SerializeField] private AudioSource sfxSource;

        /// <summary>
        /// Sound effect played when a shot is fired.
        /// </summary>
        [SerializeField] private AudioClip shootClip;

        /// <summary>
        /// Sound effect played when something is hit.
        /// </summary>
        [SerializeField] private AudioClip hitClip;

        /// <summary>
        /// Sound effect played when a collectable is picked up.
        /// </summary>
        [SerializeField] private AudioClip collectClip;

        /// <summary>
        /// Sound effect played on death events.
        /// </summary>
        [SerializeField] private AudioClip deathClip;

        /// <summary>
        /// Audio source used for looping background music.
        /// </summary>
        [Header("Music")] [SerializeField] private AudioSource musicSource;

        /// <summary>
        /// Background music clip played during gameplay.
        /// </summary>
        [SerializeField] private AudioClip gameplayMusic;

        /// <summary>
        /// Enforces the singleton instance so only one audio manager remains active.
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
        /// Starts gameplay background music when the manager is initialized.
        /// </summary>
        private void Start()
        {
            PlayMusic(gameplayMusic);
        }

        /// <summary>
        /// Plays the configured shooting sound effect.
        /// </summary>
        public void PlayShoot()
        {
            sfxSource.PlayOneShot(shootClip);
        }

        /// <summary>
        /// Plays the configured hit sound effect.
        /// </summary>
        public void PlayHit()
        {
            sfxSource.PlayOneShot(hitClip);
        }

        /// <summary>
        /// Plays the configured collect sound effect.
        /// </summary>
        public void PlayCollect()
        {
            sfxSource.PlayOneShot(collectClip);
        }

        /// <summary>
        /// Plays the configured death sound effect.
        /// </summary>
        public void PlayDeath()
        {
            sfxSource.PlayOneShot(deathClip);
        }

        /// <summary>
        /// Sets and starts looping background music if a valid clip is provided.
        /// </summary>
        /// <param name="clip">The music clip to assign and play.</param>
        private void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}