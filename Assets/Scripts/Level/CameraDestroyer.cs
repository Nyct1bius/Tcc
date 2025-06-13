using UnityEngine;

public class CameraDestroyer : MonoBehaviour
{
    
    public void DestroyAfterCutscene()
    {
        Destroy(gameObject);
    }

    public void StartGame()
    {
        GameManager.instance.SwitchGameState(GameManager.GameStates.Started);
    }
}
