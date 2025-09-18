using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetingTrigger : MonoBehaviour
{
    public static CameraTargetingTrigger Instance;
    public CinemachineCamera thisCamera;
    [SerializeField] private CameraTarget originalTarget;
    [SerializeField] private CameraTarget temporaryTarget;
    private float standardFOV, focusedFOV;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);


        thisCamera = GetComponent<CinemachineCamera>();
        thisCamera.Priority = 0;
        focusedFOV = 45f;
        StartCoroutine(DelayStart());
    }

    public void TrackingTriggerEnter(Transform targetToLookAt)
    {
        temporaryTarget.LookAtTarget = targetToLookAt;
        thisCamera.Target.LookAtTarget = temporaryTarget.LookAtTarget;
        //StartCoroutine(FOVChangerIn());
    }

    public void TrackingTriggerExit()
    {
        thisCamera.Target.LookAtTarget = originalTarget.LookAtTarget;
        //StartCoroutine(FOVChangerOut());
    }

    public void EndCustceneSetting()
    {
        thisCamera.Priority = 2;
        GameManager.instance.PlayerInstance.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.25f);
        originalTarget = thisCamera.Target;
        standardFOV = thisCamera.Lens.FieldOfView;
    }

    private IEnumerator FOVChangerIn()
    {
        float timer = 0;
        while (timer < 2f)
        {
            thisCamera.Lens.FieldOfView = Mathf.Lerp(standardFOV, focusedFOV, timer / 2f);
            yield return null;
            timer += Time.deltaTime;
        }
    }private IEnumerator FOVChangerOut()
    {
        float timer = 0;
        while (timer < 2f)
        {
            thisCamera.Lens.FieldOfView = Mathf.Lerp(focusedFOV, standardFOV, timer / 2f);
            yield return null;
            timer += Time.deltaTime;
        }
    }
}
