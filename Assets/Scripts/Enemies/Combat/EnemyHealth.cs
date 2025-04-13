using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public float MaxHealth, CurrentHealth;

    private bool isAlive = true;

    public RoomManager RoomManager;

    //Temporary
    private Color originalColor;
    private Renderer enemyRenderer;

    void Awake()
    {
        CurrentHealth = MaxHealth;

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    void Update()
    {
        if (CurrentHealth <= 0 && isAlive)
        {
            isAlive = false;
            Death();
        }
    }
    
    public void Damage(float damage)
    {
        CurrentHealth -= damage;

        if (isAlive)
        {
            StartCoroutine(ChangeColorWhenTakingDamage());
        }
        else
        {
            enemyRenderer.material.color = Color.white;
        }
    }
    
    public void HealHealth(float health)
    {

    }

    public void Death()
    {
        gameObject.GetComponent<IdlePathfinding>().enabled = false;
        gameObject.GetComponent<EnemyMeleeCombat>().enabled = false;

        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }
    }

    // Temporary
    private IEnumerator ChangeColorWhenTakingDamage()
    {
        enemyRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        enemyRenderer.material.color = originalColor;
    }
}
