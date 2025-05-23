using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeCombat : MonoBehaviour
{
    public EnemyStats Stats;

    GameObject player;
    BoxCollider attackHitbox;
    NavMeshAgent agent;
    Animator animator;

    bool hasAttacked = false;

    public float knockbackForce, knockbackDuration;

    public bool WasAttacked = false;

    private void Awake()
    {
        attackHitbox = GetComponentInChildren<BoxCollider>();
    }

    private void Start()
    {
        player = Stats.Player;
        agent = Stats.Agent;
        animator = Stats.Animator;
    }

    private void Update()
    {
        transform.LookAt(Stats.PlayerPosition);

        //animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);
        
        if (!hasAttacked)
        {
            StartCoroutine(MeleeAttack());
        }

        if (WasAttacked)
        {
            agent.enabled = false;

            hasAttacked = true;
            
            StopAllCoroutines();

            StartCoroutine(ApplyKnockback());
        }
        else
        {
            agent.ResetPath();
        }
    }

    private IEnumerator MeleeAttack()
    {
        hasAttacked = true;

        yield return new WaitForSeconds(Stats.TimeBetweenAttacks);

        animator.SetTrigger("Melee");

        StartCoroutine(AttackHitboxOnThenOff());

        hasAttacked = false;
    }

    private IEnumerator AttackHitboxOnThenOff()
    {
        yield return new WaitForSeconds(1.1f);

        attackHitbox.enabled = true;

        yield return new WaitForSeconds(0.85f);

        attackHitbox.enabled = false;
    }

    private IEnumerator ApplyKnockback()
    {
        Vector3 direction = (transform.position - player.transform.position).normalized;

        yield return new WaitForSeconds(knockbackDuration);

        agent.enabled = true;

        WasAttacked = false;
        hasAttacked = false;
    }
}
