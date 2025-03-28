using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private PlayerStatsSO stats;
    [SerializeField] private Slider healthSlider;


    public void AtualizePlayerHealthUI()
    {
        healthText.text = stats.currentHealth.ToString() + "/" + stats.maxHealth.ToString();

        healthSlider.value = stats.currentHealth;
    }
}
