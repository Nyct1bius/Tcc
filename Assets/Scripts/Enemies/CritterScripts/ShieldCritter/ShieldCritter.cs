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

    public bool IsAtM1, IsAtM2, IsAtL1, IsAtL2, IsAtR1, IsAtR2, IsCornered, IsWaiting;

    public Transform M1Point, M2Point, L1Point, L2Point, R1Point, R2Point, CurrentPoint, NextPoint;

    private void Awake()
    {
        Stats = GetComponent<ShieldCritterStats>();
    }

    private void Start()
    {
        
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
}
