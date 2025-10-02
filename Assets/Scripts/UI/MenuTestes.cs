using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuTestes : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        DataPersistenceManager.Instance.SaveGame(); // não pode salvar sem inicializar o game data primeiro
    }

    public void LoadGame()
    {
        DataPersistenceManager.Instance.LoadGame();
    }

    public void NewGame()
    {
        DataPersistenceManager.Instance.NewGame();
    }
}
