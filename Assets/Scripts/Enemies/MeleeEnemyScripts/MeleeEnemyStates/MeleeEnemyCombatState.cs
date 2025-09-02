using System.Collections;
//using System.Xml;
//using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MeleeEnemyCombatState : EnemyState
{
    private MeleeEnemy enemy;

    private bool hasAttacked = false;
    
    public MeleeEnemyCombatState(MeleeEnemyStateMachine stateMachine) : base(stateMachine)
    {
        enemy = stateMachine as MeleeEnemy;
    }

    public override void Enter()
    {
        base.Enter();
        // Enter combat (e.g., draw weapon, play combat animation)
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (enemy.Stats.CurrentHealth <= 0)       
            stateMachine.ChangeState(new MeleeEnemyDeadState(stateMachine));

        if (Vector3.Distance(enemy.transform.position, enemy.PlayerPosition) > enemy.Stats.MeleeAttackDistance)
        {
            enemy.Agent.SetDestination(enemy.PlayerPosition);

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);
        }
        else
        {
            enemy.Agent.ResetPath();

            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

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

        enemy.StartCoroutine(AttackHitboxOnThenOff());

        hasAttacked = false;
    }

    private IEnumerator AttackHitboxOnThenOff()
    {
        yield return new WaitForSeconds(0.7f);

        enemy.AttackHitbox.enabled = true;

        yield return new WaitForSeconds(0.85f);

        enemy.AttackHitbox.enabled = false;
    }
}
