using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedCombat : MonoBehaviour
{
    [Header("References")]
    GameObject player;
    Vector3 playerPosition;
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

        player = stats.Player;
    }

    private void Update()
    {
        if (stats.IsAlive)
        {
            playerPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);

            if (player == null) return;

            if (CanSeePlayer())
            {
                agent.isStopped = true;

                Debug.Log("Player visible");

                transform.LookAt(playerPosition);

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

                Debug.Log("Player not visible");
            }
        }
        else
        {
            AnimatorSetDead();
        }
    }

    private bool CanSeePlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        Ray ray = new Ray(transform.position, direction.normalized);
        LayerMask mask = LayerMask.GetMask("Player");

        if (Physics.Raycast(ray, out RaycastHit hit, mask))
        {
            return hit.collider.gameObject == player;
        }

        return false;
    }

    private IEnumerator Attack()
    {   
        yield return new WaitForSeconds(stats.TimeBetweenAttacks);

        AnimatorRanged();

        hasAttacked = false;

        yield return new WaitForSeconds(1.1f);

        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);    
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
