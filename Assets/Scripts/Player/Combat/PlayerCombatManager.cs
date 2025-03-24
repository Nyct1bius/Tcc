using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    //wepon variables
    [SerializeField] private bool hasSword;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private Transform attackCollisionCheck;
    [SerializeField] private float attackRange;
    [SerializeField] private float weaponDamage;


    //Combat variables
    private int attackCount;
    private bool attackIncooldown;
    private float timeBetweenAttacks = 0.2f;
    [SerializeField] private float timeToResetAttack = 0.5f;
    private void OnEnable()
    {
        PlayerEvents.Attack += PerformAttack;
        PlayerEvents.SwordPickUp += CheckIfHasSword;
    }
    private void OnDisable()
    {
        PlayerEvents.Attack -= PerformAttack;
        PlayerEvents.SwordPickUp -= CheckIfHasSword;
    }

    private void CheckIfHasSword()
    {
        hasSword = true;
    }

    private void PerformAttack()
    {
        if(hasSword && !attackIncooldown)
        {
           StartCoroutine(SelectAttack());
           attackCount++;
        }
    }
    private IEnumerator SelectAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        Debug.Log("Attack" + attackCount);
        switch (attackCount)
        {
            case 0:
                CheckAllNearbyColliders();
                break;
            case 1:
                CheckAllNearbyColliders();
                break;
            case 2:
                CheckAllNearbyColliders();
                attackIncooldown = true;
                StartCoroutine(ResetAttack());
                break;
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeToResetAttack);
        attackIncooldown = false;
        attackCount = 0;
    }
    private void CheckAllNearbyColliders()
    {
        foreach(Collider enemy in GetAllNearbyColliders())
        {
            Vector3 vectorToCollider = enemy.transform.position - gameObject.transform.position;
            vectorToCollider = vectorToCollider.normalized;

            if (Vector3.Dot(vectorToCollider, gameObject.transform.forward) > 0.5)
            {
                DamageEnemy(enemy);
            }
        }
    }

    private Collider[] GetAllNearbyColliders()
    {
        return Physics.OverlapSphere(attackCollisionCheck.position, attackRange, damageableLayer);
    }



    private void DamageEnemy(Collider enemy)
    {
        IHealth enemyHealth = enemy.GetComponent<IHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(weaponDamage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(attackCollisionCheck.position, attackRange);
    }
}
