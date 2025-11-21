using UnityEngine;

public class EnemyMeleeHitbox : MonoBehaviour
{
    [SerializeField] private MeleeEnemyStats enemyStats;
    
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            other.GetComponent<PlayerHealthManager>().Damage(enemyStats.DamageToPlayer, transform.position);
        }
    }
}
