using UnityEngine;

public class CombatPathfinding : MonoBehaviour
{
    public GameObject Player;

    [SerializeField] float targetDistanceToPlayer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) > targetDistanceToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, 1.8f * Time.deltaTime);

            gameObject.GetComponent<EnemyMeleeCombat>().StartCoroutine("Attack");
        }
        else
        {
            gameObject.GetComponent<EnemyMeleeCombat>().StopCoroutine("Attack");
        }
    }
}
