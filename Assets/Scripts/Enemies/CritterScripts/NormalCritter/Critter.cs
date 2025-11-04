using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Critter : CritterStateMachine
{
    public CritterStats Stats { get; private set; }
 
    public GameObject Player;
    public NavMeshAgent Agent;
    public Animator Animator;
    public Transform[] PatrolPoints;

    private void Awake()
    {
        Stats = GetComponent<CritterStats>();
    }

    void Start()
    {
        InitializeStateMachine(new CritterPatrolState(this, this));

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());
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
}
