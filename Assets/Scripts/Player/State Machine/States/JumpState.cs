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
        PlayerEvents.OnJumpSFX();
        _timeSinceEntered = 0f;
        _ctx.AnimationSystem.Jump();
        _ctx.AnimationSystem.UpdateGrounded(false);
        _ctx.Movement.FallDeathTimer = 0f;
    }

    public override void Do() 
    {
        _timeSinceEntered += Time.deltaTime;
        if (_timeSinceEntered >= _switchDelay)
        {
            CheckSwitchState();
        }

        if (!_ctx.Movement.IsGroundAtLandingPoint())
        {
            _ctx.Movement.FallDeathTimer += Time.deltaTime;
            if (_ctx.Movement.FallDeathTimer >= _ctx.Movement.MaxFallTime)
            {
                GameManager.instance.RespawnPlayer();
                SwitchStates(_factory.Grounded());
            }

        }
    }
    public override void FixedDo()
    {
        if (_ctx.GameIsPaused) return;
        _ctx.AnimationSystem.UpdateJump(_ctx.Body.linearVelocity.y);
    }
    public override void Exit()
    {
        _ctx.AnimationSystem.UpdateGrounded(true);
        CameraShakeManager.CameraShakeFromProfile(_ctx.Movement.LandProfile, _ctx.CameraShakeSource);
        PlayerEvents.OnLandSFX();
    }

    public override void CheckSwitchState()
    {
        if (_ctx.Movement.IsGrounded)
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

        var currentVerticalSpeed = Vector3.Dot(_ctx.Body.linearVelocity, _ctx.transform.up);
        var targetVerticalSpeed = Mathf.Max(currentVerticalSpeed, _ctx.Movement.JumpVelocity);
        _ctx.Body.linearVelocity += _ctx.transform.up * (targetVerticalSpeed - currentVerticalSpeed);
        _ctx.Movement.RequireNewJumpPress = true;

    }
}
