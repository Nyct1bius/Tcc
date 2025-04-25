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
        _ctx.Animator.SetBool("IsMoving", false);
    }
    public override void CheckSwitchState()
    {
        if (_ctx.IsAttacking && _ctx.CurrentWeaponData != null)
        {
            SwitchStates(_factory.Attack());
        }


        if (_ctx.IsJumpButtonPressed && !_ctx.RequireNewJumpPress)
        {
            SwitchStates(_factory.Jump());
        }else if (!_ctx.GroundSensor.IsGrounded())
        {
            SwitchStates(_factory.Fall());
        }


        if (!_ctx.DashInCooldown && _ctx.IsDashButtonPressed)
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
        if (_ctx.CurrentMovementInput != Vector2.zero)
        {
            SetSubState(_factory.Walk());
        }
        else
        {
            SetSubState(_factory.Idle());
        }
    }

}
