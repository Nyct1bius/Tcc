using UnityEngine;

public class LevelBorderTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IHealth playerHealth = other.GetComponent<IHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(20f);
            GameManager.instance.RespawnPlayer();
        }
    }
}
