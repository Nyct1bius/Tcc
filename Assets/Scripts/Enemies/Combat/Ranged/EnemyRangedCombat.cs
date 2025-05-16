using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangedCombat : MonoBehaviour
{
    public EnemyStats Stats;

    GameObject player;

    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPoint;

    NavMeshAgent agent;
    Animator animator;

    bool hasAttacked = false;
    public bool IsTurret;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        player = Stats.Player;
        agent = Stats.Agent;
        animator = Stats.Animator;
    }

    private void Update()
    {
        agent.ResetPath();

        transform.LookAt(Stats.PlayerPosition);

        if (!hasAttacked)
        {
            StartCoroutine(RangedAttack());
        }
    }

    private IEnumerator RangedAttack()
    {   
        hasAttacked = true;
        
        yield return new WaitForSeconds(Stats.TimeBetweenAttacks);

        animator.SetTrigger("Ranged");

        yield return new WaitForSeconds(1.1f);

        hasAttacked = false;

        projectile.transform.position = projectileSpawnPoint.position;

        projectile.GetComponent<EnemyProjectile>().DespawnTimer = projectile.GetComponent<EnemyProjectile>().StartDespawnTimer;

        projectile.SetActive(true);    
    }
}
