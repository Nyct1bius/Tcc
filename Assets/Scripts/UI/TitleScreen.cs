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

    public UnityEngine.UI.Button continueButton;

    private void Awake()
    {
        loadscene = GetComponent<LoadScene>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fase1.SetActive(true);
        _fase2.SetActive(false);

        if(DataPersistenceManager.instance != null)
        {
            bool hasSave = DataPersistenceManager.instance.HasGameData();
            continueButton.interactable = hasSave;
        }
        else
        {
            continueButton.interactable = false;
        }
        
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

    public void Continue()
    {
        // Garante que existe um DataPersistenceManager
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.LoadGame();
        }

        loadscene.StartLoad("Programmers_TestScene");
    }

    public void NewGame()
    {
        // verificar se tem um save e se tiver mostrar uma menssagem de que vai perder o progresso do outro save
        if (DataPersistenceManager.instance != null)
        {
            DataPersistenceManager.instance.NewGame();
        }

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
