using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCritterHealth : MonoBehaviour
{
    //Stat variables
    public int CritterHealth;
    private int critterCurrentHealth;
    public float CritterMovementSpeed;

    //References
    public RoomManager RoomManager;
    public NavMeshAgent Agent;
    public GameObject Player;
    public Animator Animator;

    //Script references
    [SerializeField] private IdlePathfinding pathfindingScript;

    void Start()
    {
        critterCurrentHealth = CritterHealth;

        Animator.SetBool("Idle", true);

        Agent.speed = CritterMovementSpeed;

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());
    }

    void Update()
    {
        
    }

    //Wait for player on scene start
    private IEnumerator WaitToFindPlayer()
    {
        yield return new WaitForSeconds(0.25f);
        if (GameManager.instance.PlayerInstance != null)
        {
            while (Player == null)
            {
                Player = GameManager.instance.PlayerInstance;
                yield return null;
            }
        }
        else
        {
            Debug.LogWarning("GameManager Instance not found");
        }
    }

    private void Death()
    {
        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }

        GetComponent<CapsuleCollider>().enabled = false;

        pathfindingScript.enabled = false;
        Agent.isStopped = true;

        Animator.SetBool("Idle", false);
        Animator.SetBool("Walk", false);
        Animator.SetTrigger("Die");
    }
}
