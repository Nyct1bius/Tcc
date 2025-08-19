using UnityEngine;

public class CameraDestroyer : MonoBehaviour
{
    
    public void DestroyAfterCutscene()
    {
        Destroy(gameObject);
    }

    public void StartGame()
    {
        if (!GameManager.instance.PlayerInstance.activeSelf)
            GameManager.instance.PlayerInstance.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

}
