using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager Instance;

    [Header("Player Hud")]
    [SerializeField] private GameObject _playerHud;


    [Header("Health")]
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private PlayerStatsSO _stats;
    [SerializeField] private Slider _healthSlider;

    [Header("Timers")]
    [SerializeField] public GameObject _uiHolder;
    [SerializeField] public TextMeshProUGUI _timeText;
    [SerializeField] public Slider _timeSlider;


    [Header("Pause")]
    private bool paused = false;
    [SerializeField] InputReader _inputs;
    [SerializeField] private GameObject _mainMenuScreen;

    [Header("Game Over")]
    [SerializeField] private GameObject _gameOverMenu;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void OnEnable()
    {
        _inputs.PauseEvent += OpenPauseMenu;
        GameEvents.GameOver += OnGameOver;
        Time.timeScale = 1.4f;
    }

    private void OnDisable()
    {
        _inputs.PauseEvent -= OpenPauseMenu;
        GameEvents.GameOver -= OnGameOver;
        Time.timeScale = 1.4f;
    }

    public void AtualizePlayerHealthUI(float currentHealth)
    {
        _healthText.text = currentHealth.ToString() + "/" + _stats.maxHealth.ToString();

        _healthSlider.value = currentHealth;
    }


    public void OpenPauseMenu()
    {
        if (!paused)
        {
            _playerHud.SetActive(false);
            _mainMenuScreen.SetActive(true);
            paused = true;
            PauseGameManager.PauseGame();
        }
        else
        {
            _playerHud.SetActive(true);
            _mainMenuScreen.SetActive(false);
            paused = false;
            PauseGameManager.ResumeGame();
        }
    }

    private void OnGameOver()
    {
        _playerHud.SetActive(false);
        _mainMenuScreen.SetActive(false);
        _gameOverMenu.SetActive(true);
    }

    public void OnRestartGame()
    {
        _playerHud.SetActive(true);
        _gameOverMenu.SetActive(false);
        GameManager.instance.SwitchGameState(GameManager.GameStates.Restart);
    }
    public void Title()
    {
        GameEvents.OnQuitGame();
        SceneManager.LoadSceneAsync("Title Screen");
    }
}
