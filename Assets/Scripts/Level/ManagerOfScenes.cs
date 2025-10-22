using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ManagerOfScenes : MonoBehaviour
{
    [SerializeField] CameraDestroyer[] cameraDestroyers;
    public GameObject fogGameObject;


    public Image topVignette, bottomVignette;

    private void Awake()
    {
        StartCoroutine(DelayedStart());
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.instance.SwitchGameState(GameManager.GameStates.Started);
        GameManager.instance.PlayerInstance.SetActive(false);
        if(fogGameObject != null)
            fogGameObject.SetActive(false);
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
        ToggleVignette();

    }


    public void ToggleVignette()
    {
        topVignette.DOFade(0f, 1.5f);
        bottomVignette.DOFade(0f, 1.5f);
    }
}
