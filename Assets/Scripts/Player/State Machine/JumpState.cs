using UnityEngine;

public class JumpState : State
{
    [SerializeField] AnimationClip jumpAnimation;
    [SerializeField] PlayerMovement inputs; 
    public override void Enter()
    {
       animator.Play(jumpAnimation.name);
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
        inputs.HandleHorizontalMovement();
        inputs.CheckIfStillJumping();
    }
    public override void Exit()
    {

    }
}
