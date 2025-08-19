using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, GameManager.instance.PlayerInstance.transform.position,1 * Time.deltaTime);
    }
}
