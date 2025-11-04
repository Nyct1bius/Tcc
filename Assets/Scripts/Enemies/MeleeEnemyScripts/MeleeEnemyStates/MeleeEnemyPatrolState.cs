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

        currentPatrolPoint = 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!enemy.Agent.pathPending && enemy.Agent.remainingDistance <= enemy.Agent.stoppingDistance && !isWaiting)
            enemy.StartCoroutine(WaitAndMove());

        if (enemy.RoomManager.EnemiesAlerted && !enemy.HasHyperarmor)
            stateMachine.ChangeState(new MeleeEnemyCombatState(stateMachine, enemy));
        if (enemy.RoomManager.EnemiesAlerted && enemy.HasHyperarmor)
            stateMachine.ChangeState(new MeleeMinibossCombatState(stateMachine, enemy));
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;

        enemy.Animator.SetBool("Idle", true);
        enemy.Animator.SetBool("Walk", false);

        enemy.Agent.ResetPath();

        yield return new WaitForSeconds(2f);

        enemy.Animator.SetBool("Walk", true);
        enemy.Animator.SetBool("Idle", false);

        MoveToNextPoint();

        yield return new WaitForSeconds(0.5f);

        isWaiting = false;
    }

    private void MoveToNextPoint()
    {
        currentPatrolPoint = (currentPatrolPoint + 1) % enemy.PatrolPoints.Length;
        enemy.Agent.destination = enemy.PatrolPoints[currentPatrolPoint].position;
    }
}
