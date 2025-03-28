 using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour,IHealth
{
    [SerializeField] private float maxHealth;
    [SerializeField] private PlayerUIManager playerUIManager;
    private float currentHealth;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void Death()
    {
        throw new System.NotImplementedException();
    }

    public void HealHealth(float healing)
    {
        currentHealth += healing;
    }

    public void TakeDamage(float damage)
    {
        //if(currentHealth >= 0)
        //{
        //    Death();
        //}

        Debug.Log("Took damage");

        currentHealth -= damage;
        playerUIManager.AtualizePlayerHealthUI(maxHealth, currentHealth);
    }

}
