using UnityEngine;

public class LevelsManager : MonoBehaviour, IDataPersistence
{

    [SerializeField] IslandManager[] levels;
    bool hasShield;
    [SerializeField] Transform ship;
    [SerializeField] Transform secondSpawn;

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
            ship.position = secondSpawn.position;
            ship.rotation = secondSpawn.rotation;
            levels[0].InstaEneableIsland();
            levels[1].EneableIsland();  
        }
        DataPersistenceManager.Instance.LoadGame();
    }
}
