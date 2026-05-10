using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Controls the start menu UI by wiring button events for starting or quitting the game.
    /// </summary>
    public class StartMenu : MonoBehaviour
    {
        /// <summary>
        /// Button that starts the main game scene.
        /// </summary>
        [SerializeField] private Button playButton;
    
        /// <summary>
        /// Button that exits the application.
        /// </summary>
        [SerializeField] private Button quitButton;
    
        /// <summary>
        /// Registers click listeners for the start menu buttons when the component initializes.
        /// </summary>
        private void Start()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
        }
    
        /// <summary>
        /// Loads the main gameplay scene.
        /// </summary>
        private void OnPlayClicked()
        {
            SceneManager.LoadScene("Main");
        }
    
        /// <summary>
        /// Quits the application. In the Unity Editor, this also stops Play mode.
        /// </summary>
        private void OnQuitClicked()
        {
            Application.Quit();
            // Stops play mode in editor since Application.Quit() won't work there
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}