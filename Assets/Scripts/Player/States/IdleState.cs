using UnityEngine;

public class IdleState : PlayerStates
{
    public override void Enter() 
    {
        Debug.Log("Idle");
    }

    public override void Do() { }

    public override void FixedDo() { }

    public override void Exit() { }
}
