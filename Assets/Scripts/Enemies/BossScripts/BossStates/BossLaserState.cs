using System.Collections;
using UnityEngine;

public class BossLaserState : BossState
{
    private Boss boss;

    private Vector3 playerPos, targetPos;

    private bool canMove = true, checkPlayerPos = true;

    public BossLaserState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        if (!canMove)
            canMove = true;
        if (!checkPlayerPos)
            checkPlayerPos = true;

        boss.Stats.AttackMovementSpeed = 0f;
        boss.Animator.SetTrigger("Laser");

        boss.StartCoroutine(ChangeState());
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (checkPlayerPos)
            SetTargetPosition(2f);
        if (canMove)
            MovementLogic();
    }

    private void MovementLogic()
    {
        if (boss.IsCloseToTarget(targetPos))
        {
            canMove = false;
        }
        else
        {
            if (boss.Stats.AttackMovementSpeed != 0)
            {
                boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPos, boss.Stats.AttackMovementSpeed * Time.deltaTime);
            }
            boss.RotateTowards(targetPos);
        }
    }

    private void SetTargetPosition(float distanceMultiplier)
    {
        playerPos = new Vector3(boss.Player.transform.position.x, boss.transform.position.y, boss.Player.transform.position.z);
        Vector3 direction = playerPos - boss.transform.position;
        targetPos = boss.transform.position + direction * distanceMultiplier;

        checkPlayerPos = false;
    }

    private IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(8);

        stateMachine.ChangeState(new BossWaitingState(stateMachine, boss));
    }
}
