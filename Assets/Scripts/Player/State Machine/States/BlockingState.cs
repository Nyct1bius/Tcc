using UnityEngine;
using PlayerState;
public class BlockingState : State
{
    public BlockingState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
    : base(contex, playerStateFactory)
    {
        isRootState = true;
        InitializeSubState();
    }

    public override void Enter()
    {
        _ctx.PlayerAnimator.SetTrigger("ShieldUp");
        _ctx.PlayerAnimator.SetBool("IsBlocking", true);
    }

    public override void CheckSwitchState()
    {
        if (!_ctx.IsBlocking)
        {
            SwitchStates(_factory.Grounded());
        }


        if (_ctx.Combat.IsAttacking && _ctx.Combat.CurrentWeaponData != null && _ctx.Movement.IsGrounded)
        {
            SwitchStates(_factory.Attack());
            return;
        }

        bool canCoyoteJump = _ctx.Movement.TimeSinceUnground < _ctx.Movement.CoyoteTime
                              && !_ctx.Movement.UngroudedDueToJump;

        if (_ctx.Movement.HasBufferedJump)
        {
            if (canCoyoteJump || _ctx.Movement.IsGrounded)
            {
                Debug.Log("Jumping from GroundState");
                SwitchStates(_factory.Jump());
                _ctx.Movement._timeSinceJumpPressed = Mathf.Infinity;
                return;
            }
        }

        if (_ctx.Body.linearVelocity.y < 0 && !canCoyoteJump)
        {
            SwitchStates(_factory.Fall());
        }

        if (!_ctx.Movement.DashInCooldown && _ctx.Movement.IsDashButtonPressed)
        {
            SwitchStates(_factory.Dash());
        }

        if (_ctx.Health.IsDamaged)
        {
            SwitchStates(_factory.Damaged());
        }

    }

    public override void Do()
    {
        CheckSwitchState();
    }
    public override void Exit()
    {
        _ctx.IsBlocking = false;
        _ctx.Shield.ToggleShield(false);
        _ctx.PlayerAnimator.SetBool("IsBlocking", false);


    }

    public override void FixedDo()
    {

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

}
