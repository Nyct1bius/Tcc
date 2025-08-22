using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject _fase1, _fase2;
    private LoadScene loadscene;
    private bool fase2 = false;

    public PlayerInputs inputActions;

    public GameObject botaoInicial;

    private void Awake()
    {
        loadscene = GetComponent<LoadScene>();

        inputActions = new PlayerInputs();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fase1.SetActive(true);
        _fase2.SetActive(false);

        
    }

    // Update is called once per frame
    void Update()
    {
        if(!fase2)
        {
            if(Input.anyKeyDown)
            {
                _fase1.SetActive(false);
                _fase2.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        inputActions.Enable();
       // StartCoroutine(SelecionarQuandoAtivar());
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

  /*  private IEnumerator SelecionarQuandoAtivar()
    {
        Debug.Log("Esperando botão ficar ativo...");

        // Espera até o botão estar ativo
        yield return new WaitUntil(() =>
            botaoInicial != null &&
            botaoInicial.activeInHierarchy &&
            botaoInicial.GetComponent<Button>().interactable);

        Debug.Log("Botão está ativo e interagível.");

        yield return null; // espera 1 frame

        EventSystem.current.SetSelectedGameObject(null); // limpa seleção atual
        EventSystem.current.SetSelectedGameObject(botaoInicial);

        Debug.Log("Botão selecionado: " + EventSystem.current.currentSelectedGameObject?.name);
    }*/

    public void Continue()
    {
        loadscene.StartLoad("Programmers_TestScene");
    }

    public void NewGame()
    {
        loadscene.StartLoad("Programmers_TestScene");
    }

    public void LoadGame()
    {
        loadscene.StartLoad("Programmers_TestScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
