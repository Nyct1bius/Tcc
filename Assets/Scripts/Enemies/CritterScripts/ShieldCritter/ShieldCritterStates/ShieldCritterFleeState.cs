using System.Collections;
using UnityEngine;

public class ShieldCritterFleeState : ShieldCritterState
{
    private ShieldCritter critter;

    private Vector3 playerPosition;

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

        critter.Animator.SetBool("Running", true);
        critter.Animator.SetBool("Idle", false);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

         playerPosition = new Vector3(critter.Player.transform.position.x, critter.transform.position.y, critter.Player.transform.position.z);

        if (Vector3.Distance(critter.transform.position, playerPosition) <= 5 && !critter.IsWaiting)
        {
            critter.CurrentPoint = critter.NextPoint;

            critter.Animator.SetBool("Tremble", true);
            critter.Animator.SetBool("Run", false);

            critter.transform.LookAt(playerPosition);

            critter.IsWaiting = true;
        }
        if (Vector3.Distance(critter.transform.position, playerPosition) > 5 && !critter.IsWaiting)
        {
            critter.Agent.SetDestination(critter.NextPoint.position);

            critter.Animator.SetBool("Run", true);
            critter.Animator.SetBool("Tremble", false);
        }
    }
}
