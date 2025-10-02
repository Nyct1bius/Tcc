using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameLoader : MonoBehaviour
{
    [Tooltip("Offset em rela��o ao totem salvo (por exemplo, (0, 0, 2) para spawnar 2 unidades � frente).")]
    public Vector3 spawnOffset = new Vector3(0, 0, 2);


    public Vector3 LoadSpawnPos()
    {
        PlayerData data = DataPersistenceManager.Instance.GetGameData();
        var posToSpawn = Vector3.zero;
        if (data != null && !string.IsNullOrEmpty(data.lastTotemId))
        {
            // Confere se o save � da mesma cena
            if (data.lastSceneName != SceneManager.GetActiveScene().name)
            {
                Debug.Log("Save � de outra cena, spawnando no in�cio desta fase.");
                posToSpawn =  Vector3.zero;
            }

            // Procura o totem salvo na cena
            // var totems = GameObject.FindObjectsByType<SaveTotem>(FindObjectsSortMode.None);
            // var targetTotem = totems.FirstOrDefault(t => t.name == data.lastTotemId);
            var targetTotem = data.lastTotemId;

            if (targetTotem != null)
            {
                posToSpawn = data.spawnPos + spawnOffset;

            }
            else
            {
                Debug.LogWarning($"Totem {data.lastTotemId} n�o encontrado nesta cena! Spawnando no default.");
            }
        }
        else
        {
            Debug.Log("Nenhum totem salvo, spawnando no default.");
        }

        return posToSpawn;
    }
}
