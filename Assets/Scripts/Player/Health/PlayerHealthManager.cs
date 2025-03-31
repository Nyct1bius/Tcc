using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour,IHealth
{
    [SerializeField] private PlayerUIManager playerUIManager;
    [SerializeField] private PlayerStatsSO stats;

    private void Awake()
    {
        stats.currentHealth = stats.maxHealth;
        playerUIManager.AtualizePlayerHealthUI();
    }
    public void Death()
    {
        GameManager.instance.RespawnPlayer();
        stats.currentHealth = stats.maxHealth;
        playerUIManager.AtualizePlayerHealthUI();
    }

    public void HealHealth(float healing)
    {
        stats.currentHealth += healing;
    }

    public void TakeDamage(float damage)

    {
        Debug.Log("Took damage");

        stats.currentHealth -= damage;
        playerUIManager.AtualizePlayerHealthUI();
        if(stats.currentHealth <= 0)
        {
            Death();
        }
    }

}
