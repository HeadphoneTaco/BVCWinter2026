using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public enum GameState { Playing, Paused, Win, Lose }
    public GameState CurrentState { get; private set; }

    [SerializeField] private MouseBehavior mouseBehavior;
    [SerializeField] private InputAction pauseInput;
    
    // Other scripts listen to these to show/hide their UI
    public event Action OnGamePaused;
    public event Action OnGameResumed;
    public event Action OnGameWon;
    public event Action OnGameLost;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
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

    private void Start()
    {
        SetState(GameState.Playing);
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
        OnGameWon?.Invoke();
    }

    public void TriggerLose()
    {
        SetState(GameState.Lose);
        Time.timeScale = 0f;
        mouseBehavior.ShowMouse(true);
        OnGameLost?.Invoke();
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