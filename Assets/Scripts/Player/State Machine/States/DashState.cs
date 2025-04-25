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
        _ctx.CameraFowardXZ = new Vector3(_ctx.MainCameraRef.transform.forward.x, 0, _ctx.MainCameraRef.transform.forward.z).normalized;
        _ctx.CameraRightXZ = new Vector3(_ctx.MainCameraRef.transform.right.x, 0, _ctx.MainCameraRef.transform.right.z).normalized;
        _ctx.MoveDirection = _ctx.CameraRightXZ * _ctx.CurrentMovementInput.x + _ctx.CameraFowardXZ * _ctx.CurrentMovementInput.y;
        Vector3 dashDir = _ctx.MoveDirection == Vector3.zero ? _ctx.PlayerTransform.forward : _ctx.MoveDirection;
        _ctx.Body.AddForce(dashDir.normalized * _ctx.DashVelocity, ForceMode.Impulse);
        _ctx.DashInCooldown = true;
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
        _ctx.ResetDash();
    }

    public override void CheckSwitchState()
    {
        currentTime += Time.deltaTime;

        if (currentTime > _ctx.DashTime)
        {
            SwitchStates(_factory.Grounded());
        }
    }

    public override void InitializeSubState()
    {
        
    }
}
