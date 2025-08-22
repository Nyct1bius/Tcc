using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
using System.Collections;

public class UIInputManager : MonoBehaviour
{
    public GameObject botaoInicial;

    private PlayerInputs inputActions;

    private void Awake()
    {
        inputActions = new PlayerInputs();
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
