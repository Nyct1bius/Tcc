using NUnit.Framework;
using UnityEngine;

public class Boss : BossStateMachine
{
    public BossStats Stats {  get; private set; }

    public RoomManager RoomManager;
    public Animator Animator;
    public BoxCollider[] AttackHitboxes;
    public Transform[] MovementPoints;
    public Transform NextPoint, CurrentPoint;

    public bool TookDamage = false;

    private void Awake()
    {
        Stats = GetComponent<BossStats>();  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Animator.SetBool("Idle", true);
        Animator.SetBool("Moving", false);

        NextPoint = MovementPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (RoomManager.EnemiesAlerted)
        {
            InitializeStateMachine(new BossIntroState(this, this));
        }
    }

    public void SetCurrentPoint()
    {
        CurrentPoint = NextPoint;
    }
}
