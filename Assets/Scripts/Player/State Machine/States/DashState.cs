using UnityEngine;
using PlayerState;

public class DashState : State
{
    private float currentTime;
    private Vector3 dashDir;
    private bool hitWall;

    public DashState(PlayerStateMachine context, PlayerStateFactory factory) : base(context, factory)
    {
        isRootState = true;
    }

    public override void Enter()
    {
        currentTime = 0f;
        hitWall = false;

        var move = _ctx.Movement;
        var cam = _ctx.MainCameraRef.transform;

        var camForward = Vector3.ProjectOnPlane(cam.forward, Vector3.up).normalized;
        var camRight = Vector3.ProjectOnPlane(cam.right, Vector3.up).normalized;
        var moveDir = camRight * move.CurrentMovementInput.x + camForward * move.CurrentMovementInput.y;

        dashDir = moveDir.sqrMagnitude > 0.01f ? moveDir.normalized : move.PlayerTransform.forward;

        if (move.OnSlope(out RaycastHit slopeHit))
            dashDir = Vector3.ProjectOnPlane(dashDir, slopeHit.normal).normalized;
        else
        {
            dashDir.y = 0f;
            dashDir.Normalize();
        }

        move.PlayerTransform.rotation = Quaternion.LookRotation(dashDir);
        move.DashInCooldown = true;

        _ctx.PlayerAnimator.SetBool("IsDashing", true);
        PlayerEvents.OnDashSFX();
        CameraShakeManager.CameraShakeFromProfile(move.DashProfile, _ctx.CameraShakeSource);

        _ctx.Body.useGravity = false;
        _ctx.Body.linearVelocity = Vector3.zero;
    }

    public override void FixedDo()
    {
        var move = _ctx.Movement;
        var rb = _ctx.Body;

        if (hitWall)
        {
            EndDash(move, rb);
            return;
        }

        currentTime += Time.fixedDeltaTime;
        float t = currentTime / move.DashTime;
        float speedFactor = move.DashSpeedCurve.Evaluate(t);

        float dashSpeed = move.DashDistance / move.DashTime;
        Vector3 velocity = dashDir * dashSpeed * speedFactor;
        float moveDist = velocity.magnitude * Time.fixedDeltaTime;

        if (Physics.Raycast(rb.position + Vector3.up * 0.3f, Vector3.down, out RaycastHit groundHit, 1.5f, _ctx.GroundSensor.groundMask))
        {
            if (Vector3.Angle(groundHit.normal, Vector3.up) <= move.MaxSlopeAngle)
                dashDir = Vector3.ProjectOnPlane(dashDir, groundHit.normal).normalized;
        }

        if (Physics.SphereCast(rb.position + Vector3.up * 0.2f, 0.3f, dashDir, out RaycastHit wallHit, moveDist + 0.05f, move.DashCollisionMask))
        {
            rb.MovePosition(wallHit.point - dashDir * 0.05f);
            hitWall = true;
            return;
        }

        Vector3 targetPos = rb.position + dashDir * dashSpeed * speedFactor * Time.fixedDeltaTime;

        if (Physics.Raycast(targetPos + Vector3.up * 0.3f, Vector3.down, out RaycastHit snapHit, 1f, _ctx.GroundSensor.groundMask))
        {
            float slopeAngle = Vector3.Angle(snapHit.normal, Vector3.up);
            if (slopeAngle <= move.MaxSlopeAngle)
                targetPos.y = Mathf.MoveTowards(targetPos.y, snapHit.point.y + 0.05f, 0.3f);
        }

        rb.MovePosition(targetPos);

        if (t >= 1f)
            EndDash(move, rb);
    }

    private void EndDash(PlayerMovement move, Rigidbody rb)
    {
        _ctx.PlayerAnimator.SetBool("IsDashing", false);
        move.ResetDash();
        rb.useGravity = true;
        SwitchStates(_factory.Grounded());
    }

    public override void Exit()
    {
        _ctx.Body.useGravity = true;
    }

    public override void Do() { }
    public override void CheckSwitchState() { }
    public override void InitializeSubState() { }
}
