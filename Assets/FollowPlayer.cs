using UnityEngine;
using System.Collections;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] float smoothTime = 0.5f;
    [SerializeField] Vector3 offset = new Vector3(-2f, 0f, -2f);
    [SerializeField] float minFollowDistance = 0.5f;

    private void Awake()
    {
        StartCoroutine(DelayStart());
    }

    private void LateUpdate()
    {
        if (playerTransform == null) return;

        Vector3 targetPos = new Vector3(
            playerTransform.position.x + offset.x,
            transform.position.y,
            playerTransform.position.z + offset.z
        );

        float distance = Vector3.Distance(transform.position, targetPos);

        if (distance > minFollowDistance)
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.35f);
        playerTransform = GameManager.instance.PlayerInstance.transform;
    }
}
