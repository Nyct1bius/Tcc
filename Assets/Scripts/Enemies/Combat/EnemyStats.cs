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
    private IdlePathfinding idlePathfindingScript;

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

        idlePathfindingScript = GetComponent<IdlePathfinding>();

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
    
    //Take damage and play hit animation
    public void Damage(float damage, Vector3 DamageSourcePos)
    {
        CurrentHealth -= damage;

        Vector3 knockbackDir = (transform.position - DamageSourcePos).normalized;
        StartCoroutine(Knockback(knockbackDir, 8f, .4f));

        Animator.SetTrigger("Hit");

        //StartCoroutine(ChangeColor(0.8f));
    }
    
    //Enemy doesn't heal
    public void HealHealth(float health)
    {

    }

    //Start despawn coroutine, disable combat and pathfinding, remove self from room list
    public void Death()
    {
        int deathAnimDice = Random.Range(0, 2);

        IsAlive = false;

        DisableAllBehaviours();

        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }

        Animator.SetBool("Idle", false);
        Animator.SetBool("Walk", false);

        if (deathAnimDice == 0)
        {
            Animator.SetTrigger("Dead1");
        }
        if (deathAnimDice == 1)
        {
            Animator.SetTrigger("Dead2");
        }

        StartCoroutine(Despawn(2.2f));
    }

    //Wait to store player on scene start
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


    //Check player distance and switch between combat and pathfinding
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

    //Disable pathfinding and enable combat
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

    //Disable combat and combat coroutines and enable pathfinding
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

    //Stop all combat coroutines, disable combat and pathfinding
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

    //Quickly move backwards on getting hit
    private IEnumerator Knockback(Vector3 direction, float force, float duration)
    {
        if (IsMeleeEnemy)
        {
            meleeCombatScript.StopMeleeCoroutines();
        }
        else
        {
            rangedCombatScript.StopRangedCoroutines();

        }

        Agent.enabled = false;

        yield return new WaitForSeconds(0.1f);

        float timer = 0f;
        while (timer < duration)
        {
            transform.position += direction.normalized * force * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        Agent.enabled = true;
    }

    //Disable agent component, collider component, play death animation and disable object
    private IEnumerator Despawn(float timeToDespawn)
    {
        if (Agent.enabled)
            Agent.isStopped = true;

        GetComponent<CapsuleCollider>().enabled = false;

        yield return new WaitForSeconds(timeToDespawn);

        Vector3 originalScale = transform.localScale;
        float timer = 0f;

        while (timer < timeToDespawn)
        {
            float t = 1f - Mathf.Pow(1f - (timer / timeToDespawn), 2f);
            transform.localScale = Vector3.Lerp(originalScale, Vector3.zero, t);
            timer += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
