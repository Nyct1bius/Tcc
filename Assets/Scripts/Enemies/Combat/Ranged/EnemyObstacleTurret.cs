using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyObstacleTurret : MonoBehaviour
{
    Animator animator;
    Transform projectileSpawnPoint;

    [SerializeField] GameObject projectile;

    bool hasAttacked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Attack(float timeBetweenAttacks)
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        AnimatorRanged();

        hasAttacked = false;

        yield return new WaitForSeconds(1.1f);

        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
    }

    private void Shoot()
    {
        if (!hasAttacked)
        {
            StartCoroutine(Attack(1.5f));
            hasAttacked = true;
        }
    }

    private void AnimatorSetIdle()
    {
        animator.SetBool("Idle", true);
    }

    private void AnimatorRanged()
    {
        animator.SetTrigger("Ranged");
    }
}
