using UnityEngine;
using PlayerState;
public class PausedState : State
{
    public PausedState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
     : base(contex, playerStateFactory)
    {
        isRootState = true;
    }
    public override void CheckSwitchState()
    {
        if (!_ctx.GameIsPaused)
        {
            SwitchStates(_factory.Grounded());
        }
    }

    public override void Do()
    {
     
    }

    public override void Enter()
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
       
    }

}
