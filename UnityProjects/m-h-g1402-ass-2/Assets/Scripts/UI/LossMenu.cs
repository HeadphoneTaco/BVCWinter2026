using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LossMenu : MonoBehaviour
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            GameManager.Instance.OnGameLoss += Show;
            Hide();
        }

        private void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        private void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        //Moved Subscription to Start
        private void OnDisable()
        {
            GameManager.Instance.OnGameLoss -= Show;
        }

        private void OnRestartClicked()
        {
            GameManager.Instance.RestartGame();
        }

        private void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}