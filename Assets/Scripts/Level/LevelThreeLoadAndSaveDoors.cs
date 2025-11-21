using System.Collections;
using UnityEngine;

public class LevelThreeLoadAndSaveDoors : MonoBehaviour, IDataPersistence
{
    PlayerData playerData;
    public Door[] doors32;
    public Door[] doors33;
    public Button[] buttons32;
    public Button[] buttons33;
    public void LoadData(PlayerData data)
    {
        playerData = data;
    }

    public void SaveData(PlayerData data)
    {
        
    }

    public void Start()
    {
        StartCoroutine(DelayedStart());
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(0.25f);
        OpenDoors();
        TriggerLevers();
    }

    void OpenDoors()
    {
        if(playerData.lastTotemId == "Mapa3.2")
        {
            foreach(Door door in doors32)
            {
                door.OpenDoor();
            }
        }
        else if(playerData.lastTotemId == "Mapa3.3")
        {
            foreach (Door door in doors32)
            {
                door.OpenDoor();
            }

            foreach (Door door in doors33)
            {
                door.OpenDoor();
            }
        }
    }

    void TriggerLevers()
    {
        if (playerData.lastTotemId == "Mapa3.2")
        {
            foreach (Button button in buttons32)
            {
                button.CheatPressButton();
            }
        }
        else if (playerData.lastTotemId == "Mapa3.3")
        {
            foreach (Button button in buttons32)
            {
                button.CheatPressButton();
            }

            foreach (Button button in buttons33)
            {
                button.CheatPressButton();
            }
        }
    }
}
