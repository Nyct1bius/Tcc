using System.Collections;
using UnityEngine;

public class CritterFleeState : CritterState
{
    private Critter critter;

    public CritterFleeState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
    }
}
