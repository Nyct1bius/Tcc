using UnityEngine;

[System.Serializable]

public class GameData
{
    public int money;

    // Último totem usado pelo jogador (por ID ou nome do objeto)
    public string lastTotemId;
    public string lastSceneName;



    // valores padrão quando não há save
    public GameData()
    {
        this.money = 0;
        this.lastTotemId = null; 
        this.lastSceneName = null;
    }
}
