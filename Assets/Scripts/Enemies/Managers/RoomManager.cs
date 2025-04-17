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
    [SerializeField] private Door door;


    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.playerInstance == other.gameObject)
        {
            if(!isWavedRoom)
            {
                foreach (GameObject enemy in MyEnemies)
                {
                    enemy.GetComponent<IdlePathfinding>().enabled = false;
                    enemy.GetComponent<EnemyMeleeCombat>().enabled = true;
                }
            }
            else
            {
                foreach (GameObject enemy in enemiesWave1)
                {
                    enemy.GetComponent<IdlePathfinding>().enabled = false;
                    enemy.GetComponent<EnemyMeleeCombat>().enabled = true;
                }
            }
            

            Debug.Log("Player found");
        }
    }

    public void SpawnSecondWave()
    {
        foreach (GameObject enemy in enemiesWave2)
        {
            enemy.SetActive(true);
            enemy.GetComponent<IdlePathfinding>().enabled = false;
            enemy.GetComponent<EnemyMeleeCombat>().enabled = true;
        }
    }
    public void SpawnThirdWave()
    {
        foreach (GameObject enemy in enemiesWave3)
        {
            enemy.SetActive(true);
            enemy.GetComponent<IdlePathfinding>().enabled = false;
            enemy.GetComponent<EnemyMeleeCombat>().enabled = true;
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        if(!isWavedRoom)
        {
            MyEnemies.Remove(enemy);
            if (MyEnemies.Count == 0)
            {
                door.OpenDoor();
            }
        }
        else
        {
            if (enemiesWave1.Contains(enemy))
            {
                enemiesWave1.Remove(enemy);
                if (enemiesWave1.Count == 0)
                {
                    SpawnSecondWave();
                }
            }
            else if (enemiesWave2.Contains(enemy))
            {
                enemiesWave2.Remove(enemy);
                if (enemiesWave2.Count == 0)
                {
                    if(isThereWave3)
                        SpawnThirdWave();
                    else
                        door.OpenDoor();
                }
            }
            else if (enemiesWave3.Contains(enemy))
            {
                enemiesWave3.Remove(enemy);
                if (enemiesWave3.Count == 0)
                {
                    door.OpenDoor();
                }
            }
        }
    }
}
