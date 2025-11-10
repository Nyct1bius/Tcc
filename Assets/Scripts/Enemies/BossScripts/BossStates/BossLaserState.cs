using UnityEngine;

public class BossLaserState : BossState
{
    private Boss boss;

    private Vector3 playerPos;
    private Vector3 targetPos;

    private bool canMove = true, checkPlayerPos = true;

    public BossLaserState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        Debug.Log("Laser State");

        if (!canMove)
            canMove = true;
        if (!checkPlayerPos)
            checkPlayerPos = true;

        boss.Animator.SetTrigger("Laser");
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        if (checkPlayerPos)
            GetTargetPosition();
        if (canMove)
            MovementLogic();
    }

    private void MovementLogic()
    {
        if (Vector3.Distance(boss.transform.position, targetPos) <= 2)
        {
            boss.Animator.SetBool("Idle", true);
            boss.Animator.SetBool("Moving", false);

            canMove = false;
        }
        else
        {
            boss.Animator.SetBool("Moving", true);
            boss.Animator.SetBool("Idle", false);

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, targetPos, 30 * Time.deltaTime);
            boss.transform.LookAt(targetPos);
        }
    }

    private void GetTargetPosition()
    {
        playerPos = new Vector3(boss.Player.transform.position.x, boss.transform.position.y, boss.Player.transform.position.z);
        Vector3 direction = playerPos - boss.transform.position;
        targetPos = boss.transform.position + direction * 2f;

        checkPlayerPos = false;
    }
}
