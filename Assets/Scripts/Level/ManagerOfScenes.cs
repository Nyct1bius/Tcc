using System.Collections;
using UnityEngine;

public class ManagerOfScenes : MonoBehaviour
{
    [SerializeField] CameraDestroyer[] cameraDestroyers;
    private void Awake()
    {
        StartCoroutine(DelayedStart());
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.instance.SwitchGameState(GameManager.GameStates.Started);
        Cursor.lockState = CursorLockMode.None;

    }

    public void SkipCutscene()
    {
        CameraTargetingTrigger.Instance.EndCustceneSetting();
        foreach (var camera in cameraDestroyers)
        {
            camera.DestroyAfterCutscene();
        }
        Cursor.lockState = CursorLockMode.Locked;

    }
}
