using UnityEngine;

public class MeleeEnemyHitboxProxy : MonoBehaviour
{
    [SerializeField] MeleeEnemy enemy;
    
    public void EnableEnemyHitbox()
    {
        enemy.EnableHitbox();
    }
    public void DisableEnemyHitbox()
    {
        enemy.DisableHitbox();
    }
}
