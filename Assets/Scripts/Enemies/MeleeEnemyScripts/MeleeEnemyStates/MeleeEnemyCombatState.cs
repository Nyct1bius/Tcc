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

        enemy.Agent.ResetPath();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        playerPosition = new Vector3(enemy.Player.transform.position.x, enemy.transform.position.y, enemy.Player.transform.position.z);

        if (!enemy.TookDamage)
            CombatLogic();

        if (enemy.Stats.CurrentHealth <= 0)
        {
            stateMachine.ChangeState(new MeleeEnemyDeadState(stateMachine, enemy));

            if (enemy.RoomManager != null)
                enemy.RemoveSelfFromList();
        }
    }

    private void CombatLogic()
    {
        if (Vector3.Distance(enemy.transform.position, playerPosition) > enemy.Stats.MeleeAttackDistance)
        {
            enemy.Agent.SetDestination(playerPosition);

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);

            enemy.StopCoroutine(MeleeAttack(Random.Range(1, 3)));

            hasAttacked = false;
            enemy.AttackHitbox.enabled = false;
        }
        else
        {
            enemy.Agent.ResetPath();

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
            enemy.Animator.SetTrigger("Melee1");

        if (meleeAnimDice == 2)
            enemy.Animator.SetTrigger("Melee2");

        yield return new WaitForSeconds(0.7f);

        enemy.AttackHitbox.enabled = true;

        yield return new WaitForSeconds(0.85f);

        enemy.AttackHitbox.enabled = false;

        hasAttacked = false;
    }
}
