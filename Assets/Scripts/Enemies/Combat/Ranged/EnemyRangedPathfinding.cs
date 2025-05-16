using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedPathfinding : MonoBehaviour
{
    public EnemyStats Stats;

    private NavMeshAgent agent;
    private Animator animator;

    void Start()
    {
        agent = Stats.Agent;
        animator = Stats.Animator;
    }

    void Update()
    {
        agent.SetDestination(Stats.PlayerPosition);

        animator.SetBool("Idle", true);
        //animator.SetBool("Walk", true);
    }
}
