using System.Collections;
using UnityEngine;

public class Boss : BossStateMachine
{
    public BossStats Stats {  get; private set; }

    public GameObject Player, Laser;
    public RoomManager RoomManager;
    public Animator Animator;
    public BoxCollider BounceHitbox, bossCollider;
    public Transform[] MovementPoints;
    public Transform NextPoint, CurrentPoint;

    public bool TookDamage = false;

    private bool roomAlerted = false;

    private void Awake()
    {
        Stats = GetComponent<BossStats>();  
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NextPoint = MovementPoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (RoomManager.EnemiesAlerted && !roomAlerted)
        {
            Player = GameManager.instance.PlayerInstance;
            InitializeStateMachine(new BossIntroState(this, this));
            roomAlerted = true;
        }
    }

    public void SetCurrentPoint()
    {
        CurrentPoint = NextPoint;
    }

    public void EnableDiveHitbox()
    {
        BounceHitbox.enabled = true;
    }
    public void DisableDiveHitbox()
    {
        BounceHitbox.enabled = false;
    }

    public void EnableCollider()
    {
        bossCollider.enabled = true;    
    }
    public void DisableCollider()
    {
        bossCollider.enabled = false;   
    }

    public void SetLaserOn()
    {
        Laser.SetActive(true);
    }
    public void SetLaserOff()
    {
        Laser.SetActive(false);
    }

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Stats.RotationSpeed * 100f * Time.deltaTime);
        }
    }

    public bool IsCloseToTarget(Vector3 targetPosition)
    {
        if (Vector3.Distance(transform.position, targetPosition) > 3)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
