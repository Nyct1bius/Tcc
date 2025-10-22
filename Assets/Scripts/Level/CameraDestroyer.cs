using UnityEngine;

public class CameraDestroyer : MonoBehaviour
{
    public GameObject fogGameObject, uiCanva;
    

    public void DestroyAfterCutscene()
    {
        if(fogGameObject != null)
            fogGameObject.SetActive(true);
        
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
