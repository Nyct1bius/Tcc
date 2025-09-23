using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ShieldCritter : ShieldCritterStateMachine
{
    public ShieldCritterStats Stats { get; private set; }

    public GameObject Player;
    public NavMeshAgent Agent;
    public RoomManager RoomManager;
    public Animator Animator;

    public bool IsCornered, IsWaiting;

    public Transform M1Point, M2Point, M3Point, L1Point, L2Point, R1Point, R2Point;
    public Transform CurrentPoint, NextPoint;

    private void Awake()
    {
        Stats = GetComponent<ShieldCritterStats>();
    }

    private void Start()
    {
        InitializeStateMachine(new ShieldCritterIdleState(this, this));

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());
    }

    IEnumerator WaitToFindPlayer()
    {
        yield return new WaitForSeconds(1);
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

    public void SetL1OrL2()
    {
        if (Random.value < 0.5f)
            NextPoint = L1Point;
        else
            NextPoint = R1Point;
    }
    public void SetL1()
    {
        NextPoint = L1Point;
    }
    public void SetL2()
    {
        NextPoint = L2Point;
    }
    public void SetR1()
    {
        NextPoint = R1Point;
    }
    public void SetR2()
    {
        NextPoint = R2Point;
    }
    public void SetM1()
    {
        NextPoint = M1Point;
    }
    public void SetM2()
    {
        NextPoint = M2Point;
    }
    public void SetM3()
    {
        NextPoint = M3Point;
    }
}
