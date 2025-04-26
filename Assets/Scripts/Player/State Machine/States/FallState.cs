using UnityEngine;
using System;

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

        _ctx.Animator.SetTrigger("OnAir");
        _ctx.Animator.SetBool("IsGrounded",false);
    }

    public override void Do()
    {
        _timeSinceEntered += Time.deltaTime;
        if (_timeSinceEntered >= _switchDelay)
        {
            CheckSwitchState();
        }
        _ctx.Animator.SetFloat("YSpeed", _ctx.Body.linearVelocity.y);
    }


    public override void FixedDo()
    {
       
        _ctx.Body.AddForce(Vector3.up * _ctx.Movement.Gravity, ForceMode.Force);
    }

    public override void InitializeSubState()
    {
        if (_ctx.Movement.CurrentMovementInput != Vector2.zero)
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
        _ctx.Animator.SetBool("IsGrounded", true);

    }
}
