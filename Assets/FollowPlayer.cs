using System.Collections;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 currentPos, targetPos;
    float followSpeed = 5f;

    private void Awake()
    {
        StartCoroutine(DelayStart());
    }
    private void LateUpdate()
    {
        if(playerTransform != null)
        {
            currentPos = transform.position;

            targetPos = new Vector3(playerTransform.position.x, currentPos.y, playerTransform.position.z);

            transform.position = Vector3.Lerp(currentPos, targetPos, followSpeed * Time.deltaTime);
        }
        
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(0.35f);
        playerTransform = GameManager.instance.PlayerInstance.transform;
    }
}
