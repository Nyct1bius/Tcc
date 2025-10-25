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
            _ctx.PlayerAnimator.SetFloat("inputX", _ctx.Movement.CurrentMovementInput.x);
            _ctx.PlayerAnimator.SetFloat("inputY", _ctx.Movement.CurrentMovementInput.y);
            return;
        }
        _ctx.PlayerAnimator.SetFloat("Speed", _ctx.Body.linearVelocity.magnitude);
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
