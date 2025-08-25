using UnityEngine;
using UnityEngine.EventSystems;
using PlayerState;

public class GroundState : State
{
    public GroundState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
        : base(contex, playerStateFactory)
    {
        isRootState = true;
        InitializeSubState();
    }

    public override void Enter()
    {
    }

    public override void CheckSwitchState()
    {
        // Ataque
        if (_ctx.Combat.IsAttacking && _ctx.Combat.CurrentWeaponData != null)
        {
            SwitchStates(_factory.Attack());
            return;
        }

        // Pulo com input buffer
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
        // Fall
        if (_ctx.Body.linearVelocity.y < 0 && !canCoyoteJump)
        {
            SwitchStates(_factory.Fall());
        }

        // Dash
        if (!_ctx.Movement.DashInCooldown && _ctx.Movement.IsDashButtonPressed)
        {
            SwitchStates(_factory.Dash());
        }

        // Recebeu dano
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
