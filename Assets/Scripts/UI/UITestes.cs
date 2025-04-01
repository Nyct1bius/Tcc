using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UITestes : MonoBehaviour, IDataPersistence
{

    public int money; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Aumenta()
    {
        money += 1;
        Debug.Log("money: " + money);
    }

    public void SaveGame()
    {
        
    }

    public void LoadGame()
    {

    }

    public void LoadData(GameData data)
    {
        this.money = data.money;
    }

    public void SaveData(ref GameData data)
    {
        data.money = this.money;
    }
}
