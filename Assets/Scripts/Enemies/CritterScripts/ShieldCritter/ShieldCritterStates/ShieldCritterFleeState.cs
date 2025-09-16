using UnityEngine;

public class ShieldCritterFleeState : ShieldCritterState
{
    private ShieldCritter critter;

    private Vector3 playerPosition;

    private bool leftSideChosen, chaseStarted = false;

    public ShieldCritterFleeState(ShieldCritterStateMachine stateMachine, ShieldCritter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.Agent.SetDestination(critter.M1Point.position);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (critter.Agent.remainingDistance < critter.Agent.stoppingDistance)
        {
            critter.IsWaiting = true;
        }
    }
}
