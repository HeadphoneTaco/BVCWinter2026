using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Controls the pause screen UI and routes button actions to the <see cref="GameManager"/>.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        /// <summary>
        /// Button used to resume gameplay from the pause state.
        /// </summary>
        [SerializeField] private Button resumeButton;

        /// <summary>
        /// Button used to restart the current game session.
        /// </summary>
        [SerializeField] private Button restartButton;

        /// <summary>
        /// Button that returns the player to the main menu.
        /// </summary>
        [SerializeField] private Button mainMenuButton;

        /// <summary>
        /// Canvas group used to show/hide the pause menu.
        /// </summary>
        [SerializeField] private CanvasGroup canvasGroup;

        /// <summary>
        /// Registers button click listeners and ensures the pause menu starts hidden.
        /// </summary>
        private void Start()
        {
            resumeButton.onClick.AddListener(OnResumeClicked);
            restartButton.onClick.AddListener(OnRestartClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            GameManager.Instance.OnGamePaused += Show;
            Hide();
        }

        /// <summary>
        /// Shows the pause menu and enables interaction.
        /// </summary>
        private void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hides the pause menu and disables interaction.
        /// </summary>
        private void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Subscribes to game pause event when this component becomes active.
        /// </summary>
        //TODO: Streamline to only have OnGamePaused
        private void OnEnable()
        {
            GameManager.Instance.OnGamePaused += Show;
            GameManager.Instance.OnGameResumed += Hide;
        }

        /// <summary>
        /// Unsubscribes from game pause event when this component becomes inactive.
        /// </summary>
        //TODO: Streamline to only have OnGamePaused
        private void OnDisable()
        {
            GameManager.Instance.OnGamePaused -= Show;
            GameManager.Instance.OnGameResumed -= Hide;
        }

        /// <summary>
        /// Handles resume button clicks by requesting the game to resume.
        /// </summary>
        private void OnResumeClicked()
        {
            GameManager.Instance.Resume();
        }

        /// <summary>
        /// Handles restart button clicks by requesting a game restart.
        /// </summary>
        private void OnRestartClicked()
        {
            GameManager.Instance.RestartGame();
        }

        /// <summary>
        /// Handles main menu button clicks by loading the main menu scene.
        /// </summary>
        private void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}