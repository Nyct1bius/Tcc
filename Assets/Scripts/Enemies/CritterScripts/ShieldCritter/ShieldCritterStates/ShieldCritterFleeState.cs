using System.Collections;
using UnityEngine;

public class ShieldCritterFleeState : ShieldCritterState
{
    private ShieldCritter critter;
    private Vector3 playerPosition;
    private bool leftSideChosen, chaseStarted = false, isMovingCoroutineRunning = false;

    public ShieldCritterFleeState(ShieldCritterStateMachine stateMachine, ShieldCritter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.CurrentPoint = critter.M1Point;
        critter.Agent.SetDestination(critter.CurrentPoint.position);
        critter.IsWaiting = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Vector3.Distance(critter.transform.position, critter.NextPoint.transform.position) <= 6 && !critter.IsWaiting)
        {
            critter.Agent.ResetPath();
            critter.CurrentPoint = critter.NextPoint;

            Debug.Log("Close");

            critter.IsWaiting = true;
        }
        if (Vector3.Distance(critter.transform.position, critter.NextPoint.transform.position) > 6 && !critter.IsWaiting)
        {
            critter.Agent.SetDestination(critter.NextPoint.position);

            Debug.Log("Far");

            critter.IsWaiting = false;
        }
    }

    private IEnumerator MoveAfterDelay(float delayTimer)
    {
        isMovingCoroutineRunning = true;

        yield return new WaitForSeconds(delayTimer);

        critter.Agent.SetDestination(critter.NextPoint.position);

        isMovingCoroutineRunning = false;
    }
}
