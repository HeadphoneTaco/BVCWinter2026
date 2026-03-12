using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;

        private void Start()
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
            restartButton.onClick.AddListener(OnRestartClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);

            // Hide immediately - GameManager controls visibility
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameManager.Instance.OnGamePaused += Show;
            GameManager.Instance.OnGameResumed += Hide;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnGamePaused -= Show;
            GameManager.Instance.OnGameResumed -= Hide;
        }

        private void Show() => gameObject.SetActive(true);
        private void Hide() => gameObject.SetActive(false);

        private void OnResumeClicked() => GameManager.Instance.Resume();
        private void OnRestartClicked() => GameManager.Instance.RestartGame();
        private void OnMainMenuClicked() => GameManager.Instance.LoadMainMenu();
    }
}