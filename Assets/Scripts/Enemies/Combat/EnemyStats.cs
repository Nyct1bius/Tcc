using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour, IHealth
{
    public bool IsAlive, IsMeleeEnemy;
    public float MaxHealth, CurrentHealth, TimeBetweenAttacks, MovementSpeed, MinimumPlayerDistance;

    public RoomManager RoomManager;
    public NavMeshAgent Agent;
    public GameObject Player;
    public Animator Animator;

    public Vector3 PlayerPosition;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        if (Agent != null)
            Agent.speed = MovementSpeed;

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());

        Animator = GetComponentInChildren<Animator>();
        Animator.SetBool("Idle", true);
    }

    void Update()
    {
        if (IsAlive)
        {
            if (CurrentHealth <= 0)
            {
                IsAlive = false;
                Death();
            }
            else
            {
                CheckPlayerDistance();
            }
        }
    }
    
    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CurrentHealth -= damage;

        gameObject.GetComponent<EnemyMeleeCombat>().WasAttacked = true;
    }
    
    public void HealHealth(float health)
    {

    }

    public void Death()
    {
        if (IsMeleeEnemy)
        {
            gameObject.GetComponent<EnemyMeleeCombat>().enabled = false;
            gameObject.GetComponent<EnemyMeleePathfinding>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<EnemyRangedCombat>().enabled = false;
            gameObject.GetComponent<EnemyRangedPathfinding>().enabled = false;
        }

        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }

        StartCoroutine(Despawn(2.2f));
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

    private void CheckPlayerDistance()
    {
        if (Player != null)
        {
            PlayerPosition = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);

            if (Vector3.Distance(transform.position, PlayerPosition) <= MinimumPlayerDistance)
            {
                if (IsMeleeEnemy)
                {
                    gameObject.GetComponent<EnemyMeleeCombat>().enabled = true; 
                    gameObject.GetComponent<EnemyMeleePathfinding>().enabled = false;    
                }
                else
                {
                    gameObject.GetComponent<EnemyRangedCombat>().enabled = true;
                    gameObject.GetComponent<EnemyRangedPathfinding>().enabled = false;
                }
            }
            else
            {
                if (IsMeleeEnemy)
                {
                    gameObject.GetComponent<EnemyMeleeCombat>().enabled = false;
                    gameObject.GetComponent<EnemyMeleePathfinding>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<EnemyRangedCombat>().enabled = false;
                    gameObject.GetComponent<EnemyRangedPathfinding>().enabled = true;
                }
            }
        }
    }

    private IEnumerator Despawn(float timeToDespawn)
    {
        Agent.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        Animator.SetBool("Idle", false);
        Animator.SetBool("Dead", true);

        yield return new WaitForSeconds(timeToDespawn);

        gameObject.SetActive(false);
    }
}
