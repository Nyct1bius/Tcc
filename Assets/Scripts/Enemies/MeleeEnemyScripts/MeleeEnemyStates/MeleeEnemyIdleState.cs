using UnityEngine;

public class MeleeEnemyIdleState : MeleeEnemyState
{
    private MeleeEnemy enemy;

    public MeleeEnemyIdleState(MeleeEnemyStateMachine stateMachine, MeleeEnemy enemy) : base(stateMachine)
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

        if (enemy.RoomManager.EnemiesAlerted && !enemy.HasHyperarmor)
            stateMachine.ChangeState(new MeleeEnemyCombatState(stateMachine, enemy));
        if (enemy.RoomManager.EnemiesAlerted && enemy.HasHyperarmor)
            stateMachine.ChangeState(new MeleeMinibossCombatState(stateMachine, enemy));
    }
}
