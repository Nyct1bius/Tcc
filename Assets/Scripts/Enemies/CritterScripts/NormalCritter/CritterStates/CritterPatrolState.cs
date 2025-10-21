using System.Collections;
using UnityEngine;

public class CritterPatrolState : CritterState
{
    private Critter critter;

    private int currentPatrolPoint = 0;
    private bool isWaiting = false;

    public CritterPatrolState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.StopAllCoroutines();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!critter.Agent.pathPending && critter.Agent.remainingDistance <= critter.Agent.stoppingDistance && !isWaiting)
            critter.StartCoroutine(WaitAndMove());
    }

    private void MoveToNextPoint()
    {
        if (critter.PatrolPoints == null || critter.PatrolPoints.Length == 0)
            return;

        critter.Agent.destination = critter.PatrolPoints[currentPatrolPoint].position;

        currentPatrolPoint = (currentPatrolPoint + 1) % critter.PatrolPoints.Length;
    }

    private IEnumerator WaitAndMove()
    {
        isWaiting = true;

        critter.Animator.SetBool("Walk", false);
        critter.Animator.SetBool("Idle", true);

        critter.Agent.ResetPath();

        yield return new WaitForSeconds(2f);

        critter.Animator.SetBool("Idle", false);
        critter.Animator.SetBool("Walk", true);

        MoveToNextPoint();

        isWaiting = false;
    }
}
