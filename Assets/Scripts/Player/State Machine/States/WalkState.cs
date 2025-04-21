using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class WalkState : State
{
    public WalkState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) { }


    public override void Enter()
    {
         
    }
    public override void FixedDo()
    {
        _ctx.Animator.SetBool("IsMoving", true);
        HandleHorizontalMovement();
    }

    public override void Exit()
    {
        _ctx.Animator.SetBool("IsMoving", false);

    }

    public override void Do()
    {
        CheckSwitchState();

    }

    public override void CheckSwitchState()
    {
        if (_ctx.CurrentMovementInput == Vector2.zero)
        {
            SwitchStates(_factory.Idle());
        }
    }

    public override void InitializeSubState()
    {
       
    }

    public void HandleHorizontalMovement()
    {
       _ctx.CameraFowardXZ = new Vector3(_ctx.MainCameraRef.transform.forward.x, 0, _ctx.MainCameraRef.transform.forward.z).normalized;
        _ctx.CameraRightXZ = new Vector3(_ctx.MainCameraRef.transform.right.x, 0, _ctx.MainCameraRef.transform.right.z).normalized;
        _ctx.MoveDirection = _ctx.CameraRightXZ * _ctx.CurrentMovementInput.x + _ctx.CameraFowardXZ * _ctx.CurrentMovementInput.y;

        _ctx.MovementDelta = _ctx.MoveDirection * _ctx.WalkAcceleration;
        _ctx.HorizontalVelocity += _ctx.MovementDelta;
        _ctx.HorizontalVelocity = Vector3.ClampMagnitude(_ctx.HorizontalVelocity, _ctx.MaxWalkSpeed);
    }

}
