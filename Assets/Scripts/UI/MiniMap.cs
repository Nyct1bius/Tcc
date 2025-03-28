using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    private void Start()
    {
        player = GameManager.instance.playerInstance.transform;
    }
    private void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
