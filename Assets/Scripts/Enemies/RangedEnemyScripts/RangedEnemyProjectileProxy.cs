using UnityEngine;
using FMODUnity;

public class RangedEnemyProjectileProxy : MonoBehaviour
{
    public AudioManager AudioManager;
    [SerializeField] private EventReference gunshotSound;
    
    public RangedEnemy enemy;

    public void IntantiateEnemyProjectile()
    {
        enemy.InstantiateProjectile();
        AudioManager.PlayOneShot(gunshotSound, transform.position);
    }
}
