using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour, IDataPersistence
{
    public GameObject _fase1, _fase2;
    public GameObject _camera1, _camera2;
    private LoadScene loadscene;
    private bool fase2 = false;
    [SerializeField]
    private string lastScene;
    [SerializeField] private InputAction anyKeyAction;

    public UnityEngine.UI.Button continueButton;

    private void Awake()
    {
        loadscene = GetComponent<LoadScene>();
        anyKeyAction.Enable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fase1.SetActive(true);
        _fase2.SetActive(false);

        if(DataPersistenceManager.Instance != null)
        {
            bool hasSave = DataPersistenceManager.Instance.HasGameData();
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
            if(anyKeyAction.WasPressedThisFrame())
            {
                _fase1.SetActive(false);
                _fase2.SetActive(true);
                _camera1.SetActive(false);
                _camera2.SetActive(true);
            }
        }
    }

    public void Continue()
    {
        // Garante que existe um DataPersistenceManager
        if (DataPersistenceManager.Instance != null)
        {
            DataPersistenceManager.Instance.LoadGame();
        }
        GameManager.instance.isNewGame = false;
        //loadscene.StartLoad("1 - First Level");
        loadscene.StartLoad(lastScene);
    }

    public void NewGame()
    {
        // verificar se tem um save e se tiver mostrar uma menssagem de que vai perder o progresso do outro save
        if (DataPersistenceManager.Instance != null)
        {
            DataPersistenceManager.Instance.NewGame();
        }
        GameManager.instance.isNewGame = true;
        loadscene.StartLoad("Cutscene Scene");
        //loadscene.StartLoad("1 - First Level");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadData(PlayerData data)
    {
        lastScene = data.lastSceneName;
    }

    public void SaveData(PlayerData data)
    {
        
    }
}
