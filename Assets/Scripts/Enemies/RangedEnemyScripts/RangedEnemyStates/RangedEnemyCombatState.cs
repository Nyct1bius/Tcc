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

        enemy.Agent.ResetPath();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        playerPosition = new Vector3(enemy.Player.transform.position.x, enemy.transform.position.y, enemy.Player.transform.position.z);

        if (enemy.Stats.CurrentHealth <= 0)
            stateMachine.ChangeState(new RangedEnemyDeadState(stateMachine, enemy));

        if (!enemy.TookDamage)
            CombatLogic();
    }

    private void CombatLogic()
    {
        if (Vector3.Distance(enemy.transform.position, playerPosition) > enemy.Stats.RangedAttackDistance)
        {
            enemy.Agent.SetDestination(playerPosition);

            enemy.Animator.SetBool("Idle", false);
            enemy.Animator.SetBool("Walk", true);

            enemy.StopCoroutine(RangedAttack(Random.Range(1, 3)));
        }
        else
        {
            enemy.Agent.ResetPath();

            enemy.Animator.SetBool("Walk", false);
            enemy.Animator.SetBool("Idle", true);

            enemy.transform.LookAt(playerPosition);

            if (!hasAttacked)
            {
                enemy.StartCoroutine(RangedAttack(Random.Range(1, 3)));
            }
        }
    }

    private IEnumerator RangedAttack(int meleeAnimDice)
    {
        hasAttacked = true;

        yield return new WaitForSeconds(enemy.Stats.TimeBetweenAttacks);

        enemy.Animator.SetTrigger("Ranged");

        yield return new WaitForSeconds(0.6f);

        hasAttacked = false;

        EnemyProjectileGenerator.Instance.SpawnProjectile(enemy.ProjectileSpawnPoint);
    }
}
