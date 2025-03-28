 using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour,IHealth
{
    [SerializeField] private PlayerUIManager playerUIManager;
    [SerializeField] private PlayerStatsSO stats;


    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void HealHealth(float healing)
    {
        stats.currentHealth += healing;
    }

    public void TakeDamage(float damage)
    {
        //if(currentHealth >= 0)
        //{
        //    Death();
        //}

        Debug.Log("Took damage");

        stats.currentHealth -= damage;
        playerUIManager.AtualizePlayerHealthUI();
    }

}
