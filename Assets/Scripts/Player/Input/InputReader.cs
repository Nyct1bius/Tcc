using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;



[CreateAssetMenu( menuName = "Inputs/Input reader")]
public class InputReader : ScriptableObject, PlayerInputs.IPlayerControlsActions
{
    public event UnityAction<Vector2> MoveEvent;
    public event UnityAction<bool> JumpEvent;
    public event UnityAction<bool> AttackEvent;
    public event UnityAction <bool>DashEvent;
    public event UnityAction InteractEvent;
    public event UnityAction PauseEvent;


    private PlayerInputs inputs;
    private void OnEnable()
    {
        if(inputs == null)
        {
            inputs = new PlayerInputs();
            inputs.PlayerControls.SetCallbacks(this);
        }

        inputs.Enable();
        GameEvents.ResumeGame += ResumeGameByUIButton;
        GameEvents.QuitGame += ResumeGameByUIButton;
    }

    private void OnDisable()
    {
        inputs.Disable();
        GameEvents.ResumeGame -= ResumeGameByUIButton;
        GameEvents.QuitGame -= ResumeGameByUIButton;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AttackEvent?.Invoke(true);
        }
        if (context.canceled)
        {
            AttackEvent?.Invoke(false);
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DashEvent?.Invoke(true);
        }
        if (context.canceled)
        {
            DashEvent?.Invoke(false);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            InteractEvent?.Invoke();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            JumpEvent?.Invoke(true);
        }
        if (context.canceled)
        {
            JumpEvent?.Invoke(false);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
        Debug.Log(context.ReadValue<Vector2>());
    }

    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseEvent?.Invoke();
            MenuInputs(true);
        }
     
    }

    private void OnCloseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseEvent?.Invoke();
            MenuInputs();
        }
    
    }
    private void ResumeGameByUIButton()
    {
        MenuInputs();
    }

    private void MenuInputs(bool isEneable = false)
    {
        if (isEneable)
        {
            inputs.PlayerControls.Disable();
            inputs.UI.Enable();
            inputs.UI.CloseMenu.performed += OnCloseMenu;
        }
        else
        {
            inputs.UI.CloseMenu.performed -= OnCloseMenu;
            inputs.UI.Disable();
            inputs.PlayerControls.Enable();
        }
    }
}
