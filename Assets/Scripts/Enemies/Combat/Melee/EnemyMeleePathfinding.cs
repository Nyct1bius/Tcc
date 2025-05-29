using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleePathfinding : MonoBehaviour
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
        if (agent.enabled && Stats.IsAlive)
        {
            agent.SetDestination(Stats.PlayerPosition);
        }
        
        if (agent.enabled && !Stats.IsAlive)
        {
            agent.isStopped = true;
        }

        animator.SetBool("Idle", false);
        animator.SetBool("Walk", true);
    }
}
