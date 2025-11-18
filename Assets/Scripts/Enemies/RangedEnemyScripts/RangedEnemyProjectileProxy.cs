using UnityEngine;
using FMODUnity;

public class RangedEnemyProjectileProxy : MonoBehaviour
{
    [SerializeField] private EventReference gunshotSound;
    
    public RangedEnemy enemy;

    public void IntantiateEnemyProjectile()
    {
        enemy.InstantiateProjectile();
    }
}
