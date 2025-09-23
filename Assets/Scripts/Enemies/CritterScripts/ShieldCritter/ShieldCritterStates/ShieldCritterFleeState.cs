using System.Collections;
using UnityEngine;

public class ShieldCritterFleeState : ShieldCritterState
{
    private ShieldCritter critter;

    public ShieldCritterFleeState(ShieldCritterStateMachine stateMachine, ShieldCritter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.NextPoint = critter.M1Point;
        critter.CurrentPoint = critter.NextPoint;
        critter.Agent.SetDestination(critter.CurrentPoint.position);
        critter.IsWaiting = false;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Vector3.Distance(critter.transform.position, critter.NextPoint.transform.position) <= 10 && !critter.IsWaiting)
        {
            critter.Agent.ResetPath();
            critter.CurrentPoint = critter.NextPoint;

            critter.IsWaiting = true;
        }
        if (Vector3.Distance(critter.transform.position, critter.NextPoint.transform.position) > 10 && !critter.IsWaiting)
        {
            critter.Agent.SetDestination(critter.NextPoint.position);

            critter.IsWaiting = false;
        }
    }
}
