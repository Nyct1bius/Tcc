using System;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerState;
public class WalkState : State
{
    public WalkState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) { }


    public override void Enter()
    {
    }
    public override void FixedDo()
    {
        HandleHorizontalMovement();
    }

    public override void Exit()
    {
        PlayerEvents.OnStopChickenSFX();
    }

    public override void Do()
    {
        if (_ctx.GameIsPaused)
        {
            PlayerEvents.OnStopChickenSFX();
            return;
        }
        CheckSwitchState();
        if (_ctx.Movement.IsGrounded)
        {
            PlayerEvents.OnChickenSFX();
            _ctx.AudioManager.SetChickenVelocity(Mathf.Clamp(_ctx.Body.linearVelocity.magnitude,1f,10f));
        }
        else
        {
            PlayerEvents.OnStopChickenSFX();
        }

    }

    public override void CheckSwitchState()
    {
        if (_ctx.Movement.CurrentMovementInput == Vector2.zero)
        {
            SwitchStates(_factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
       
    }

    private void HandleHorizontalMovement()
    {
        var cam = _ctx.MainCameraRef.transform;
        var move = _ctx.Movement;

        var camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        var camRight = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;

        move.MoveDirection = camRight * move.CurrentMovementInput.x + camForward * move.CurrentMovementInput.y;
        if (move.OnSlope())
            move.MoveDirection = move.GetSlopeDirection();

        move.MovementDelta = move.MoveDirection * move.WalkAcceleration;
        move.HorizontalVelocity += move.MovementDelta;

        if (_ctx.IsBlocking)
        {
            _ctx.AnimationSystem.UpdateShieldMovement(move.CurrentMovementInput);
            move.HorizontalVelocity = Vector3.ClampMagnitude(move.HorizontalVelocity, move.MaxWalkSpeed) * 0.5f;
            return;
        }

        _ctx.AnimationSystem.UpdateMovement(_ctx.Body.linearVelocity.magnitude);
        move.HorizontalVelocity = Vector3.ClampMagnitude(move.HorizontalVelocity, move.MaxWalkSpeed);
    }


}
