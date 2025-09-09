using System.Collections;
using UnityEngine;

public class CritterFleeState : CritterState
{
    private Critter critter;

    private int currentFleePoint = 0;

    private bool isWaiting = false;

    public CritterFleeState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        currentFleePoint = 0;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (!critter.Agent.pathPending && critter.Agent.remainingDistance <= critter.Agent.stoppingDistance && !isWaiting)
            critter.StartCoroutine(WaitAndMove());
    }

    private void MoveToNextPoint()
    {
        if (critter.FleePoints == null || critter.FleePoints.Length == 0)
            return;

        critter.Agent.destination = critter.FleePoints[currentFleePoint].position;

        currentFleePoint = (currentFleePoint + 1) % critter.FleePoints.Length;
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
