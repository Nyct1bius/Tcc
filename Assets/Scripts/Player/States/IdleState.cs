using UnityEngine;

public class IdleState : PlayerStates
{
    [SerializeField] AnimationClip idleAnimation;
    public override void Enter()
    {
        animator.Play(idleAnimation.name);
    }
    public override void Do() { }

    public override void FixedDo() { }

    public override void Exit() { }
}
