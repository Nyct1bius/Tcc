using UnityEngine;

public class EnemyTest : MonoBehaviour
{
    [SerializeField] PlayerHealthManager playerHealth;

    [ContextMenu("Damage Player")]
    public void DamagePlayer()
    {
        playerHealth.Damage(20f,transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        IHealth player = other.GetComponent<IHealth>();

        if (player != null)
        {
            Debug.Log("Hit Player");
            player.Damage(20f, transform.position);
        }
    }
}
