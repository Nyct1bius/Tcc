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

    }

    public override void Do()
    {
        CheckSwitchState();

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

    public void HandleHorizontalMovement()
    {
       _ctx.Movement.CameraFowardXZ = new Vector3(_ctx.MainCameraRef.transform.forward.x, 0, _ctx.MainCameraRef.transform.forward.z).normalized;
        _ctx.Movement.CameraRightXZ = new Vector3(_ctx.MainCameraRef.transform.right.x, 0, _ctx.MainCameraRef.transform.right.z).normalized;
        _ctx.Movement.MoveDirection = _ctx.Movement.CameraRightXZ * _ctx.Movement.CurrentMovementInput.x + _ctx.Movement.CameraFowardXZ * _ctx.Movement.CurrentMovementInput.y;
        if (_ctx.Movement.OnSlope())
        {
            _ctx.Movement.MoveDirection = _ctx.Movement.GetSlopeDirection();
        }


        _ctx.Movement.MovementDelta = _ctx.Movement.MoveDirection * _ctx.Movement.WalkAcceleration;
        _ctx.Movement.HorizontalVelocity += _ctx.Movement.MovementDelta;
        _ctx.Movement.HorizontalVelocity = Vector3.ClampMagnitude(_ctx.Movement.HorizontalVelocity, _ctx.Movement.MaxWalkSpeed);
        _ctx.AnimationSystem.UpdateMovement(_ctx.Body.linearVelocity.magnitude);
    }

}
