using System.Collections;
using UnityEngine;

public class MeleeEnemyCombatState : MeleeEnemyState
{
    private MeleeEnemy enemy;
    private Coroutine cooldownCoroutine;
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

        //Debug.Log(enemy.Agent.isStopped);

        if (enemy.CheckPlayerPosition)
            playerPosition = new Vector3(enemy.Player.transform.position.x, enemy.transform.position.y, enemy.Player.transform.position.z);

        if (!enemy.TookDamage)
            CombatLogic();
        else
        {
            StopCooldownCoroutine();
            StartCooldownCoroutine();
        }

        if (enemy.Stats.Health <= 0 && enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new MeleeEnemyDeadState(stateMachine, enemy));
        if (!enemy.Stats.IsGrounded())
            stateMachine.ChangeState(new MeleeEnemyFallingState(stateMachine, enemy));
    }

    private void CombatLogic()
    {
        if (enemy.IsCloseToTarget(playerPosition))
        {
            enemy.Agent.isStopped = true;

            if (enemy.CheckPlayerPosition)
                enemy.RotateTowards(playerPosition);

            enemy.Animator.SetBool("Idle", true);
            enemy.Animator.SetBool("Walk", false);

            MeleeAttack();
        }
        else
        {
            enemy.Agent.isStopped = false;
            enemy.Agent.SetDestination(playerPosition);

            enemy.Animator.SetBool("Walk", true);
            enemy.Animator.SetBool("Idle", false);

            if (hasAttacked)
            {
                StopCooldownCoroutine();
                enemy.DisableHitbox();
                StartCooldownCoroutine();
            }
        }
    }

    private void MeleeAttack()
    {
        if (!hasAttacked)
        {
            enemy.Animator.SetTrigger("MeleeSlow");
            StartCooldownCoroutine();
        }
    }

    private IEnumerator MeleeAttackCooldown()
    {
        hasAttacked = true;
        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);
        hasAttacked = false;
    }

    private void StartCooldownCoroutine()
    {
        cooldownCoroutine = enemy.StartCoroutine(MeleeAttackCooldown());
    }

    private void StopCooldownCoroutine()
    {
        if (cooldownCoroutine != null)
        {
            enemy.StopCoroutine(cooldownCoroutine);
        }
    }
}
