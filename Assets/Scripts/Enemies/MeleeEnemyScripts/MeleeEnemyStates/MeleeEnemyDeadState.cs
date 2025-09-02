using UnityEngine;

public class MeleeEnemyDeadState : EnemyState
{
    public MeleeEnemyDeadState(MeleeEnemyStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        // Handle death (disable AI, play death animation)
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        // Usually nothing here since enemy is dead
    }
}
