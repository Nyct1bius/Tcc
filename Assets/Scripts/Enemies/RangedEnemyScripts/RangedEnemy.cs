using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : RangedEnemyStateMachine
{
    public RangedEnemyStats Stats { get; private set; }

    public GameObject Player;
    public NavMeshAgent Agent;
    public RoomManager RoomManager;
    public Animator Animator;
    public Transform ProjectileSpawnPoint;

    public bool TookDamage = false;

    private void Awake()
    {
        Stats = GetComponent<RangedEnemyStats>();
    }

    void Start()
    {
        if (!RoomManager.EnemiesAlerted)
            InitializeStateMachine(new RangedEnemyIdleState(this, this));
        else
            InitializeStateMachine(new RangedEnemyCombatState(this, this));

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
        print(chance);

        // 20% probability
        if (chance <= 0.2f)
        {
            CombatHealGenerator.Instance.SpawnHeal(transform);
        }
    }

    public void InstantiateProjectile()
    {
        EnemyProjectileGenerator.Instance.SpawnProjectile(ProjectileSpawnPoint);
    }
}
