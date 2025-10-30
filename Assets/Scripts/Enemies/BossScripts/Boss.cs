using UnityEngine;

public class Boss : BossStateMachine
{
    public BossStats Stats {  get; private set; }

    public RoomManager RoomManager;
    public Animator Animator;
    public GameObject[] AttackHitboxes;
    public Transform[] MovementPoints;

    private void Awake()
    {
        Stats = GetComponent<BossStats>();  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator.SetBool("Idle", true);
        Animator.SetBool("Moving", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (RoomManager.EnemiesAlerted)
        {
            InitializeStateMachine(new BossIntroState(this, this));
        }
    }
}
