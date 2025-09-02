using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
using System.Collections;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    public GameObject botaoInicial;

    private PlayerInputs inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputs();
    }

    private void Update()
    {
        // Se não tiver nada selecionado
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            // Detecta navegação por teclado/controle
            if (Keyboard.current != null &&
               (Keyboard.current.upArrowKey.wasPressedThisFrame ||
                Keyboard.current.downArrowKey.wasPressedThisFrame ||
                Keyboard.current.leftArrowKey.wasPressedThisFrame ||
                Keyboard.current.rightArrowKey.wasPressedThisFrame))
            {
                EventSystem.current.SetSelectedGameObject(botaoInicial);
            }
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
        StartCoroutine(SelecionarQuandoAtivar());
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private IEnumerator SelecionarQuandoAtivar()
    {
        yield return new WaitUntil(() =>
            botaoInicial != null &&
            botaoInicial.activeInHierarchy);

        yield return null;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(botaoInicial);
    }
}
