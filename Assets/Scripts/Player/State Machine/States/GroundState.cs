using UnityEngine;
using UnityEngine.EventSystems;
public class GroundState : State
{
    public GroundState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
    : base(contex, playerStateFactory) {
        isRootState = true;
        InitializeSubState();
    }
    public override void Enter()
    {
    }
    public override void CheckSwitchState()
    {
        if (_ctx.Combat.IsAttacking && _ctx.Combat.CurrentWeaponData != null)
        {
            SwitchStates(_factory.Attack());
        }


        if (_ctx.Movement.IsJumpButtonPressed && !_ctx.Movement.RequireNewJumpPress)
        {
            SwitchStates(_factory.Jump());
        }else if (!_ctx.GroundSensor.IsGrounded())
        {
            SwitchStates(_factory.Fall());
        }


        if (!_ctx.Movement.DashInCooldown && _ctx.Movement.IsDashButtonPressed)
        {
            SwitchStates(_factory.Dash());
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
