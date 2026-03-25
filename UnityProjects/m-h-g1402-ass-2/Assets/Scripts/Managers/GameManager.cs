using System;
using Enums;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    /// <summary>
    /// Manages global game flow states such as playing, pausing, win, and loss,
    /// and broadcasts state-change events for UI and other systems.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        /// <summary>
        /// Controls cursor visibility/locking behavior of the mouse.
        /// </summary>
        [SerializeField] private MouseBehavior mouseBehavior;

        /// <summary>
        /// Input action used to toggle pause and resume.
        /// </summary>
        [SerializeField] private InputAction pauseInput;

        /// <summary>
        /// Reference to the player controller.
        /// </summary>
        [SerializeField] private PlayerController playerController;

        /// <summary>
        /// Singleton instance of the game manager.
        /// </summary>
        public static GameManager Instance;

        /// <summary>
        /// Gets the current high-level game state.
        /// </summary>
        public GameState CurrentState { get; private set; }
        
        // Other scripts listen to these to show/hide their UI
        /// <summary>
        /// Raised when the game transitions into the paused state.
        /// </summary>
        public event Action OnGamePaused;

        /// <summary>
        /// Raised when the game transitions from paused back to playing.
        /// </summary>
        public event Action OnGameResumed;

        /// <summary>
        /// Raised when the game transitions into the win state.
        /// </summary>
        public event Action OnGameWin;

        /// <summary>
        /// Raised when the game transitions into the loss state.
        /// </summary>
        public event Action OnGameLoss;
        
        /// <summary>
        /// Initializes the singleton instance and ensures only one game manager exists.
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
        /// Sets the initial game state when the scene starts.
        /// </summary>
        private void Start()
        {
            SetState(GameState.Playing);
        }

        /// <summary>
        /// Enables pause input handling when this component is active.
        /// </summary>
        private void OnEnable()
        {
            pauseInput.Enable();
            pauseInput.performed += OnPausePerformed;
        }

        /// <summary>
        /// Disables pause input handling when this component is inactive.
        /// </summary>
        private void OnDisable()
        {
            pauseInput.performed -= OnPausePerformed;
            pauseInput.Disable();
        }

        /// <summary>
        /// Handles pause input by toggling between playing and paused states.
        /// </summary>
        /// <param name="context">Input callback context for the performed pause action.</param>
        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (CurrentState == GameState.Playing) Pause();
            else if (CurrentState == GameState.Paused) Resume();
        }

        /// <summary>
        /// Pauses gameplay, shows the cursor, and notifies listeners.
        /// </summary>
        private void Pause()
        {
            SetState(GameState.Paused);
            Time.timeScale = 0f;
            mouseBehavior.ShowMouse(true);
            OnGamePaused?.Invoke();
        }

        /// <summary>
        /// Resumes gameplay, hides the cursor, and notifies listeners.
        /// </summary>
        public void Resume()
        {
            SetState(GameState.Playing);
            Time.timeScale = 1f;
            mouseBehavior.ShowMouse(false);
            OnGameResumed?.Invoke();
        }

        /// <summary>
        /// Transitions the game to the win state, stops time, disables player input, and notifies listeners.
        /// </summary>
        public void TriggerWin()
        {
            SetState(GameState.Win);
            Time.timeScale = 0f;
            mouseBehavior.ShowMouse(true);
            playerController.DisableInput();
            OnGameWin?.Invoke();
        }

        /// <summary>
        /// Transitions the game to the loss state, stops time, disables player input, and notifies listeners.
        /// </summary>
        public void TriggerLoss()
        {
            SetState(GameState.Loss);
            Time.timeScale = 0f;
            mouseBehavior.ShowMouse(true);
            playerController.DisableInput();
            OnGameLoss?.Invoke();
        }

        /// <summary>
        /// Restarts the currently active scene and restores normal timescale.
        /// </summary>
        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        /// <summary>
        /// Loads the main menu scene (build index 0) and restores normal timescale.
        /// </summary>
        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// Updates the current game state.
        /// </summary>
        /// <param name="newState">The state to set as current.</param>
        private void SetState(GameState newState)
        {
            CurrentState = newState;
        }
    }
}