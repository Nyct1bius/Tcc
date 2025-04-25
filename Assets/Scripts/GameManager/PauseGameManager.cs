using UnityEngine;

public static class PauseGameManager 
{
    public static void PauseGame()
    {
        GameEvents.OnGamePause();
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        GameEvents.OnResumeGame();
        Time.timeScale = 1.4f;
    }

    public static void QuitGame()
    {
        Time.timeScale = 1.4f;
    }
}
