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

    private PlayerInputs inputs;
    private void OnEnable()
    {
        if(inputs == null)
        {
            inputs = new PlayerInputs();
            inputs.PlayerControls.SetCallbacks(this);
        }

        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        AttackEvent?.Invoke(context.performed);
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        DashEvent?.Invoke(context.performed);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke(context.performed);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
