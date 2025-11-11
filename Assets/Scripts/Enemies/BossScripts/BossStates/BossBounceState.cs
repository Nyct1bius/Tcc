using System.Collections;
using UnityEngine;

public class BossBounceState : BossState
{
    private Boss boss;
    private Vector3 playerPos;
    private bool canMove = true, canAttack = false;
    private Coroutine bounceCoroutine;
    private int bounceCounter;

    public BossBounceState(BossStateMachine stateMachine, Boss boss) : base(stateMachine)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        if (!canMove)
            canMove = true;
        if (bounceCounter != 0)
            bounceCounter = 0;

        boss.Animator.SetBool("Moving", true);
        boss.Animator.SetBool("Idle", false);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

        CheckHealth();

        if (canMove)
        {
            playerPos = new Vector3(boss.Player.transform.position.x, boss.transform.position.y, boss.Player.transform.position.z);
            MovementLogic();
        }
        if (canAttack)
        {
            StartBounceCoroutine();
        }
    }

    public override void Exit()
    {
        base.Exit();

        if (canAttack)
            canAttack = false;
    }

    private void MovementLogic()
    {
        if (Vector3.Distance(boss.transform.position, playerPos) <= 2)
        {
            boss.Animator.SetBool("Idle", true);
            boss.Animator.SetBool("Moving", false);

            canAttack = true;
            canMove = false;
        }
        else
        {
            boss.Animator.SetBool("Moving", true);
            boss.Animator.SetBool("Idle", false);

            boss.transform.position = Vector3.MoveTowards(boss.transform.position, playerPos, boss.Stats.MovementSpeed * Time.deltaTime);
            boss.RotateTowards(playerPos);
        }
    }

    private void StartBounceCoroutine()
    {
        bounceCoroutine = boss.StartCoroutine(BounceAttack());
    }
    private void StopBounceCoroutine()
    {
        if (bounceCoroutine != null)
        {
            boss.StopCoroutine(bounceCoroutine);
        }
    }

    private IEnumerator BounceAttack()
    {
        canAttack = false;

        boss.Animator.SetTrigger("Bounce");

        yield return new WaitForSeconds(7.5f);

        if (bounceCounter < 3)
        {
            canMove = true;
            bounceCounter++;
        }
        else
            stateMachine.ChangeState(new BossWaitingState(stateMachine, boss));
    }

    private void CheckHealth()
    {
        if (boss.Stats.Health <= 0)
        {
            StopBounceCoroutine();
            stateMachine.ChangeState(new BossDeadState(stateMachine, boss));
        }
    }
}
