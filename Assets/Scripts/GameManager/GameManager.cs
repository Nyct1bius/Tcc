using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
    //Player Variables
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera cnCameraPrefab;
    [SerializeField] private Camera minimapCameraPrefab;
    private Camera _minimapCamRef;
    private CinemachineCamera _cnCameraRef;
    private CinemachineCamera oldCnCameraRef;
    private Transform _checkpoint;
    private Transform _spawnPos;
    public bool IsRestartingGame;
    public GameObject PlayerInstance { get; private set; }
    [SerializeField] private PlayerStatsSO _playerStats;

    [SerializeField] private int lastCheckPointIndex;
    
    



    //Gameplay States
    public enum GameStates
    {
        Started,
        RunningGame,
        EnterCombat,
        ExitCombat,
        Paused,
        Resume,
        Respawn,
        GameOver,
        Restart,
        Quit
    }
    public GameStates State;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        GameEvents.PauseGame += OnGamePaused;
        GameEvents.ResumeGame += OnGameResume;
        GameEvents.QuitGame += OnQuitGame;
    }

    private void OnDisable()
    {
        GameEvents.PauseGame -= OnGamePaused;
        GameEvents.ResumeGame -= OnGameResume;
        GameEvents.QuitGame -= OnQuitGame;
    }
    private void OnApplicationQuit()
    {
        _playerStats.hasSword = false;
    }
    public void SwitchGameState(GameStates newState)
    {
        State = newState;
        switch(State)
        {
            case GameStates.Started:
                OnEnterGame();
                break;
            case GameStates.RunningGame:
                OnGame();
                break;
            case GameStates.EnterCombat:
                OnEnterCombat();
                break;
            case GameStates.ExitCombat:
                OnExitCombat();
                break;
            case GameStates.Paused:
                break;
            case GameStates.Resume:
                break;
            case GameStates.GameOver:
                OnGameOver();
                break;
            case GameStates.Restart:
                RestarGame();
                break;
            case GameStates.Quit:
                Quit();
                break;
        }
    }

    private void OnEnterGame()
    {
       
        //do something more when the game is initializing


        SwitchGameState(GameStates.RunningGame);
    }

    private void OnGame()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnEnterCombat()
    {
        GameEvents.OnEnterCombat();
    }

    private void OnExitCombat()
    {
        GameEvents.OnExitCombat();
    }

    private void RestarGame()
    {
        DestroyCurrentPlayer();
        PauseGameManager.ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void OnGameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        GameEvents.OnGameOver();
        PauseGameManager.GameOver();
    }
    private void OnGamePaused()
    {
        SwitchGameState(GameStates.Paused);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnGameResume()
    {
        State = GameStates.Resume;
        SwitchGameState(GameStates.RunningGame);
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        SwitchGameState(GameStates.Started);
        PlayerInstance = Instantiate(playerPrefab, spawnPoint.position , spawnPoint.rotation);
        _cnCameraRef = Instantiate(cnCameraPrefab, spawnPoint.position, Quaternion.identity);
        _minimapCamRef = Instantiate(minimapCameraPrefab);

        _cnCameraRef.Follow = PlayerInstance.transform;
        _cnCameraRef.LookAt = PlayerInstance.transform;

        _spawnPos = spawnPoint;
    }
    private void DestroyCurrentPlayer()
    {
        Destroy(PlayerInstance);
        Destroy(_cnCameraRef);
    }
    public void SetCheckpoint(Transform newCheckpoint, int cpIndex)
    {
        _checkpoint = newCheckpoint;
        if(cpIndex != lastCheckPointIndex)
        {
            lastCheckPointIndex = cpIndex;
            PlayerInstance.GetComponent<PlayerHealthManager>().HealHealth(_playerStats.maxHealth);
        }
    }
    private void OnQuitGame()
    {
        SwitchGameState(GameStates.Quit);
    }

    private void Quit()
    {
        PauseGameManager.QuitGame();
        _checkpoint = null;
        _cnCameraRef = null;
        oldCnCameraRef = null;
        PlayerInstance = null;
    }
    public void RespawnPlayer()
    {
       StartCoroutine(RespawningPlayer());
    }


    IEnumerator RespawningPlayer()
    {
        _cnCameraRef.Follow = null;
        _cnCameraRef.LookAt = null;
        yield return new WaitForSeconds(3f);
        if (_checkpoint == null)
        {
            PlayerInstance.transform.position = _spawnPos.position;

        }
        else
        {
            PlayerInstance.transform.position = _checkpoint.position;
        }
          
        oldCnCameraRef = _cnCameraRef;
        oldCnCameraRef.Priority = 0;
        yield return new WaitForSeconds(1f);
        _cnCameraRef = Instantiate(cnCameraPrefab, PlayerInstance.transform.position, Quaternion.identity);
        cnCameraPrefab.Priority = 1;
        _cnCameraRef.Follow = PlayerInstance.transform;
        _cnCameraRef.LookAt = PlayerInstance.transform;
        yield return new WaitForSeconds(2f);
        Destroy(oldCnCameraRef);
    }

}
