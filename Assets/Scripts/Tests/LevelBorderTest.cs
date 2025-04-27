using UnityEngine;

public class LevelBorderTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IHealth playerHealth = other.GetComponent<IHealth>();

        if (playerHealth != null)
        {
            playerHealth.Damage(20f, transform.position);
            GameManager.instance.RespawnPlayer();
        }
    }
}
