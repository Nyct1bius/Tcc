using UnityEngine;

public class LevelsManager : MonoBehaviour, IDataPersistence
{

    [SerializeField] IslandManager[] levels;
    bool hasShield;

    public void LoadData(PlayerData data)
    {
        hasShield = data.hasShield;   
    }

    public void SaveData(PlayerData data)
    {
    }

    private void OnEnable()
    {   
        levels[0].EneableIsland();

        if (hasShield)
        {
            levels[1].EneableIsland();  
        }
        
    }
}
