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
        boss.NextPoint = boss.MovementPoints[1];

        yield return new WaitForSeconds(1);

        canMove = true;
    }

    private void MovementLogic()
    {
        if (Vector3.Distance(boss.transform.position, boss.NextPoint.position) <= 2)
        {
            boss.SetCurrentPoint();
            canMove = false;
        }
        else
        {
            boss.transform.position = Vector3.MoveTowards(boss.transform.position, boss.NextPoint.position, boss.Stats.MovementSpeed * Time.deltaTime);
            boss.transform.rotation = Quaternion.Slerp(boss.transform.rotation, boss.NextPoint.rotation, 10 * Time.deltaTime);
        }
    }
}
