using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
   
    [SerializeField] private Slider healthSlider;


    public void AtualizePlayerHealthUI(float maxHealth, float currentHealth)
    {
        healthText.text = currentHealth.ToString() + "/" + maxHealth.ToString();

        healthSlider.value = currentHealth;
    }
}
