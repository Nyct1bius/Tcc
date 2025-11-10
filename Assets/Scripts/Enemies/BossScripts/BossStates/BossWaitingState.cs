using System.Collections;
using UnityEngine;

public class BossWaitingState : BossState
{
    private Boss boss;

    private bool canMove = false;

    public BossWaitingState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        boss.Animator.SetBool("Idle", true);
        boss.Animator.SetBool("Moving", false);

        boss.StartCoroutine(SetNextPointAndMove());
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (canMove)
        {
            MovementLogic();
        }
    }

    public IEnumerator SetNextPointAndMove()
    {
        boss.NextPoint = boss.MovementPoints[Random.Range(0, 3)];

        yield return new WaitForSeconds(3);

        canMove = true;
    }

    private void MovementLogic()
    {
        if (boss.IsCloseToTarget(boss.NextPoint.position))
        {
            boss.SetCurrentPoint();
            canMove = false;

            boss.Animator.SetBool("Idle", true);
            boss.Animator.SetBool("Moving", false);

            boss.StartCoroutine(ChangeState(1f, Random.Range(0, 2)));
        }
        else
        {
            boss.Animator.SetBool("Moving", true);
            boss.Animator.SetBool("Idle", false);

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, boss.NextPoint.position, boss.Stats.MovementSpeed * Time.deltaTime);
            boss.RotateTowards(boss.NextPoint.position);
        }
    }

    private IEnumerator ChangeState(float timer, int stateIndex)
    {
        yield return new WaitForSeconds(timer);

        if (stateIndex == 0)
            stateMachine.ChangeState(new BossBounceState(stateMachine, boss));
        if (stateIndex == 1)
            stateMachine.ChangeState(new BossBounceState(stateMachine, boss)); //Change to laser state later.
    }
}
