using UnityEngine;

public class CameraDestroyer : MonoBehaviour
{
    public GameObject follawableGameObject, uiCanva;
    

    public void DestroyAfterCutscene()
    {
        if(follawableGameObject != null)
            follawableGameObject.SetActive(true);
        
        if(uiCanva != null)
            uiCanva.SetActive(false);

        ManagerOfScenes.instance.ToggleVignette();
        Destroy(gameObject);
    }

    public void StartGame()
    {
        if (!GameManager.instance.PlayerInstance.activeSelf)
            GameManager.instance.PlayerInstance.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

}
