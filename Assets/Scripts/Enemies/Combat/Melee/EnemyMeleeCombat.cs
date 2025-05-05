using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeCombat : MonoBehaviour
{
    GameObject player;
    Vector3 playerPosition;

    BoxCollider attackHitbox;
    public EnemyStats stats;
    NavMeshAgent agent;
    Animator animator;

    bool hasAttacked = false;
    float minimumDistanceToPlayer = 5;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        attackHitbox = GetComponentInChildren<BoxCollider>();
    }

    private void Start()
    {
        AnimatorSetIdle();

        player = stats.Player;

        agent = stats.agent;
    }

    private void Update()
    {
        if (stats.IsAlive)
        {
            if (player != null)
            {
                playerPosition = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
            }

            float distanceToPlayer = Vector3.Distance(transform.position, playerPosition);

            if (distanceToPlayer > minimumDistanceToPlayer)
            {
                agent.SetDestination(playerPosition);

                AnimatorSetMoving();
            }
            else
            {
                agent.ResetPath();

                AnimatorSetIdle();

                transform.LookAt(playerPosition);

                if (!hasAttacked)
                {
                    StartCoroutine(Attack());

                    Debug.Log("Attacked");

                    hasAttacked = true;
                }
            }
        }
        else
        {
            AnimatorSetDead();
        }
    }

    private IEnumerator Attack()
    { 
        yield return new WaitForSeconds(stats.TimeBetweenAttacks);

        AnimatorMelee();
        MeleeAttack();

        StartCoroutine(AttackHitboxOnThenOff());

        hasAttacked = false;
    }

    private IEnumerator AttackHitboxOnThenOff()
    {
        yield return new WaitForSeconds(1.1f);

        attackHitbox.enabled = true;

        yield return new WaitForSeconds(1);

        attackHitbox.enabled = false;
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
