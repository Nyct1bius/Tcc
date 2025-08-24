using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameLoader : MonoBehaviour
{
    private void Start()
    {
        GameData data = DataPersistenceManager.instance.GetGameData();

        if (data != null && !string.IsNullOrEmpty(data.lastTotemId))
        {
            // Se o save n�o � dessa cena, ignora e deixa o spawn padr�o
            if (data.lastSceneName != SceneManager.GetActiveScene().name)
            {
                Debug.Log("Save � de outra cena, spawnando no in�cio desta fase.");
                return;
            }

            // Procura o spawner referente ao �ltimo totem salvo
            var spawners = Object.FindObjectsByType<PlayerSpawner>(FindObjectsSortMode.None);
            var target = spawners.FirstOrDefault(s => s.name == data.lastTotemId);

            if (target != null)
            {
                GameManager.instance.SpawnPlayer(target.transform);
                Debug.Log($"Player spawnado no totem {data.lastTotemId} na cena {SceneManager.GetActiveScene().name}");
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
    }
}
