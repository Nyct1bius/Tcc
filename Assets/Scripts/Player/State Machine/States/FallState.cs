using UnityEngine;

public class FallState : State
{
    public FallState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
    : base(contex, playerStateFactory)
    {
        isRootState = true;
        InitializeSubState();
    }
    private float _timeSinceEntered;
    private float _switchDelay = 0.3f;
    public override void Enter()
    {

        _timeSinceEntered = 0f;
        _ctx.Animator.SetBool("IsFalling", true);

    }

    public override void Do()
    {
        _timeSinceEntered += Time.deltaTime;
        if (_timeSinceEntered >= _switchDelay)
        {
            CheckSwitchState();
        }
    }


    public override void FixedDo()
    {
       
    }

    public override void InitializeSubState()
    {
        if (_ctx.CurrentMovementInput != Vector2.zero)
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            SetSubState(_factory.Idle());
        }
    }
    public override void CheckSwitchState()
    {
        if (_ctx.GroundSensor.IsGrounded())
        {
            SwitchStates(_factory.Grounded());
        }
    }

    public override void Exit()
    {
        _ctx.Animator.SetBool("IsFalling", false);

    }
}
