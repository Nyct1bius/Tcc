using UnityEngine;

public class JumpState : PlayerStates
{
    [SerializeField] private float jumpForce;
    public override void Enter() 
    {
        body.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
    }

    public override void Do() { }

    public override void FixedDo() { }

    public override void Exit() { }
}
