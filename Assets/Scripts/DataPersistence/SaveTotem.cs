using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTotem : MonoBehaviour, IDataPersistence
{
    public string totemId;

    private bool isPlayerNearby = false;

    [SerializeField] private GameObject saveText;
    public void SaveData(PlayerData data)
    {
        if (isPlayerNearby) // s� salva se o player estiver nesse totem
        {
            data.lastTotemId = totemId;
            data.lastSceneName = SceneManager.GetActiveScene().name;
            data.spawnPos = transform.position;
        }
    }

    public void LoadData(PlayerData data)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // dispara o save automaticamente
            DataPersistenceManager.instance.SaveGame();

            saveText.SetActive(true); // trocar para anima��o depois
            StartCoroutine(HideSaveTextAfterDelay(3f));

            Debug.Log($"Salvou no totem {totemId} na cena {SceneManager.GetActiveScene().name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            saveText.SetActive(false);
        }
    }

    private IEnumerator HideSaveTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        saveText.SetActive(false);
    }

}
