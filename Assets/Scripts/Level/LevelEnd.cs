using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    [SerializeField] private LoadScene LoadScene;
    [SerializeField] private InputReader _inputReader;
    private bool canInteract = false;
    public bool isLevelTwo = false;


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
        print("HasLoadedLevel");
        if(isLevelTwo)
            GameManager.instance.hasCompletedLevel2 = true;
        LoadScene.StartLoad(levelToLoad);
    }
}
