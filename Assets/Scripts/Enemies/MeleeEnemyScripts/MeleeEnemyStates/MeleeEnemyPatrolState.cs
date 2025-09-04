using UnityEngine;

public class MeleeEnemyPatrolState : EnemyState
{
    private MeleeEnemy enemy;

    private int currentPatrolPoint = 0;

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
    }

    private void MoveToNextPoint()
    {
        enemy.Agent.destination = enemy.PatrolPoints[currentPatrolPoint].position;

        currentPatrolPoint = (currentPatrolPoint + 1) % enemy.PatrolPoints.Length;
    }
}
