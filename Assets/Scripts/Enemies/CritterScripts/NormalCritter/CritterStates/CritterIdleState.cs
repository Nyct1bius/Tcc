using UnityEngine;

public class CritterIdleState : CritterState
{
    private Critter critter;

    public CritterIdleState(CritterStateMachine stateMachine, Critter critter) : base(stateMachine)
    {
        this.critter = critter;
    }

    public override void Enter()
    {
        base.Enter();

        critter.Animator.SetBool("Walk", false);
        critter.Animator.SetBool("Idle", true);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (critter.RoomManager.EnemiesAlerted)
        {
            stateMachine.ChangeState(new CritterFleeState(stateMachine, critter));
        }
    }
}
