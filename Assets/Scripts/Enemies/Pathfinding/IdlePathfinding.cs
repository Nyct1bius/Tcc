using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class IdlePathfinding : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimer = 2f;

    public EnemyStats Stats;

    NavMeshAgent agent;
    Animator animator;
    
    private int currentPatrolIndex = 0;
    private bool isWaiting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = Stats.Agent;
        animator = Stats.Animator;

        if (patrolPoints.Length > 0)
        {
            agent.SetDestination(patrolPoints[0].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isWaiting || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            StartCoroutine(WaitAndMove());
        }
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;

        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);

        yield return new WaitForSeconds(waitTimer);

        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;

        agent.SetDestination(patrolPoints[currentPatrolIndex].position);

        isWaiting = false;

        animator.SetBool("Walk", true);
        animator.SetBool("Idle", false);
    }
}
