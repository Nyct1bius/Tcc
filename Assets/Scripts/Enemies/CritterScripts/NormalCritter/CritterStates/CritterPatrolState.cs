using System.Collections;
using UnityEngine;

public class CritterPatrolState : CritterState
{
    private Critter critter;

    private int currentPatrolPoint = 0;
    private bool isWaiting = false;

    private Coroutine patrolCoroutine;

    public CritterPatrolState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (critter.Agent.remainingDistance <= critter.Agent.stoppingDistance && !isWaiting)
            StartPatrolCoroutine();

        if (critter.Stats.Health <= 0)
            stateMachine.ChangeState(new CritterDeadState(stateMachine, critter));
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;

        critter.Animator.SetBool("Idle", true);
        critter.Animator.SetBool("Walk", false);

        critter.Agent.ResetPath();

        yield return new WaitForSeconds(2f);

        critter.Animator.SetBool("Walk", true);
        critter.Animator.SetBool("Idle", false);

        MoveToNextPoint();

        yield return new WaitForSeconds(0.5f);

        isWaiting = false;
    }

    private void MoveToNextPoint()
    {
        currentPatrolPoint = (currentPatrolPoint + 1) % critter.PatrolPoints.Length;
        critter.Agent.destination = critter.PatrolPoints[currentPatrolPoint].position;
    }

    public override void Exit()
    {
        base.Exit();

        StopPatrolCoroutine();

        if (isWaiting)
            isWaiting = false;
    }

    private void StartPatrolCoroutine()
    {
        patrolCoroutine = critter.StartCoroutine(WaitAndMove());
    }

    private void StopPatrolCoroutine()
    {
        if (patrolCoroutine != null)
        {
            critter.StopCoroutine(patrolCoroutine);
        }
    }
}
