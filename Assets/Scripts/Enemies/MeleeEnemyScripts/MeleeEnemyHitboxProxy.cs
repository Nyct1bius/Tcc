using UnityEngine;
using FMODUnity;

public class MeleeEnemyHitboxProxy : MonoBehaviour
{
    public AudioManager AudioManager;
    [SerializeField] private EventReference slamSound;

    [SerializeField] MeleeEnemy enemy;
    
    public void EnableEnemyHitbox()
    {
        enemy.EnableHitbox();
        AudioManager.PlayOneShot(slamSound, transform.position);
    }
    public void DisableEnemyHitbox()
    {
        enemy.DisableHitbox();
    }

    public void StopCheckingPlayerPosition()
    {
        enemy.CheckPlayerPosition = false;
    }
    public void StartCheckingPlayerPosition()
    {
        enemy.CheckPlayerPosition = true;
    }
}
