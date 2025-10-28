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
    public Transform[] PatrolPoints;

    public bool TookDamage = false;
    public bool HasHyperarmor;
    public bool IsPatroller;
    public bool CheckPlayerPosition = true;

    private void Awake()
    {
        Stats = GetComponent<MeleeEnemyStats>();
    }

    void Start()
    {
        if (!RoomManager.EnemiesAlerted)
        {
            if (IsPatroller)
                InitializeStateMachine(new MeleeEnemyPatrolState(this, this));
            else
                InitializeStateMachine(new MeleeEnemyIdleState(this, this));
        }
        else
            InitializeStateMachine(new MeleeEnemyCombatState(this, this));

        Player = GameManager.instance.PlayerInstance;
        if (Player == null)
            StartCoroutine(WaitToFindPlayer());
    }

    IEnumerator WaitToFindPlayer()
    {
        yield return new WaitForSeconds(0.25f);
        if (GameManager.instance.PlayerInstance != null)
            while (Player == null)
            {
                Player = GameManager.instance.PlayerInstance;
                yield return null;
            }
    }

    public void RemoveSelfFromList()
    {
        if (RoomManager != null)
            RoomManager.RemoveEnemyFromList(gameObject);

        float chance = Random.value; // same as Random.Range(0f, 1f)

        // 20% probability
        if (chance <= 0.2f)
        {
            CombatHealGenerator.Instance.SpawnHeal(transform);
        }
    }

    public void EnableHitbox()
    {
        AttackHitbox.enabled = true;
    }
    public void DisableHitbox()
    {
        AttackHitbox.enabled = false;
    }
}
