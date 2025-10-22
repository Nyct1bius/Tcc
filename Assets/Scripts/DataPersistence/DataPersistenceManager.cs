using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor;

public class DataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]

    [SerializeField] private string fileName;

    [SerializeField] private bool useEncryption;

    private PlayerData gameData;

    private List<IDataPersistence> dataPersistenceObjects;

    private FileDataHandler dataHandler;
    public static DataPersistenceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.dataPath, fileName, useEncryption);
        FindData();
        LoadGame(); // Load game at start 
    }
    void FindData()
    {
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewGameDuringPlayTime()
    {
        this.gameData = new PlayerData();
        SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void NewGame()
    {
        this.gameData = new PlayerData();
        SaveGame();
    }

    public void LoadGame()
    {

        // Load any saved data from a file using the data handler 
        this.gameData = dataHandler.Load();

        // if no data can be loaded, initialize to a new game  
        if(this.gameData == null)
        {
            NewGame();
        }
        FindData();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        FindData();
        foreach (IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        //Debug.Log("Saved money count " + gameData.money);

        //  save that data to a file using the data handler 
        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
       // SaveGame();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects =
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None) // mais rápido
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public bool HasGameData()
    {
        return gameData != null;
    }

    public PlayerData GetGameData()
    {
        return gameData;
    }


}

//[CustomEditor(typeof(DataPersistenceManager))]
//public class DataPersistenceManagerCustomEditor: Editor
//{
//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();
//        DataPersistenceManager playerData = (DataPersistenceManager)target; 

//        if (GUILayout.Button("New game"))
//        {
//            playerData.NewGameDuringPlayTime();
//        }
//    }
    
//}

