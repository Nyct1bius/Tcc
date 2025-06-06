using UnityEngine;
using PlayerState;
public class IdleState : State
{
    [SerializeField] AnimationClip idleAnimation;
    public IdleState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) { }
    public override void Enter()
    {
    }
    public override void Do() 
    {
        CheckSwitchState();
        if (!_ctx.Movement.IsGroundAtLandingPoint())
        {
            PlayerEvents.OnStopChickenSFX();
        }
    }

    public override void FixedDo() 
    {
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
