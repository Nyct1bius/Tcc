using UnityEngine;

public class BossDiveState : BossState
{
    private Boss boss;

    private Vector3 playerPos, targetPos;

    private bool canMove = true, checkPlayerPos = true;

    private float attackMovementSpeed;

    public BossDiveState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        attackMovementSpeed = boss.Stats.MovementSpeed * 2;

        if (!canMove)
            canMove = true;
        if (!checkPlayerPos)
            checkPlayerPos = true;

        boss.Animator.SetTrigger("Dive");
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (checkPlayerPos)
            SetTargetPosition(1.5f);
        if (canMove)
            MovementLogic();
    }

    private void MovementLogic()
    {
        if (boss.IsCloseToTarget(targetPos))
        {
            boss.Animator.SetBool("Idle", true);
            boss.Animator.SetBool("Moving", false);

            canMove = false;
        }
        else
        {
            boss.Animator.SetBool("Moving", true);
            boss.Animator.SetBool("Idle", false);

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPos, attackMovementSpeed * Time.deltaTime);
            boss.transform.LookAt(targetPos);
        }
    }

    private void SetTargetPosition(float distanceMultiplier)
    {
        playerPos = new Vector3(boss.Player.transform.position.x, boss.transform.position.y, boss.Player.transform.position.z);
        Vector3 direction = playerPos - boss.transform.position;
        targetPos = boss.transform.position + direction * distanceMultiplier;

        checkPlayerPos = false;
    }
}
