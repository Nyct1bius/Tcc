using System;
using UnityEngine;
using PlayerState;
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
        _ctx.Animator.SetFloat("YSpeed", _ctx.Body.linearVelocity.y);
        if (_timeSinceEntered >= _switchDelay)
        {
            CheckSwitchState();
        }
    }
    public override void FixedDo()
    {
        CheckIfStillJumping();
    }
    public override void Exit()
    {
        if (_ctx.Movement.IsJumpButtonPressed)
        {
            _ctx.Movement.RequireNewJumpPress = true;
        }
        _ctx.Animator.SetBool("IsGrounded", true);
    }

    public override void CheckSwitchState()
    {
        if (_ctx.GroundSensor.IsGrounded())
        {
            SwitchStates(_factory.Grounded());
        }
        if (_ctx.Health.IsDamaged)
        {
            SwitchStates(_factory.Damaged());
        }
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

    private void HandleJump()
    {
        _ctx.Body.AddForce(Vector3.up * _ctx.Movement.JumpVelocity, ForceMode.Impulse);
        _ctx.Movement.ButtonPressedTime = 0;
    }

    public void CheckIfStillJumping()
    {

        _ctx.Movement.ButtonPressedTime += Time.deltaTime;

        if (_ctx.Movement.ButtonPressedTime > _ctx.Movement.MaxJumpTime || !_ctx.Movement.IsJumpButtonPressed)
        {
            CancelJump();
        }



    }
    private void CancelJump()
    {
        _ctx.Body.AddForce(Vector3.up * _ctx.Movement.Gravity, ForceMode.Force);
    }

}
