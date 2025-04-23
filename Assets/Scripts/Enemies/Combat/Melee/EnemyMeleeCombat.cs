using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject player;
    Transform playerPosition;
    [SerializeField] GameObject attackHitbox;
    public EnemyStats stats;
    [SerializeField] NavMeshAgent agent;
    Animator animator;

    bool hasAttacked = false;
    float minimumDistanceToPlayer = 5;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        //player = GameManager.instance.playerInstance;
        playerPosition = player.transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerPosition.position);

        if (distanceToPlayer > minimumDistanceToPlayer)
        {
            agent.SetDestination(playerPosition.position);

            AnimatorSetMoving();
        }
        else
        {
            agent.ResetPath();

            AnimatorSetIdle();

            transform.LookAt(playerPosition.position);

            if (!hasAttacked)
            {
                StartCoroutine(Attack());

                Debug.Log("Attacked");

                hasAttacked = true;
            }
        }
    }

    private IEnumerator Attack()
    { 
        yield return new WaitForSeconds(stats.TimeBetweenAttacks);

        AnimatorMelee();

        //StartCoroutine(AttackHitboxOnThenOff());

        hasAttacked = false;
    }

    private IEnumerator AttackHitboxOnThenOff()
    {
        attackHitbox.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        attackHitbox.SetActive(false);
    }

    public void MeleeAttack()
    {
        StartCoroutine(AttackHitboxOnThenOff());
    }

    private void AnimatorSetIdle()
    {
        //animator.SetBool("Moving", false);
        animator.SetBool("Idle", true);
    }

    private void AnimatorSetMoving()
    {
        //animator.SetBool("Idle", false);
        //animator.SetBool("Moving", true);
    }

    private void AnimatorMelee()
    {
        animator.SetTrigger("Melee");
    }

    private void AnimatorRanged()
    {
        animator.SetTrigger("Ranged");
    }

    public void AnimatorHit()
    {
        //animator.SetTrigger("Hit");
    }

    public void AnimatorSetDead()
    {
        animator.SetBool("Idle", false);
        //animator.SetBool("Moving", false);
        animator.SetBool("Dead", true);
    }
}
