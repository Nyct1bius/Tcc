using UnityEngine;

public class DashState : State
{
    //[SerializeField] AnimationClip dashAnimation;
    public DashState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) { }
    public override void Enter()
    {
        //animator.Play(dashAnimation.name);
    }

    public override void Do()
    {
        CheckSwitchState();
    }
    public override void FixedDo()
    {
    }
    public override void Exit() { }

    public override void CheckSwitchState()
    {
       
    }

    public override void InitializeSubState()
    {
        
    }
}
