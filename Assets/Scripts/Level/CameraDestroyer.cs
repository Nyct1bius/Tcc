using UnityEngine;

public class CameraDestroyer : MonoBehaviour
{
    public void DestroyAfterCutscene()
    {
        Destroy(gameObject);
    }
}
