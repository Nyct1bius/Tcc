using System.Collections;
using UnityEngine;

public class MeleeEnemyCombatState : MeleeEnemyState
{
    private MeleeEnemy enemy;

    private Vector3 playerPosition;

    private bool hasAttacked = false;

    public MeleeEnemyCombatState(MeleeEnemyStateMachine stateMachine, MeleeEnemy enemy) : base(stateMachine)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StopAllCoroutines();
        enemy.Agent.SetDestination(playerPosition);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        playerPosition = new Vector3(enemy.Player.transform.position.x, enemy.transform.position.y, enemy.Player.transform.position.z);

        if (!enemy.TookDamage)
            CombatLogic();

        if (enemy.Stats.CurrentHealth <= 0 && enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new MeleeEnemyDeadState(stateMachine, enemy));
        if (!enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new MeleeEnemyFallingState(stateMachine, enemy));
    }

    private void CombatLogic()
    {
        if (enemy.Agent.remainingDistance > enemy.Agent.stoppingDistance)
        {
            enemy.Agent.isStopped = false;

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);

            enemy.StopCoroutine(MeleeAttack(Random.Range(1, 3)));

            hasAttacked = false;
            enemy.AttackHitbox.enabled = false;
        }
        else
        {
            enemy.Agent.isStopped = true;
            
            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            enemy.transform.LookAt(playerPosition);

            if (!hasAttacked)
            {
                enemy.StartCoroutine(MeleeAttack(Random.Range(1, 3)));
            }
        }
    }

    private IEnumerator MeleeAttack(int meleeAnimDice)
    {
        hasAttacked = true;

        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);

        if (meleeAnimDice == 1)
        {
            enemy.StartCoroutine(FastMeleeAttack());
        }
        if (meleeAnimDice == 2)
        {
            enemy.StartCoroutine(SlowMeleeAttack());
        }
    }

    private IEnumerator FastMeleeAttack()
    {
        enemy.Animator.SetTrigger("MeleeFast");

        yield return new WaitForSeconds(0.2f);

        enemy.AttackHitbox.enabled = true;

        yield return new WaitForSeconds(0.25f);

        enemy.AttackHitbox.enabled = false;

        hasAttacked = false;
    }

    private IEnumerator SlowMeleeAttack()
    {
        enemy.Animator.SetTrigger("MeleeSlow");

        yield return new WaitForSeconds(1.19f);

        enemy.AttackHitbox.enabled = true;

        yield return new WaitForSeconds(1.25f);

        enemy.AttackHitbox.enabled = false;

        hasAttacked = false;
    }
}
