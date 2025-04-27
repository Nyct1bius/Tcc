using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour, IHealth
{
    public float MaxHealth, TimeBetweenAttacks, MovementSpeed;

    public float currentHealth;

    public bool IsAlive = true;

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
        Player = GameManager.instance.PlayerInstance;
        if(Player == null)
            StartCoroutine(WaitToFindPlayer());
    }

    void Update()
    {
        if (currentHealth <= 0 && IsAlive)
        {
            IsAlive = false;
            Death();
        }
    }
    
    public void Damage(float damage, Vector3 DamageSourcePos)
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
            gameObject.GetComponent<EnemyMeleeCombat>().enabled = false;
        }
        if (gameObject.GetComponent<EnemyRangedCombat>() != null)
        {
            gameObject.GetComponent<EnemyRangedCombat>().AnimatorSetDead();
            gameObject.GetComponent<EnemyRangedCombat>().enabled = false;
        }

        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }

        Destroy(gameObject, 2.2f);
    }

    IEnumerator WaitToFindPlayer()
    {
        yield return new WaitForSeconds(0.25f);
        if(GameManager.instance.PlayerInstance != null)
        {
            while(Player == null)
            {
                Player = GameManager.instance.PlayerInstance;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("GameManager Instance not found");
        }
    }
}
