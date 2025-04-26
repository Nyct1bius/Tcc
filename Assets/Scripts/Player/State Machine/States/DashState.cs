using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DashState : State
{
    float currentTime;
    public DashState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) 
    {
        isRootState = true;
    }
    public override void Enter()
    {
        currentTime = 0f;
        _ctx.Animator.SetBool("IsDashing", true);
        ApllyDashForce();
    }

    private void ApllyDashForce()
    {
        _ctx.Movement.CameraFowardXZ = new Vector3(_ctx.MainCameraRef.transform.forward.x, 0, _ctx.MainCameraRef.transform.forward.z).normalized;
        _ctx.Movement.CameraRightXZ = new Vector3(_ctx.MainCameraRef.transform.right.x, 0, _ctx.MainCameraRef.transform.right.z).normalized;
        _ctx.Movement.MoveDirection = _ctx.Movement.CameraRightXZ * _ctx.Movement.CurrentMovementInput.x + _ctx.Movement.CameraFowardXZ * _ctx.Movement.CurrentMovementInput.y;

        Vector3 dashDir = _ctx.Movement.MoveDirection == Vector3.zero ? _ctx.Movement.PlayerTransform.forward : _ctx.Movement.MoveDirection;
        _ctx.Body.AddForce(dashDir.normalized * _ctx.Movement.DashVelocity, ForceMode.Impulse);
        _ctx.Movement.DashInCooldown = true;
    }

    public override void Do()
    {
        CheckSwitchState();
    }
    public override void FixedDo()
    {
    }
    public override void Exit() 
    {
        _ctx.Animator.SetBool("IsDashing", false);
        _ctx.Movement.ResetDash();
    }

    public override void CheckSwitchState()
    {
        currentTime += Time.deltaTime;

        if (currentTime > _ctx.Movement.DashTime)
        {
            SwitchStates(_factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        
    }
}
