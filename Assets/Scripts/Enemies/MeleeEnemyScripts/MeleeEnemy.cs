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

    public Vector3 PlayerPosition;

    private void Awake()
    {
        Stats = GetComponent<MeleeEnemyStats>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeStateMachine(new MeleeEnemyIdleState(this));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerPosition = new Vector3(Player.transform.position.x, gameObject.transform.position.y, Player.transform.position.z);
    }
}
