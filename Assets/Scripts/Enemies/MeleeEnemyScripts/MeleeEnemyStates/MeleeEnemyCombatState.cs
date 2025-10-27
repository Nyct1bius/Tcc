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

        if (enemy.CheckPlayerPosition)
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

            if (hasAttacked)
            {
                enemy.DisableHitbox();
                enemy.StartCoroutine(MeleeAttackCooldown());
            }
        }
        else
        {
            enemy.Agent.isStopped = true;
            
            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            if (!hasAttacked)
                MeleeAttack();
        }
    }

    private void MeleeAttack()
    {
        hasAttacked = true;
        enemy.CheckPlayerPosition = false;
        enemy.Animator.SetTrigger("MeleeSlow");
        enemy.StartCoroutine(MeleeAttackCooldown());
    }

    private IEnumerator MeleeAttackCooldown()
    {
        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);
        hasAttacked = false;
    }
}
