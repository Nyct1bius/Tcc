using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> MyEnemies;
    public List<GameObject> enemiesWave1;
    public List<GameObject> enemiesWave2;
    public List<GameObject> enemiesWave3;

    public bool isWavedRoom = false;
    public bool isThereWave3 = false;
    public bool isRoomCleared = false;
    [SerializeField] private Door door;
    [SerializeField] private List<GameObject> closingWalls;


    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.PlayerInstance == other.gameObject)
        {
            if (!isWavedRoom)
            {
                GameManager.instance.SwitchGameState(GameManager.GameStates.EnterCombat);
                Debug.Log("Player found");
                foreach (GameObject enemy in MyEnemies)
                {
                    enemy.GetComponent<EnemyStats>().enabled = true;
                }
            }
            else
            {
                if(!isRoomCleared)
                {
                    GameManager.instance.SwitchGameState(GameManager.GameStates.EnterCombat);
                    Debug.Log("Player found");
                    foreach (GameObject walls in closingWalls)
                    {
                        walls.SetActive(true);
                    }

                    foreach (GameObject enemy in enemiesWave1)
                    {
                        enemy.GetComponent<EnemyStats>().enabled = true;
                    }
                    
                }
                
            }
            Debug.Log("Player found_end");

            
        }
    }

    public void SpawnSecondWave()
    {
        foreach (GameObject enemy in enemiesWave2)
        {
            enemy.SetActive(true);
            enemy.GetComponent<EnemyStats>().enabled = true;
        }
    }
    public void SpawnThirdWave()
    {
        foreach (GameObject enemy in enemiesWave3)
        {
            enemy.SetActive(true);
            enemy.GetComponent<EnemyStats>().enabled = false;
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        if(!isWavedRoom)
        {
            MyEnemies.Remove(enemy);
            if (MyEnemies.Count == 0)
            {
                OpenTheDoor();
            }
        }
        else
        {
            if (enemiesWave1.Contains(enemy))
            {
                print("EnemiesWave 1 Cleared");
                enemiesWave1.Remove(enemy);
                if (enemiesWave1.Count == 0)
                {
                    print("EnemiesWave 2 Called to Spawn");
                    SpawnSecondWave();
                }
            }
            else if (enemiesWave2.Contains(enemy))
            {
                print("EnemiesWave 2 Cleared");
                enemiesWave2.Remove(enemy);
                if (enemiesWave2.Count == 0)
                {
                    if(isThereWave3)
                        SpawnThirdWave();
                    else
                        OpenTheDoor();
                }
            }
            else if (enemiesWave3.Contains(enemy))
            {
                enemiesWave3.Remove(enemy);
                if (enemiesWave3.Count == 0)
                {
                    OpenTheDoor();
                }
            }
        }
    }

    public void OpenTheDoor()
    {
        GameManager.instance.SwitchGameState(GameManager.GameStates.ExitCombat);
        isRoomCleared = true;
        if (door != null)
            door.OpenDoor();

        foreach (GameObject walls in closingWalls)
        {
            walls.SetActive(false);
        }
    }
}
