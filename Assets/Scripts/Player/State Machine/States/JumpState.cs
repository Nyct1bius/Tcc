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
    private bool _isJumpCanceled;
    public override void Enter()
    {

        HandleJump();
        PlayerEvents.OnJumpSFX();
        _timeSinceEntered = 0f;
        _ctx.AnimationSystem.Jump();
        _ctx.AnimationSystem.UpdateGrounded(false);
        _isJumpCanceled = false;
        _ctx.Movement.FallDeathTimer = 0f;
    }

    public override void Do() 
    {
        _timeSinceEntered += Time.deltaTime;
        _ctx.PlayerAnimator.SetFloat("YSpeed", _ctx.Body.linearVelocity.y);
        if (_timeSinceEntered >= _switchDelay)
        {
            CheckSwitchState();
        }

        if (!_ctx.Movement.HasGround())
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
        CheckIfStillJumping();
    }
    public override void Exit()
    {
        if (_ctx.Movement.IsJumpButtonPressed)
        {
            _ctx.Movement.RequireNewJumpPress = true;
        }
        _ctx.AnimationSystem.UpdateGrounded(true);
        CameraShakeManager.CameraShakeFromProfile(_ctx.Movement.LandProfile, _ctx.CameraShakeSource);
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
        if (!_ctx.Movement.IsJumpButtonPressed)
        {
            JumpButtonCanceled();
            _isJumpCanceled = true;
        }
        if (_ctx.Movement.ButtonPressedTime > _ctx.Movement.MaxJumpTime)
        {
            _isJumpCanceled = true;
        }


        CancelJump();
    }
    private void JumpButtonCanceled()
    {
        if (!_isJumpCanceled && _ctx.Body.linearVelocity.y > 0f)
        {
            _ctx.Body.linearVelocity = new Vector3(
                _ctx.Body.linearVelocity.x,
                _ctx.Body.linearVelocity.y * _ctx.Movement.ShortHopMultiplier, 
                _ctx.Body.linearVelocity.z
            );
            _isJumpCanceled = true;
        }
    }
    private void CancelJump()
    {
        if (_isJumpCanceled)
        _ctx.Body.AddForce(Vector3.up * _ctx.Movement.Gravity, ForceMode.Force);
    }

}
