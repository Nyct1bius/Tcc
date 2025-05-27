using UnityEngine;
using PlayerState;
public class DeathState : State
{
    public DeathState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
    : base(contex, playerStateFactory)
    {
        isRootState = true;
    }
    bool CalledDeath;
    public override void Enter()
    {
        _ctx.AnimationSystem.Death();
        _ctx.Movement.enabled = false;
        _ctx.Combat.enabled = false;
        _ctx.Health.enabled = false;
        _timeInState = 0;
        CalledDeath = false;
    }

    public override void CheckSwitchState()
    {
       
    }

    public override void Do()
    {
        _timeInState += Time.deltaTime;
        if (_timeInState > 3f && !CalledDeath)
        {
            CalledDeath = true;
            GameManager.instance.SwitchGameState(GameManager.GameStates.GameOver);
        }
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
