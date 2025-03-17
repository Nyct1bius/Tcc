using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{

    private PlayerInputs input;
    Vector3 direction;

    public event Action OnJump;

    private void Awake()
    {
        input = new PlayerInputs();
        input.BaseActionMap.Enable();
    }
    private void OnEnable()
    {
        input.BaseActionMap.Jump.performed += JumpInputPerformed;
    }
    private void OnDisable()
    {
        input.BaseActionMap.Jump.performed -= JumpInputPerformed;
    }

    public void JumpInputPerformed(InputAction.CallbackContext context)
    {
        OnJump?.Invoke();
    }

    public Vector3 InputDirection()
    {
        direction = input.BaseActionMap.Move.ReadValue<Vector3>();
        direction = direction.normalized;
        return direction;
    }

}
