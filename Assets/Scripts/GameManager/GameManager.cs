using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
    //Player Variables
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private CinemachineCamera cnCameraPrefab;
    [SerializeField] private Camera minimapCameraPrefab;
    private CinemachineCamera cnCameraRef;
    private CinemachineCamera oldCnCameraRef;
    private Vector3 _checkpoint;
    public GameObject playerInstance { get; private set; }
    
    



    //Gameplay States
    public enum GameStates
    {
        Started,
        RunningGame,
        Paused,
        Resume,
        Respawn,
        GameOver
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
    }
    private void OnDisable()
    {
        GameEvents.PauseGame -= OnGamePaused;
        GameEvents.ResumeGame -= OnGameResume;
    }

    public void ChangeGameState(GameStates newState)
    {
        State = newState;
        Debug.Log("Game State = " + State);
        switch(State)
        {
            case GameStates.Started:
                OnEnterGame();
                break;
            case GameStates.RunningGame:
                OnGame();
                break;
            case GameStates.Paused:
                break;
            case GameStates.Resume:
                break;
            case GameStates.GameOver:
                break;
        }
    }

    private void OnEnterGame()
    {
       
        //do something more when the game is initializing


        ChangeGameState(GameStates.RunningGame);
    }

    private void OnGame()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void OnGamePaused()
    {
        ChangeGameState(GameStates.Paused);
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnGameResume()
    {
        State = GameStates.Resume;
        ChangeGameState(GameStates.RunningGame);
    }

    public void SpawnPlayer(Transform spawnPoint)
    {
        ChangeGameState(GameStates.Started);
        playerInstance = Instantiate(playerPrefab, spawnPoint.position , spawnPoint.rotation);
        cnCameraRef = Instantiate(cnCameraPrefab, spawnPoint.position, Quaternion.identity);
        Instantiate(minimapCameraPrefab);

        cnCameraRef.Follow = playerInstance.transform;
        cnCameraRef.LookAt = playerInstance.transform;

        _checkpoint = spawnPoint.position;
    }
    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        _checkpoint = newCheckpoint;
    }

    public void RespawnPlayer()
    {
       StartCoroutine(RespawningPlayer());
    }


    IEnumerator RespawningPlayer()
    {
        cnCameraRef.Follow = null;
        cnCameraRef.LookAt = null;
        yield return new WaitForSeconds(3f);
        playerInstance.transform.position = _checkpoint;
        oldCnCameraRef = cnCameraRef;
        oldCnCameraRef.Priority = 0;
        yield return new WaitForSeconds(1f);
        cnCameraRef = Instantiate(cnCameraPrefab, playerInstance.transform.position, Quaternion.identity);
        cnCameraPrefab.Priority = 1;
        cnCameraRef.Follow = playerInstance.transform;
        cnCameraRef.LookAt = playerInstance.transform;
        yield return new WaitForSeconds(2f);
        Destroy(oldCnCameraRef);
    }

}
