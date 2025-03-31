using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
    //Player Variables
    [SerializeField] private GameObject playerObj;
    [SerializeField] private CinemachineCamera cnCamera;
    [SerializeField] private Camera minimapCamera;
    public Transform checkPoint;




    public GameObject playerInstance { get; private set; }
    
    



    //Gameplay States
    public enum GameplayStates
    {
        Paused,
        Playing,
        GameIsOver
    }
    public GameplayStates currentState;

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

    private void SelectGameplayState()
    {
        switch(currentState)
        {
            case GameplayStates.Playing:
                break;
            case GameplayStates.Paused:
                break;

            case GameplayStates.GameIsOver:
                break;
        }
    }


    public void SpawnPlayer(Transform spawnPoint)
    {
        playerInstance = Instantiate(playerObj, spawnPoint.position , spawnPoint.rotation);
        CinemachineCamera camera = Instantiate(cnCamera, spawnPoint.position, Quaternion.identity);
        Instantiate(minimapCamera);

        camera.Follow = playerInstance.transform;
        camera.LookAt = playerInstance.transform;

        checkPoint = spawnPoint;

        currentState = GameplayStates.Playing;
    }


    public void RespawnPlayer()
    {
        playerInstance.transform.position = checkPoint.position;
    }

}
