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

    public void StopCheckingPlayerPosition()
    {
        enemy.CheckPlayerPosition = false;
    }
    public void StartCheckingPlayerPosition()
    {
        enemy.CheckPlayerPosition = true;
    }
}
