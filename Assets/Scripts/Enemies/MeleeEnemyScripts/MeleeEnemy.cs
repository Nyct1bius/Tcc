using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : MeleeEnemyStateMachine
{
    public MeleeEnemyStats Stats { get; private set; }

    public GameObject Player;
    public NavMeshAgent Agent;
    public RoomManager RoomManager;
    public Animator Animator;
    public BoxCollider AttackHitbox;

    private void Awake()
    {
        Stats = GetComponent<MeleeEnemyStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeStateMachine(new MeleeEnemyIdleState(this, this));

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
        {
            StartCoroutine(WaitToFindPlayer());
        }
        else
        {
            Debug.Log("Player found");
        }

    }

    IEnumerator WaitToFindPlayer()
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

    public void RemoveSelfFromList()
    {
        if (RoomManager != null)
        {
            RoomManager.RemoveEnemyFromList(gameObject);
        }
    }
}
