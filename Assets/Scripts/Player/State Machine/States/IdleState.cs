using UnityEngine;

public class IdleState : State
{
    [SerializeField] AnimationClip idleAnimation;
    public IdleState(PlayerStateMachine contex, PlayerStateFactory playerStateFactory)
   : base(contex, playerStateFactory) { }
    public override void Enter()
    {
        _ctx.Animator.SetBool("IsMoving", false);
    }
    public override void Do() 
    {
        CheckSwitchState();
    }

    public override void FixedDo() { }

    public override void Exit() { }

    public override void CheckSwitchState()
    {
        if(_ctx.CurrentMovementInput != Vector2.zero)
        {
            SwitchStates(_factory.Walk());
        }
    }

    public override void InitializeSubState()
    {
       
    }
}
