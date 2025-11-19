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

        if (boss.BossCollider.enabled)
            boss.DisableCollider();

        boss.StartCoroutine(LoadCreditsScene());
    }

    private IEnumerator LoadCreditsScene()
    {
        yield return new WaitForSeconds(5f);
        boss.BossDeathScript.LoadCreditsScene();
    }
}
