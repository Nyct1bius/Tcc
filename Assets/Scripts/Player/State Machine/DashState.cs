using UnityEngine;

public class DashState : State
{
    [SerializeField] AnimationClip dashAnimation;
    public override void Enter()
    {
        animator.Play(dashAnimation.name);
    }

    public override void Do()
    {
        if (core.groundSensor.IsGrounded())
        {
            IsComplete = true;
        }
    }
    public override void FixedDo()
    {
    }
    public override void Exit() { }
}
