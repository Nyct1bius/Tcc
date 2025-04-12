using System;
using UnityEngine;
using UnityEngine.Windows;

public class WalkState : State
{
    float xVel;
    [SerializeField] AnimationClip walkAnimation;
    [SerializeField] PlayerMovement inputs;
    public override void Enter()
    {
        animator.Play(walkAnimation.name);
    }
    public override void FixedDo()
    {
        inputs.HandleHorizontalMovement();
        inputs.CheckIfStillJumping();
        xVel = body.linearVelocity.x;
        if (core.groundSensor.IsGrounded() || MathF.Abs(xVel) < 0.1f)
        {
            IsComplete = true;
        }
    }

    public override void Exit()
    {
        inputs.HandleHorizontalMovement(); 
    }

}
