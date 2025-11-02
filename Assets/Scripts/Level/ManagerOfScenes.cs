using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FMODUnity;

public class ManagerOfScenes : MonoBehaviour
{
    public static ManagerOfScenes instance;
    [SerializeField] CameraDestroyer[] cameraDestroyers;
    public GameObject followableGameObjects;

    [SerializeField] private StudioEventEmitter bgmEmitter;
    [SerializeField] private StudioEventEmitter waveEmitter;

    public Image topVignette, bottomVignette;

    private void Awake()
    {
        instance = this;
        StartCoroutine(DelayedStart());
    }
    private void OnEnable()
    {
        instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }
    private void OnDestroy()
    {
        instance = null;
    }
    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.25f);
        GameManager.instance.SwitchGameState(GameManager.GameStates.Started);
        GameManager.instance.PlayerInstance.SetActive(false);
        if(followableGameObjects != null)
            followableGameObjects.SetActive(false);
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

    public void IntensifyCombat(float combatLevel)
    {
        bgmEmitter.SetParameter("State", combatLevel);
    }


    public void ToggleVignette()
    {
        topVignette.DOFade(0f, 1.5f);
        bottomVignette.DOFade(0f, 1.5f);
    }
}
