using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour, IHealth
{
    public float MaxHealth, TimeBetweenAttacks, MovementSpeed;

    public float currentHealth;

    bool isAlive = true;

    public RoomManager RoomManager;

    NavMeshAgent agent;

    public GameObject Player;

    void Awake()
    {
        currentHealth = MaxHealth;

        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (agent != null)
        {
            agent.speed = MovementSpeed;
        }

        Player = GameManager.instance.playerInstance;
    }

    void Update()
    {
        if (currentHealth <= 0 && isAlive)
        {
            isAlive = false;
            Death();
        }
    }
    
    public void Damage(float damage)
    {
        currentHealth -= damage;
    }
    
    public void HealHealth(float health)
    {

    }

    public void Death()
    {
        gameObject.GetComponent<IdlePathfinding>().enabled = false;

        if (gameObject.GetComponent<EnemyMeleeCombat>() != null)
        {
            gameObject.GetComponent<EnemyMeleeCombat>().AnimatorSetDead();
            gameObject.GetComponent<EnemyMeleeCombat>().enabled = true;
        }
        if (gameObject.GetComponent<EnemyRangedCombat>() != null)
        {
            gameObject.GetComponent<EnemyMeleeCombat>().AnimatorSetDead();
            gameObject.GetComponent<EnemyRangedCombat>().enabled = true;
        }

        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }

        Destroy(gameObject, 0.5f);
    }
}
