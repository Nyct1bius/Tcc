using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour, IHealth
{
    public bool IsAlive, IsMeleeEnemy, WasAttacked;
    public float MaxHealth, CurrentHealth, TimeBetweenAttacks, MovementSpeed, MinimumPlayerDistance;

    public RoomManager RoomManager;
    public NavMeshAgent Agent;
    public GameObject Player;
    public Animator Animator;

    public Vector3 PlayerPosition;

    private EnemyMeleeCombat meleeCombatScript;
    private EnemyMeleePathfinding meleePathfindingScript;
    private EnemyRangedCombat rangedCombatScript;
    private EnemyRangedPathfinding rangedPathfindingScript;

    private CapsuleCollider capsuleCollider;

    void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    void Start()
    {
        Animator.SetBool("Idle", true);

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());

        if (IsMeleeEnemy)
        {
            meleeCombatScript = GetComponent<EnemyMeleeCombat>();
            meleePathfindingScript = GetComponent<EnemyMeleePathfinding>();
        }
        else
        {
            rangedCombatScript = GetComponent<EnemyRangedCombat>();
            rangedPathfindingScript = GetComponent<EnemyRangedPathfinding>();
        }

        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        if (CurrentHealth <= 0 && IsAlive)
        {
            IsAlive = false;
            Death();
        }
        else
        {
            CheckEnemyState();
        }
    }
    
    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CurrentHealth -= damage;

        StartCoroutine(Knockback(PlayerPosition, 4f, .2f));

        Animator.SetTrigger("Hit");
    }
    
    public void HealHealth(float health)
    {

    }

    public void Death()
    {
        IsAlive = false;

        DisableAllBehaviours();

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

    private void CheckEnemyState()
    {
        if (Player != null && !WasAttacked)
        {
            PlayerPosition = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);

            if (Vector3.Distance(transform.position, PlayerPosition) <= MinimumPlayerDistance)
            {
                EnableCombat();
            }
            else
            {
                EnablePathfinding();
            }
        }

        if (WasAttacked)
        {
            DisableAllBehaviours();
        }
    }

    private void EnableCombat()
    {
        if (IsMeleeEnemy)
        {
            meleeCombatScript.enabled = true;
            meleePathfindingScript.enabled = false;
        }
        else
        {
            rangedCombatScript.enabled = true;
            rangedPathfindingScript.enabled = false;
        }
    }

    private void EnablePathfinding()
    {
        if (IsMeleeEnemy)
        {
            meleeCombatScript.StopMeleeCoroutines();
            meleeCombatScript.enabled = false;
            meleePathfindingScript.enabled = true;
        }
        else
        {
            rangedCombatScript.StopRangedCoroutines();
            rangedCombatScript.enabled = false;
            rangedPathfindingScript.enabled = true;
        }
    }

    private void DisableAllBehaviours()
    {
        if (IsMeleeEnemy)
        {
            meleeCombatScript.StopMeleeCoroutines();
            meleeCombatScript.enabled = false;
            meleePathfindingScript.enabled = false;
        }
        else
        {
            rangedCombatScript.StopRangedCoroutines();
            rangedCombatScript.enabled = false;
            rangedPathfindingScript.enabled = false;
        }
    }

    private IEnumerator Knockback(Vector3 direction, float force, float duration)
    {
        if (IsMeleeEnemy)
        {
            gameObject.GetComponent<EnemyMeleeCombat>().StopMeleeCoroutines();
        }
        else
        {
            gameObject.GetComponent<EnemyRangedCombat>().StopRangedCoroutines();

        }

        Agent.enabled = false;

        float timer = 0f;
        while (timer < duration)
        {
            transform.position += direction.normalized * force * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        Agent.enabled = true;
    }

    private IEnumerator Despawn(float timeToDespawn)
    {
        if (Agent.enabled)
            Agent.isStopped = true;

        gameObject.GetComponent<CapsuleCollider>().enabled = false;

        Animator.SetBool("Idle", false);
        Animator.SetBool("Walk", false);
        Animator.SetBool("Dead1", true);

        yield return new WaitForSeconds(timeToDespawn);

        gameObject.SetActive(false);
    }
}
