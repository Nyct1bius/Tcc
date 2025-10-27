using System.Collections;
using UnityEngine;

public class RangedEnemyCombatState : RangedEnemyState
{
    private RangedEnemy enemy;

    private Vector3 playerPosition;

    private bool hasAttacked = false;

    public RangedEnemyCombatState(RangedEnemyStateMachine stateMachine, RangedEnemy enemy) : base(stateMachine)
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
            stateMachine.ChangeState(new RangedEnemyDeadState(stateMachine, enemy));
        if (!enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new RangedEnemyFallingState(stateMachine, enemy));
    }

    private void CombatLogic()
    {
        if (enemy.Agent.remainingDistance > enemy.Agent.stoppingDistance)
        {
            enemy.Agent.isStopped = false;

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);

            if (hasAttacked)
            {
                enemy.StartCoroutine(RangedAttackCooldown());
            }
        }
        else
        {
            enemy.Agent.isStopped = true;

            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            if (!hasAttacked)
            {
                RangedAttack();
            }
            else
            {
                enemy.transform.LookAt(playerPosition);
            }
        }
    }

    private void RangedAttack()
    {
        hasAttacked = true;
        enemy.Animator.SetTrigger("Shoot");
        enemy.StartCoroutine(RangedAttackCooldown());
    }

    private IEnumerator RangedAttackCooldown()
    {
        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);
        hasAttacked = false;
    }
}
