using System;
using UnityEngine;
using UnityEngine.Windows;

public class WalkState : PlayerStates
{
    float xVel;
    Animator anim;
    public override void Enter()
    {
        anim = inputs.animator;
        //anim.Play("Run");
    }
    public override void Do()
    {
        xVel = body.linearVelocity.x;
        if (inputs.Grounded() || MathF.Abs(xVel) < 0.1f)
        {
            isComplete = true;
        }
    }

    public override void Exit() { }

}
