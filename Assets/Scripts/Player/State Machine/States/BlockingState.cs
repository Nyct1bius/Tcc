using UnityEngine;
using PlayerState;
public class BlockingState : State
{
    public BlockingState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
    : base(contex, playerStateFactory)
    {
        isRootState = true;
    }

    public override void Enter()
    {

    }

    public override void CheckSwitchState()
    {

    }

    public override void Do()
    {
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
