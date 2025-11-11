using System.Collections;
using UnityEngine;

public class RangedEnemyCombatState : RangedEnemyState
{
    private RangedEnemy enemy;

    private Vector3 playerPosition;

    private bool hasAttacked = false;

    private Coroutine cooldownCoroutine;

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
        else
        {
            StopCooldownCoroutine();
            StartCooldownCoroutine();
        }

        if (enemy.Stats.Health <= 0 && enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new RangedEnemyDeadState(stateMachine, enemy));
        if (!enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new RangedEnemyFallingState(stateMachine, enemy));
    }

    private void CombatLogic()
    {
        if (enemy.IsCloseToTarget(playerPosition))
        {
            enemy.Agent.isStopped = true;

            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            RangedAttack();
            enemy.RotateTowards(playerPosition);
        }
        else
        {
            enemy.Agent.isStopped = false;

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);

            if (hasAttacked)
            {
                StopCooldownCoroutine();
                StartCooldownCoroutine();
            }
        }
    }

    private void RangedAttack()
    {
        if (!hasAttacked)
        {
            enemy.Animator.SetTrigger("Shoot");
            StartCooldownCoroutine();
        }
    }

    private IEnumerator RangedAttackCooldown()
    {
        hasAttacked = true;
        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);
        hasAttacked = false;
    }

    private void StartCooldownCoroutine()
    {
        cooldownCoroutine = enemy.StartCoroutine(RangedAttackCooldown());
    }

    private void StopCooldownCoroutine()
    {
        if (cooldownCoroutine != null)
        {
            enemy.StopCoroutine(cooldownCoroutine);
        }
    }
}
