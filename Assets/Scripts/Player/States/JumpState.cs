using UnityEngine;

public class JumpState : PlayerStates
{
    [SerializeField] AnimationClip jumpAnimation;
    public override void Enter()
    {
        animator.Play(jumpAnimation.name);
    }

    public override void Do() 
    {
        if (inputs.Grounded())
        {
            isComplete = true;
        }
    }

    public override void FixedDo() { }

    public override void Exit() { }
}
