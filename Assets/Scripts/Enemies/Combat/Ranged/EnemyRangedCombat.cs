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

    private void Start()
    {
        player = Stats.Player;
        agent = Stats.Agent;
        animator = Stats.Animator;
    }

    private void Update()
    {
        if (Stats.IsAlive)
            transform.LookAt(Stats.PlayerPosition);

        animator.SetBool("Walk", false);
        animator.SetBool("Idle", true);

        if (!hasAttacked)       
            StartCoroutine(RangedAttack());

        if (agent.enabled)
            agent.ResetPath();
    }

    private IEnumerator RangedAttack()
    {   
        hasAttacked = true;
        
        yield return new WaitForSeconds(Stats.TimeBetweenAttacks);

        animator.SetTrigger("Ranged");

        yield return new WaitForSeconds(0.6f);

        hasAttacked = false;

        EnemyProjectileGenerator.Instance.SpawnProjectile(projectileSpawnPoint);  
    }

    public void StopRangedCoroutines()
    {
        StopAllCoroutines();

        if (hasAttacked)
            hasAttacked = false;
    }
}
