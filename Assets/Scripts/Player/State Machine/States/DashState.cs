using UnityEngine;
using PlayerState;

public class DashState : State
{
    float currentTime;
    Vector3 dashDir;
    float currentSpeed;

    public DashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        isRootState = true;
    }

    public override void Enter()
    {
        currentTime = 0f;
        _ctx.AnimationSystem.UpdateDash(true);
        PlayerEvents.OnDashSFX();

        _ctx.Body.linearVelocity = Vector3.zero;
        SetupDashDirection();
        _ctx.Movement.DashInCooldown = true;

        CameraShakeManager.CameraShakeFromProfile(_ctx.Movement.DashProfile, _ctx.CameraShakeSource);
    }

    public override void Do()
    {
        CheckSwitchState();
    }

    public override void FixedDo()
    {
        var move = _ctx.Movement;
        currentTime += Time.fixedDeltaTime;

        float normalizedTime = currentTime / move.DashTime;
        float curveValue = move.DashSpeedCurve.Evaluate(normalizedTime);
        currentSpeed = move.DashVelocity * curveValue;

        Vector3 step = dashDir * currentSpeed * Time.fixedDeltaTime;
        Vector3 start = _ctx.Body.position;
        float checkDistance = step.magnitude + 0.3f;

        if (Physics.Raycast(start, dashDir, out RaycastHit hit, checkDistance, move.DashCollisionMask))
        {
            _ctx.Body.position = hit.point - dashDir * 0.1f;
            SwitchStates(_factory.Grounded());
            return;
        }

        _ctx.Body.MovePosition(start + step);

        if (currentTime >= move.DashTime)
            SwitchStates(_factory.Grounded());
    }

    public override void Exit()
    {
        _ctx.AnimationSystem.UpdateDash(false);
        _ctx.Movement.ResetDash();
    }

    public override void CheckSwitchState()
    {
        if (currentTime >= _ctx.Movement.DashTime)
            SwitchStates(_factory.Grounded());
    }

    public override void InitializeSubState() { }

    private void SetupDashDirection()
    {
        var cam = _ctx.MainCameraRef.transform;
        var move = _ctx.Movement;

        var camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        var camRight = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;

        var moveDir = camRight * move.CurrentMovementInput.x + camForward * move.CurrentMovementInput.y;
        dashDir = moveDir == Vector3.zero
            ? (cam.position - move.PlayerTransform.position).normalized
            : moveDir.normalized;

        dashDir.y = 0f;
        dashDir.Normalize();

        if (Physics.Raycast(move.PlayerTransform.position, dashDir, out RaycastHit hit, 0.5f, move.DashCollisionMask))
        {
            SwitchStates(_factory.Grounded());
            return;
        }

        move.PlayerTransform.rotation = Quaternion.LookRotation(dashDir);
    }
}
