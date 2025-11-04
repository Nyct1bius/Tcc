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

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (Vector3.Distance(boss.transform.position, boss.NextPoint.position) <= 5)
        {
            boss.SetCurrentPoint();
            stateMachine.ChangeState(new BossWaitingState(stateMachine, boss));
        }
        else
        {
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, boss.NextPoint.position, boss.Stats.MovementSpeed * Time.deltaTime);
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, boss.NextPoint.rotation, 10 * Time.deltaTime);
        }
    }


    public override void Exit()
    {
        base.Exit();
    }
}
