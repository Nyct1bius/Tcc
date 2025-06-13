using UnityEngine;
using UnityEngine.Events;

public static class GameEvents
{
    public static event UnityAction StartGame;
    public static void OnStartGame() => StartGame?.Invoke();
    public static event UnityAction PauseGame;
    public static void OnGamePause() => PauseGame?.Invoke();

    public static event UnityAction ResumeGame;
    public static void OnResumeGame() => ResumeGame?.Invoke();

    public static event UnityAction GameOver;
    public static void OnGameOver() => GameOver?.Invoke();

    public static event UnityAction EnterCombat;
    public static void OnEnterCombat() => EnterCombat?.Invoke();

    public static event UnityAction ExitCombat;
    public static void OnExitCombat() => ExitCombat?.Invoke();

    public static event UnityAction RestartGame;
    public static void OnRestartGame() => RestartGame?.Invoke();

    public static event UnityAction QuitGame;
    public static void OnQuitGame() => QuitGame?.Invoke();
}
