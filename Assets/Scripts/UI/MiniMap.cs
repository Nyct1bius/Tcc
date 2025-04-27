using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    private void Start()
    {
        player = GameManager.instance.PlayerInstance.transform;
    }
    private void LateUpdate()
    {
        if(player != null)
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;

        }
     
    }
}
