using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject player;
    Transform playerPosition;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPoint;
    public EnemyStats stats;
    [SerializeField] NavMeshAgent agent;
    Animator animator;

    bool hasAttacked = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        AnimatorSetIdle();

        playerPosition = player.transform;
    }

    private void Update()
    {
        if (player == null) return;

        if (CanSeePlayer())
        {
            agent.isStopped = true;

            transform.LookAt(playerPosition.position);

            if (!hasAttacked)
            {
                StartCoroutine(Attack());
                hasAttacked = true;
            }
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
    }

    private IEnumerator Attack()
    {   
        yield return new WaitForSeconds(stats.TimeBetweenAttacks);

        AnimatorRanged();

        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);    

        hasAttacked = false;
    }

    private bool CanSeePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        Ray ray = new Ray(transform.position + Vector3.up, direction.normalized);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.collider.gameObject == player;
        }

        return false;
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
