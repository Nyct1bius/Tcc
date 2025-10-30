using UnityEngine;

public class BossIntroState : BossState
{
    private Boss boss;

    public BossIntroState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        boss.Animator.SetBool("Moving", true);
        boss.Animator.SetBool("Idle", false);
    }
}
