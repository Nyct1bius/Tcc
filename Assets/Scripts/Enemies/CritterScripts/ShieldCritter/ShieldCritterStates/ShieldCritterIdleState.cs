using UnityEngine;

public class ShieldCritterIdleState : ShieldCritterState
{
    private ShieldCritter critter;

    public ShieldCritterIdleState(ShieldCritterStateMachine stateMachine, ShieldCritter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        //critter.Animator.SetBool("Walk", false);
        //critter.Animator.SetBool("Idle", true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (critter.RoomManager.EnemiesAlerted)
        {
            stateMachine.ChangeState(new ShieldCritterFleeState(stateMachine, critter));
        }
    }
}
