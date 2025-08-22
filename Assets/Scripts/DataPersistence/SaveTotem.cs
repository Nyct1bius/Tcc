using UnityEngine;

public class SaveTotem : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string totemId;

    private bool isPlayerNearby = false;

    [SerializeField] private GameObject saveText;

    public void SaveData(ref GameData data)
    {
        if (isPlayerNearby) // só salva se o player estiver nesse totem
        {
            data.lastTotemId = totemId;
        }
    }

    public void LoadData(GameData data)
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;

            // dispara o save automaticamente
            DataPersistenceManager.instance.SaveGame();

            saveText.SetActive(true); // trocar para animação depois

            Debug.Log("Salvou no totem: " + totemId);
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

}
