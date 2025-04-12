using UnityEngine;

public class IdleState : State
{
    [SerializeField] AnimationClip idleAnimation;
    public override void Enter()
    {
        animator.Play(idleAnimation.name);
    }
    public override void Do() 
    {
        if(core.groundSensor.IsGrounded())
        {
            IsComplete = true;
        }
    }

    public override void FixedDo() { }

    public override void Exit() { }
}
