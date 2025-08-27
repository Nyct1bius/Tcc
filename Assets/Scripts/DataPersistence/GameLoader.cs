using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameLoader : MonoBehaviour
{
    [Tooltip("Offset em relação ao totem salvo (por exemplo, (0, 0, 2) para spawnar 2 unidades à frente).")]
    public Vector3 spawnOffset = new Vector3(0, 0, 2);

    [SerializeField] private GameObject spawner;

    public GameObject maquinaDeVendas1;
    public GameObject maquinaDeVendas2;

    public SaveTotem totem1;
    public SaveTotem totem2;


    private void Start()
    {
        GameData data = DataPersistenceManager.instance.GetGameData();

        if (data != null && !string.IsNullOrEmpty(data.lastTotemId))
        {
            // Confere se o save é da mesma cena
            if (data.lastSceneName != SceneManager.GetActiveScene().name)
            {
                Debug.Log("Save é de outra cena, spawnando no início desta fase.");
                return;
            }

            // Procura o totem salvo na cena
            // var totems = GameObject.FindObjectsByType<SaveTotem>(FindObjectsSortMode.None);
            // var targetTotem = totems.FirstOrDefault(t => t.name == data.lastTotemId);
            var targetTotem = data.lastTotemId;

            if (targetTotem != null)
            {
                // Acha o PlayerSpawner padrão
                // var spawner = FindObjectOfType<PlayerSpawner>();
                //spawner = GameObject.Find("PlayerSpawner");

                if(targetTotem == totem1.totemId)
                {
                    if (spawner != null)
                    {
                        // Move o PlayerSpawner para a frente do totem
                        spawner.transform.position = maquinaDeVendas1.transform.position + spawnOffset;
                        GameManager.instance.SpawnPlayer(spawner.transform);

                        Debug.Log($"PlayerSpawner movido para próximo ao totem {data.lastTotemId} e player spawnado.");
                    }
                    else
                    {
                        Debug.LogWarning("Nenhum PlayerSpawner encontrado na cena!");
                    }
                }
                else if(targetTotem == totem2.totemId)
                {
                    if (spawner != null)
                    {
                        // Move o PlayerSpawner para a frente do totem
                        spawner.transform.position = maquinaDeVendas2.transform.position + spawnOffset;
                        GameManager.instance.SpawnPlayer(spawner.transform);

                        Debug.Log($"PlayerSpawner movido para próximo ao totem {data.lastTotemId} e player spawnado.");
                    }
                    else
                    {
                        Debug.LogWarning("Nenhum PlayerSpawner encontrado na cena!");
                    }
                }

            }
            else
            {
                Debug.LogWarning($"Totem {data.lastTotemId} não encontrado nesta cena! Spawnando no default.");
            }
        }
        else
        {
            Debug.Log("Nenhum totem salvo, spawnando no default.");
        }
    }
}
