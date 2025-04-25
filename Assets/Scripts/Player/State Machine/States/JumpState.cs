using System;
using UnityEngine;

public class JumpState : State
{
    public JumpState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) {
        isRootState = true;
        InitializeSubState();
    }

    private float _timeSinceEntered;
    private float _switchDelay = 0.3f;
    public override void Enter()
    {

        HandleJump();
        _timeSinceEntered = 0f;
        _ctx.Animator.SetTrigger("OnAir");
        _ctx.Animator.SetBool("IsGrounded", false);
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
        CheckIfStillJumping();
    }
    public override void Exit()
    {
        if (_ctx.IsJumpButtonPressed)
        {
            _ctx.RequireNewJumpPress = true;
        }
        _ctx.Animator.SetBool("IsGrounded", true);
    }

    public override void CheckSwitchState()
    {
        if (_ctx.GroundSensor.IsGrounded())
        {
            SwitchStates(_factory.Grounded());
        }
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

    private void HandleJump()
    {
        _ctx.Body.AddForce(Vector3.up * _ctx.JumpVelocity, ForceMode.Impulse);
        _ctx.ButtonPressedTime = 0;
    }

    public void CheckIfStillJumping()
    {

        _ctx.ButtonPressedTime += Time.deltaTime;

        if (_ctx.ButtonPressedTime > _ctx.MaxJumpTime || !_ctx.IsJumpButtonPressed)
        {
            CancelJump();
        }



    }
    private void CancelJump()
    {
        _ctx.Body.AddForce(Vector3.up * _ctx.Gravity, ForceMode.Force);
    }

}
