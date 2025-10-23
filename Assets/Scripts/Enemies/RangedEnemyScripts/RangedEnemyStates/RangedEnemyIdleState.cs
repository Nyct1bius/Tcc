using UnityEngine;

public class RangedEnemyIdleState : RangedEnemyState
{
    private RangedEnemy enemy;

    public RangedEnemyIdleState(RangedEnemyStateMachine stateMachine, RangedEnemy enemy) : base(stateMachine)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemy.Animator.SetBool("Walk", false);
        enemy.Animator.SetBool("Idle", true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (enemy.RoomManager.EnemiesAlerted)
            stateMachine.ChangeState(new RangedEnemyCombatState(stateMachine, enemy));
    }
}
