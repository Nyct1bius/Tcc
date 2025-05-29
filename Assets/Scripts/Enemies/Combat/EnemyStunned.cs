using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStunned : MonoBehaviour
{
    public EnemyStats Stats;

    NavMeshAgent agent;
    Animator animator;
    Collider cd;

    public float StunTimer;

    void Start()
    {
        agent = Stats.Agent;
        animator = Stats.Animator;
        cd = gameObject.GetComponent<Collider>();
    }

    private void OnEnable()
    {

    }

    private IEnumerator Stunned(float stunTimer)
    {
        cd.enabled = false;

        agent.ResetPath();

        animator.SetBool("Idle", true);
        animator.SetBool("Walk", false);

        yield return new WaitForSeconds(stunTimer);

        cd.enabled = true;

        Stats.WasAttacked = false;
    }
}
