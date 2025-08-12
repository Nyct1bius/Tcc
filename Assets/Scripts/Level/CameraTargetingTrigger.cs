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

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.25f);
        originalTarget = thisCamera.Target;
    }
}
