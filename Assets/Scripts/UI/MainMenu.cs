using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] TMP_Text mapaAtual;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private PlayerUIManager playerUIManager;
    
    void Update()
    {
        if(Input.GetKeyDown("1"))
        {
            // pega item
            Debug.Log("Pegou roda");
            Roda();
        }

        if (Input.GetKeyDown("2"))
        {
            // pega item
            Debug.Log("Pegou casco");
            Casco();
        }

        if (Input.GetKeyDown("3"))
        {
            // pega item
            Debug.Log("Pegou arma");
            Arma();
        }

        int hours = Mathf.FloorToInt(playerUIManager.tempoDeJogo / 3600);
        int minutes = Mathf.FloorToInt((playerUIManager.tempoDeJogo % 3600) / 60);
        int seconds = Mathf.FloorToInt(playerUIManager.tempoDeJogo % 60);

        timeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

    }

    public void OnEnable()
    {
        mapaAtual.text = SceneManager.GetActiveScene().name;

        healthText.text = playerUIManager._currentHealth.ToString() + "/ 75";
        healthSlider.value = playerUIManager._currentHealth;
    }

    // testes
    public InventoryManager inventoryManager;
    public void Roda()
    {
        inventoryManager.GetIten("Roda");
    }

    public void Casco()
    {
        inventoryManager.GetIten("Casco");
    }

    public void Arma()
    {
        inventoryManager.GetIten("Arma");
    }
}
