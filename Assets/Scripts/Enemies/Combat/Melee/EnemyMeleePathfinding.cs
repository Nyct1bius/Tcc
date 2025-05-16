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
        if (agent.enabled)
        {
            agent.SetDestination(Stats.PlayerPosition);
        }
        else
        {
            agent.enabled = true;
        }

        animator.SetBool("Idle", true);
        //animator.SetBool("Walk", true);
    }
}
