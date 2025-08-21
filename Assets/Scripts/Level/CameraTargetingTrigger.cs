using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraTargetingTrigger : MonoBehaviour
{
    public static CameraTargetingTrigger Instance;
    public CinemachineCamera thisCamera;
    [SerializeField] private CameraTarget originalTarget;
    [SerializeField] private CameraTarget temporaryTarget;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);


        thisCamera = GetComponent<CinemachineCamera>();
        thisCamera.Priority = 0;
        StartCoroutine(DelayStart());
    }

    public void TrackingTriggerEnter(Transform targetToLookAt)
    {
        temporaryTarget.LookAtTarget = targetToLookAt;
        thisCamera.Target.LookAtTarget = temporaryTarget.LookAtTarget;
    }

    public void TrackingTriggerExit()
    {
        thisCamera.Target.LookAtTarget = originalTarget.LookAtTarget;
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
    }
}
