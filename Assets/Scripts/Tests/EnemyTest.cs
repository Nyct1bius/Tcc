using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] PlayerHealthManager playerHealth;

    [ContextMenu("Damage Player")]
    public void DamagePlayer()
    {
        playerHealth.TakeDamage(20f);
    }

    private void OnTriggerEnter(Collider other)
    {
        IHealth player = other.GetComponent<IHealth>();

        if (player != null)
        {
            Debug.Log("Hit Player");
            player.TakeDamage(20f);
        }
    }
}
