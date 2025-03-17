using UnityEngine;
using UnityEngine.Windows;

public class WalkState : PlayerStates
{ 
    //Inputs
    float xInput;
    float yInput;

    [Header("Componets")]
    [SerializeField] private Transform mainCameraRef;

    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float maxSpeed;
    Vector3 moveDirection;
    public override void Enter() 
    {
        Debug.Log("Walk");
    }

    public override void Do() 
    {
        CheckInputs();
    }

    public override void FixedDo() 
    {
        FaceInput();
        if (inputManager.InputDirection() != Vector3.zero)
        {
            body.AddForce(moveDirection * acceleration, ForceMode.VelocityChange);
        }
    }

    public override void Exit() { }


    private void FaceInput()
    {
        moveDirection = mainCameraRef.transform.forward * yInput + mainCameraRef.transform.right * xInput;
        moveDirection.y = 0;
    }

    private void CheckInputs()
    {
        xInput = inputManager.InputDirection().x;
        yInput = inputManager.InputDirection().z;
    }

}
