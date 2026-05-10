using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Controls the win screen UI and routes button actions to the <see cref="GameManager"/>.
    /// </summary>
    public class WinMenu : MonoBehaviour
    {
        /// <summary>
        /// Button that restarts the current game session.
        /// </summary>
        [SerializeField] private Button restartButton;

        /// <summary>
        /// Button that returns the player to the main menu scene.
        /// </summary>
        [SerializeField] private Button mainMenuButton;

        /// <summary>
        /// Canvas group used to show/hide the win menu.
        /// </summary>
        [SerializeField] private CanvasGroup canvasGroup;

        /// <summary>
        /// Registers UI button callbacks, subscribes to the win event, and hides the menu at startup.
        /// </summary>
        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartClicked);
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
            GameManager.Instance.OnGameWin += Show;
            Hide();
        }

        /// <summary>
        /// Displays the win menu and enables interaction.
        /// </summary>
        private void Show()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }

        /// <summary>
        /// Hides the win menu and disables interaction.
        /// </summary>
        private void Hide()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }

        /// <summary>
        /// Unsubscribes from the win event when this component is disabled.
        /// </summary>
        private void OnDisable()
        {
            GameManager.Instance.OnGameWin -= Show;
        }

        /// <summary>
        /// Handles restart button clicks by requesting a game restart.
        /// </summary>
        private void OnRestartClicked()
        {
            GameManager.Instance.RestartGame();
        }

        /// <summary>
        /// Handles main menu button clicks by loading the main menu.
        /// </summary>
        private void OnMainMenuClicked()
        {
            GameManager.Instance.LoadMainMenu();
        }
    }
}