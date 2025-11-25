using UnityEngine;

public class LevelEnd : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string levelToLoad;
    [SerializeField] private LoadScene LoadScene;
    [SerializeField] private InputReader _inputReader;
    private bool canInteract = false;
    public bool isLevelOne = false;
    public bool hasCompletedLevel = false;
    public bool isLevelTwo = false;
    PlayerData playerData;


    private void OnEnable()
    {
        _inputReader.InteractEvent += Interact;
    }

    private void OnDisable()
    {
        _inputReader.InteractEvent -= Interact;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            LoadLevel();
            canInteract = true;
            print(canInteract);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            canInteract = false;
            print(canInteract);
        }
    }

    private void Interact()
    {
        //print("HasInteracted");
        //if(canInteract)
        //    LoadLevel();
    }
    public void LoadLevel()
    {            
        if(isLevelTwo)
            GameManager.instance.hasCompletedLevel2 = true;

        hasCompletedLevel = true;

        DataPersistenceManager.Instance.SaveGame();
        LoadScene.StartLoad(levelToLoad);
    }

    public void LoadData(PlayerData data)
    {
        if (isLevelOne)
            hasCompletedLevel = data.hasCompletedLevel1;
        else if (isLevelTwo)
            hasCompletedLevel = data.hasCompletedLevel2;
    }

    public void SaveData(PlayerData data)
    {
        if(isLevelOne)
            data.hasCompletedLevel1 = hasCompletedLevel;
        else if(isLevelTwo)
            data.hasCompletedLevel2 = hasCompletedLevel;
    }
}
