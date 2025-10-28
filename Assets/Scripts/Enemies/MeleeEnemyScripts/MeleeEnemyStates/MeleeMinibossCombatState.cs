using System.Collections;
using UnityEngine;

public class MeleeMinibossCombatState : MeleeEnemyState
{
    private MeleeEnemy enemy;

    private Vector3 playerPosition;

    private bool hasAttacked = false;

    private Coroutine cooldownCoroutine;    

    public MeleeMinibossCombatState(MeleeEnemyStateMachine stateMachine, MeleeEnemy enemy) : base(stateMachine)
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
        {
            playerPosition = new Vector3(enemy.Player.transform.position.x, enemy.transform.position.y, enemy.Player.transform.position.z);
            enemy.Agent.SetDestination(playerPosition);
        }

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
        if (enemy.Agent.remainingDistance > enemy.Agent.stoppingDistance)
        {
            enemy.Agent.isStopped = false;

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);

            if (hasAttacked)
            {
                StopCooldownCoroutine();
                enemy.DisableHitbox();
                StartCooldownCoroutine();
            }
        }
        else
        {
            enemy.Agent.isStopped = true;
            
            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            if (!hasAttacked)
                MeleeAttack(Random.Range(1, 3));
        }
    }

    private void MeleeAttack(int meleeAnimDice)
    {
        hasAttacked = true;
        enemy.CheckPlayerPosition = false;

        if (meleeAnimDice == 1)
            enemy.Animator.SetTrigger("MeleeFast");
        if (meleeAnimDice == 2)
            enemy.Animator.SetTrigger("MeleeSlow");

        StartCooldownCoroutine();
    }

    private IEnumerator MeleeAttackCooldown()
    {
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
