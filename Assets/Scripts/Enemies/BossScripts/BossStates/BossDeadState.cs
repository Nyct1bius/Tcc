using System.Collections;
using UnityEngine;

public class BossDeadState : BossState
{
    private Boss boss;

    public BossDeadState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        boss.StopAllCoroutines();
        boss.Animator.SetTrigger("Die");

        if (boss.bossCollider.enabled)
            boss.DisableCollider();

        boss.enabled = false;
        boss.Stats.enabled = false;
    }
}
