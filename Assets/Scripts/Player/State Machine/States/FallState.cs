using UnityEngine;
using System;
using PlayerState;
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

        _ctx.AnimationSystem.Jump();
        _ctx.AnimationSystem.UpdateGrounded(false);
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
            Debug.Log("Death counter Start");
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

        if (_ctx.Health.IsDamaged)
        {
            SwitchStates(_factory.Damaged());
        }
    }

    public override void Exit()
    {
        _ctx.AnimationSystem.UpdateGrounded(true);
        CameraShakeManager.CameraShakeFromProfile(_ctx.Movement.LandProfile, _ctx.CameraShakeSource);

    }
}
