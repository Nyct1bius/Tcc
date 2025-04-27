using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static event UnityAction PauseGame;
    public static void OnGamePause() => PauseGame?.Invoke();

    public static event UnityAction ResumeGame;
    public static void OnResumeGame() => ResumeGame?.Invoke();

    public static event UnityAction GameOver;
    public static void OnGameOver() => GameOver?.Invoke();

    public static event UnityAction RestartGame;
    public static void OnRestartGame() => RestartGame?.Invoke();

    public static event UnityAction QuitGame;
    public static void OnQuitGame() => QuitGame?.Invoke();
}
