using System.Collections;
using UnityEngine;

public class ShieldCritterCorneredState : ShieldCritterState
{
    private ShieldCritter critter;

    private Vector3 playerPosition;

    public ShieldCritterCorneredState(ShieldCritterStateMachine stateMachine, ShieldCritter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.Agent.ResetPath();
        critter.GetComponent<CapsuleCollider>().enabled = false;
        critter.StartThrowShieldCoroutine();
    }

    public override void FixedUpdateLogic()
    {
        base.FixedUpdateLogic();

        playerPosition = new Vector3(critter.Player.transform.position.x, critter.transform.position.y, critter.Player.transform.position.z);

        critter.transform.LookAt(playerPosition);
    }
}
