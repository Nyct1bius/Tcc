using UnityEngine;
using PlayerState;

public class DashState : State
{
    private float currentTime;
    private Vector3 dashDir;
    private Vector3 startPos;
    private Vector3 targetPos;
    private bool hitWall;

    public DashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        isRootState = true;
    }

    public override void Enter()
    {
        currentTime = 0f;
        hitWall = false;

        var cam = _ctx.MainCameraRef.transform;
        var move = _ctx.Movement;

        var camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        var camRight = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
        var moveDir = camRight * move.CurrentMovementInput.x + camForward * move.CurrentMovementInput.y;

        dashDir = moveDir.sqrMagnitude > 0.01f ? moveDir.normalized : move.PlayerTransform.forward;
        dashDir.y = 0f;
        dashDir.Normalize();

        move.PlayerTransform.rotation = Quaternion.LookRotation(dashDir);
        move.DashInCooldown = true;

        startPos = move.PlayerTransform.position;
        targetPos = startPos + dashDir * move.DashDistance;

        _ctx.PlayerAnimator.SetBool("IsDashing", true);
        PlayerEvents.OnDashSFX();
        CameraShakeManager.CameraShakeFromProfile(move.DashProfile, _ctx.CameraShakeSource);
    }

    public override void FixedDo()
    {
        var move = _ctx.Movement;

        if (hitWall)
        {
            SwitchStates(_factory.Grounded());
            return;
        }

        currentTime += Time.fixedDeltaTime;
        float t = currentTime / move.DashTime;
        float speedFactor = move.DashSpeedCurve.Evaluate(t);

        Vector3 nextPos = Vector3.Lerp(startPos, targetPos, speedFactor);

        float checkDist = Vector3.Distance(move.PlayerTransform.position, nextPos);
        if (Physics.Raycast(move.PlayerTransform.position, dashDir, out RaycastHit hit, checkDist + 0.1f, move.DashCollisionMask))
        {
            move.PlayerTransform.position = hit.point - dashDir * 0.05f;
            hitWall = true;
            return;
        }

        move.PlayerTransform.position = nextPos;

        if (t >= 1f)
            SwitchStates(_factory.Grounded());
    }

    public override void Exit()
    {
        var move = _ctx.Movement;
        _ctx.PlayerAnimator.SetBool("IsDashing", false);
        move.ResetDash();
    }

    public override void Do() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
