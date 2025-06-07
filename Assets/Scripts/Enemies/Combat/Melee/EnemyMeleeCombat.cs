using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeCombat : MonoBehaviour
{
    public EnemyStats Stats;
    public BoxCollider AttackHitbox;

    GameObject player;
    NavMeshAgent agent;
    Animator animator;

    bool hasAttacked = false;

    private void Start()
    {
        player = Stats.Player;
        agent = Stats.Agent;
        animator = Stats.Animator;
    }

    private void Update()
    {
        if (agent.enabled)
            agent.ResetPath();

        if (Stats.IsAlive)
            transform.LookAt(Stats.PlayerPosition);

        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);

        if (!hasAttacked && Stats.IsAlive)
            StartCoroutine(MeleeAttack(Random.Range(1, 3)));     
    }

    private void OnDisable()
    {
        StopMeleeCoroutines();
    }

    private IEnumerator MeleeAttack(int meleeAnimDice)
    {
        hasAttacked = true;

        yield return new WaitForSeconds(Stats.TimeBetweenAttacks);

        if (meleeAnimDice == 1)
            animator.SetTrigger("Melee1");

        if (meleeAnimDice == 2)
            animator.SetTrigger("Melee2");

        StartCoroutine(AttackHitboxOnThenOff());

        hasAttacked = false;
    }

    private IEnumerator AttackHitboxOnThenOff()
    {
        yield return new WaitForSeconds(0.7f);

        AttackHitbox.enabled = true;

        yield return new WaitForSeconds(0.85f);

        AttackHitbox.enabled = false;
    }

    public void StopMeleeCoroutines()
    {
        StopAllCoroutines();

        if (hasAttacked)
            hasAttacked = false;

        if (AttackHitbox.enabled)
            AttackHitbox.enabled = false;
    }
}
