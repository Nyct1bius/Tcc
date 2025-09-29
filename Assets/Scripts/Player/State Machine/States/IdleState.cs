using UnityEngine;
using PlayerState;
public class IdleState : State
{
    public IdleState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) { }
    public override void Enter()
    {
    }
    public override void Do() 
    {
        if (_ctx.GameIsPaused)
        {
            PlayerEvents.OnStopChickenSFX();
            return;
        }
        CheckSwitchState();
        if (!_ctx.Movement.IsGroundAtLandingPoint())
        {
            PlayerEvents.OnStopChickenSFX();
        }

    }

    public override void FixedDo() 
    {
        if (_ctx.IsBlocking)
        {
            _ctx.AnimationSystem.UpdateShieldMovement(_ctx.Movement.CurrentMovementInput);
            return;
        }
        _ctx.AnimationSystem.UpdateMovement(_ctx.Body.linearVelocity.magnitude);
    }

    public override void Exit() 
    {
        PlayerEvents.OnStopChickenSFX();
    }

    public override void CheckSwitchState()
    {
        if(_ctx.Movement.CurrentMovementInput != Vector2.zero)
        {
            SwitchStates(_factory.Walk());
        }
    }

    public override void InitializeSubState()
    {
       
    }
}
