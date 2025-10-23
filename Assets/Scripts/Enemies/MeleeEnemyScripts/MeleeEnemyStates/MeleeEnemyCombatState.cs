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
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        playerPosition = new Vector3(enemy.Player.transform.position.x, enemy.transform.position.y, enemy.Player.transform.position.z);
        enemy.Agent.SetDestination(playerPosition);

        if (!enemy.TookDamage)
            CombatLogic();

        if (enemy.Stats.Health <= 0 && enemy.Stats.IsGrounded())
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

            enemy.StopCoroutine(MeleeAttack());
            enemy.StopCoroutine(SlowMeleeAttack());
            enemy.AttackHitbox.enabled = false;

            hasAttacked = false;
        }
        else
        {
            enemy.Agent.isStopped = true;
            
            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            if (!hasAttacked)
            {
                enemy.transform.LookAt(playerPosition);
                enemy.StartCoroutine(MeleeAttack());
            }
        }
    }

    private IEnumerator MeleeAttack()
    {
        hasAttacked = true;

        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);
        enemy.StartCoroutine(SlowMeleeAttack());
    }

    private IEnumerator SlowMeleeAttack()
    {
        enemy.Animator.SetTrigger("MeleeSlow");

        yield return new WaitForSeconds(1.18f);
        enemy.AttackHitbox.enabled = true;

        yield return new WaitForSeconds(1.20f);
        enemy.AttackHitbox.enabled = false;

        hasAttacked = false;
    }
}
