using System;
using Enums;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        //Input
        [SerializeField] private MouseBehavior mouseBehavior;
        [SerializeField] private InputAction pauseInput;
        [SerializeField] private PlayerController playerController;
    
        public static GameManager Instance;
    
    
        public GameState CurrentState { get; private set; }
        // Other scripts listen to these to show/hide their UI
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameWin;
        public event Action OnGameLoss;

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
            SetState(GameState.Playing);
        }
        private void OnEnable()
        {
            pauseInput.Enable();
            pauseInput.performed += OnPausePerformed;
        }

        private void OnDisable()
        {
            pauseInput.performed -= OnPausePerformed;
            pauseInput.Disable();
        }
    
        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (CurrentState == GameState.Playing) Pause();
            else if (CurrentState == GameState.Paused) Resume();
        }
    
        public void Pause()
        {
            SetState(GameState.Paused);
            Time.timeScale = 0f;
            mouseBehavior.ShowMouse(true);
            OnGamePaused?.Invoke();
        }

        public void Resume()
        {
            SetState(GameState.Playing);
            Time.timeScale = 1f;
            mouseBehavior.ShowMouse(false);
            OnGameResumed?.Invoke();
        }

        public void TriggerWin()
        {
            SetState(GameState.Win);
            Time.timeScale = 0f;
            mouseBehavior.ShowMouse(true);
            playerController.DisableInput();
            OnGameWin?.Invoke();
        }

        public void TriggerLoss()
        {
            SetState(GameState.Loss);
            Time.timeScale = 0f;
            mouseBehavior.ShowMouse(true);
            playerController.DisableInput();
            OnGameLoss?.Invoke();
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        private void SetState(GameState newState)
        {
            CurrentState = newState;
        }
    }
}