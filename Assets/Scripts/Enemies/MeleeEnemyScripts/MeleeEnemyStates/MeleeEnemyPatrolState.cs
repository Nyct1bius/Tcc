using System.Collections;
using UnityEngine;

public class MeleeEnemyPatrolState : MeleeEnemyState
{
    private MeleeEnemy enemy;

    private int currentPatrolPoint = 0;
    private bool isWaiting = false;

    public MeleeEnemyPatrolState(MeleeEnemyStateMachine stateMachine, MeleeEnemy enemy) : base(stateMachine)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.StopAllCoroutines();

        currentPatrolPoint = 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && !isWaiting)
            enemy.StartCoroutine(WaitAndMove());

        if (enemy.RoomManager.EnemiesAlerted)
        {
            if (enemy.RoomManager.EnemiesAlerted)
            {
                if (!enemy.HasHyperarmor)
                    stateMachine.ChangeState(new MeleeEnemyCombatState(stateMachine, enemy));
                else
                    stateMachine.ChangeState(new MeleeMinibossCombatState(stateMachine, enemy));
            }
        }
    }

    private void MoveToNextPoint()
    {
        if (enemy.PatrolPoints == null || enemy.PatrolPoints.Length == 0)
            return;

        enemy.Agent.destination = enemy.PatrolPoints[currentPatrolPoint].position;

        currentPatrolPoint = (currentPatrolPoint + 1) % enemy.PatrolPoints.Length;
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;

        enemy.Animator.SetBool("Walk", false);
        enemy.Animator.SetBool("Idle", true);

        enemy.Agent.ResetPath();

        yield return new WaitForSeconds(2f);

        enemy.Animator.SetBool("Idle", false);
        enemy.Animator.SetBool("Walk", true);

        MoveToNextPoint();

        isWaiting = false;
    }
}
