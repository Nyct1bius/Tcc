using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> MyEnemies;

    private void Update()
    {
        if (MyEnemies.Count == 0)
        {
            Debug.Log("Room cleared!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (GameObject enemy in MyEnemies)
            {
                enemy.GetComponent<IdlePathfinding>().enabled = false;
                enemy.GetComponent<EnemyMeleeCombat>().enabled = true;
            }
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        MyEnemies.Remove(enemy);
    }
}
