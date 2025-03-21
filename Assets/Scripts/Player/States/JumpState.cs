using UnityEngine;

public class JumpState : PlayerStates
{
    public override void Enter() 
    {

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
