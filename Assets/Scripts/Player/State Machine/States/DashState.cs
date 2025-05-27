using System;
using UnityEngine;
using UnityEngine.EventSystems;
using PlayerState;
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
        _ctx.AnimationSystem.UpdateDash(true);
        ApllyDashForce();
        ApplyDashCameraShake();
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
        _ctx.AnimationSystem.UpdateDash(false);
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
    private void ApplyDashCameraShake()
    {
       
    }
    private void ApllyDashForce()
    {
        _ctx.Movement.CameraFowardXZ = new Vector3(_ctx.MainCameraRef.transform.forward.x, 0, _ctx.MainCameraRef.transform.forward.z).normalized;
        _ctx.Movement.CameraRightXZ = new Vector3(_ctx.MainCameraRef.transform.right.x, 0, _ctx.MainCameraRef.transform.right.z).normalized;
        _ctx.Movement.MoveDirection = _ctx.Movement.CameraRightXZ * _ctx.Movement.CurrentMovementInput.x + _ctx.Movement.CameraFowardXZ * _ctx.Movement.CurrentMovementInput.y;

        Vector3 defaultDashDir = (_ctx.MainCameraRef.transform.position - _ctx.Movement.PlayerTransform.position).normalized;
        defaultDashDir.y = 0f;

        Vector3 dashDir = _ctx.Movement.MoveDirection == Vector3.zero ? defaultDashDir.normalized : _ctx.Movement.MoveDirection.normalized;

        Quaternion _lookRotation = Quaternion.LookRotation(new Vector3(dashDir.x,0, dashDir.z));
        _ctx.Movement.PlayerTransform.rotation = Quaternion.Slerp(_ctx.Movement.PlayerTransform.rotation, _lookRotation, 1f);
        _ctx.Body.AddForce(dashDir * _ctx.Movement.DashVelocity, ForceMode.Impulse);
        _ctx.Movement.DashInCooldown = true;

        CameraShakeManager.CameraShakeFromProfile(_ctx.Movement.DashProfile, _ctx.CameraShakeSource);
    }
}
