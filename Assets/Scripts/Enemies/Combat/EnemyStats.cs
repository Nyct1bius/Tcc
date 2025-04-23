using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour, IHealth
{
    public float MaxHealth, TimeBetweenAttacks, MovementSpeed;

    private float currentHealth;

    private bool isAlive = true;

    public RoomManager RoomManager;

    private NavMeshAgent agent;

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
        gameObject.GetComponent<EnemyMeleeCombat>().enabled = false;

        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }
    }
}
