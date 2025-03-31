using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public List<GameObject> MyEnemies;
    [SerializeField] private Door door;

    private void Update()
    {
        if (MyEnemies.Count == 0)
        {
            Debug.Log("Room cleared!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.instance.playerInstance == other.gameObject)
        {
            foreach (GameObject enemy in MyEnemies)
            {
                enemy.GetComponent<IdlePathfinding>().enabled = false;
                enemy.GetComponent<EnemyMeleeCombat>().enabled = true;
            }

            Debug.Log("Player found");
        }
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        MyEnemies.Remove(enemy);
        if (MyEnemies.Count == 0)
        {
            door.OpenDoor();
        }
    }
}
