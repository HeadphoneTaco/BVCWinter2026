using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        [Header("SFX")]
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioClip shootClip;
        [SerializeField] private AudioClip hitClip;
        [SerializeField] private AudioClip collectClip;
        [SerializeField] private AudioClip deathClip;

        [Header("Music")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip gameplayMusic;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            PlayMusic(gameplayMusic);
        }

        public void PlayShoot() => sfxSource.PlayOneShot(shootClip);
        public void PlayHit() => sfxSource.PlayOneShot(hitClip);
        public void PlayCollect() => sfxSource.PlayOneShot(collectClip);
        public void PlayDeath() => sfxSource.PlayOneShot(deathClip);

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        public void StopMusic() => musicSource.Stop();
    }
}