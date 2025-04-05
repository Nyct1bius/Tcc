using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerInputs input;
    Vector3 direction;

    private void Awake()
    {
        input = new PlayerInputs();
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void OnEnable()
    {
        input.BaseActionMap.Enable();
        input.BaseActionMap.Interact.performed += HandleInteractInput;
        input.BaseActionMap.Jump.performed += HandleJumpInput;
        input.BaseActionMap.Attack.performed += HandleAttackInput;
        input.BaseActionMap.Dash.performed += HandleDashInput;

    }

  

    private void OnDisable()
    {
        input.BaseActionMap.Disable();
        input.BaseActionMap.Interact.performed -= HandleInteractInput;
        input.BaseActionMap.Jump.performed -= HandleJumpInput;
        input.BaseActionMap.Attack.performed -= HandleAttackInput;
        input.BaseActionMap.Dash.performed -= HandleDashInput;
    }
    private void HandleInteractInput(InputAction.CallbackContext context)
    {
        PlayerEvents.OnInteract();
    }
    private void HandleDashInput(InputAction.CallbackContext context)
    {
        PlayerEvents.OnDash();
    }

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        PlayerEvents.OnAttack();
    }

    public void HandleJumpInput(InputAction.CallbackContext context)
    {
        PlayerEvents.OnJump();
    }

    public Vector2 InputDirection()
    {
        direction = input.BaseActionMap.Move.ReadValue<Vector2>();
        direction = direction.normalized;
        return direction;
    }

}
