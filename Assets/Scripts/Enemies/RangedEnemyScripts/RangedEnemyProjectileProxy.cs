using UnityEngine;

public class RangedEnemyProjectileProxy : MonoBehaviour
{
    public RangedEnemy enemy;

    public void IntantiateEnemyProjectile()
    {
        enemy.InstantiateProjectile();
    }
}
