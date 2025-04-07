using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public static GameManager instance;
    //Player Variables
    [SerializeField] private GameObject playerObj;
    [SerializeField] private CinemachineCamera cnCamera;
    [SerializeField] private Camera minimapCamera;
    private CinemachineCamera cnCameraRef;
    private CinemachineCamera oldCnCameraRef;
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
        cnCameraRef = Instantiate(cnCamera, spawnPoint.position, Quaternion.identity);
        Instantiate(minimapCamera);

        cnCameraRef.Follow = playerInstance.transform;
        cnCameraRef.LookAt = playerInstance.transform;

        checkPoint = spawnPoint;

        currentState = GameplayStates.Playing;
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
        playerInstance.transform.position = checkPoint.position;
        oldCnCameraRef = cnCameraRef;
        oldCnCameraRef.Priority = 0;
        yield return new WaitForSeconds(1f);
        cnCameraRef = Instantiate(cnCamera, playerInstance.transform.position, Quaternion.identity);
        cnCamera.Priority = 1;
        cnCameraRef.Follow = playerInstance.transform;
        cnCameraRef.LookAt = playerInstance.transform;
        yield return new WaitForSeconds(2f);
        Destroy(oldCnCameraRef);
    }

}
