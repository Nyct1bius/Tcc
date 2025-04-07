using System;
using UnityEngine;
using UnityEngine.Windows;

public class WalkState : PlayerStates
{
    float xVel;
    [SerializeField] AnimationClip walkAnimation;
    public override void Enter()
    {
        animator.Play(walkAnimation.name);
    }
    public override void Do()
    {
        //xVel = body.linearVelocity.x;
        //if (inputs.Grounded() || MathF.Abs(xVel) < 0.1f)
        //{
        //    isComplete = true;
        //}
    }

    public override void Exit() { }

}
