using UnityEngine;
using UnityEngine.InputSystem;

public class CutsceneSkipper : MonoBehaviour
{
    private PlayerInputs _inputActions;

    [SerializeField] private GameObject ui;

    [SerializeField] private ManagerOfScenes managerOfScenes;

    private void Awake()
    {
        _inputActions = new PlayerInputs();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.UI.Skip.performed += OnSkipPerformed;
    }

    private void OnDisable()
    {
        _inputActions.UI.Skip.performed -= OnSkipPerformed;
        _inputActions.Disable();
    }

    private void OnSkipPerformed(InputAction.CallbackContext context)
    {
        SkipCutscene();
    }

    private void SkipCutscene()
    {
        Debug.Log("Cutscene pulada!");
        managerOfScenes.SkipCutscene();
        ui.SetActive(false);
    }
}
