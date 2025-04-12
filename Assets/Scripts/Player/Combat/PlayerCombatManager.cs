using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatManager : MonoBehaviour
{
    //wepon variables
    [SerializeField] private bool hasSword;
    private InputReader inputReader;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private Transform attackCollisionCheck;
    [SerializeField] private float attackRange;
    [SerializeField] private float weaponDamage;
    [SerializeField] private GameObject swordVisual;


    //Combat variables
    private int attackCount;
    [SerializeField] private bool attackIncooldown;
    private float timeBetweenAttacks = 0.2f;
    [SerializeField] private float timeToResetAttack = 0.5f;

    private void Awake()
    {
        inputReader = GetComponentInParent<PlayerMovement>().inputReader;
    }
    private void OnEnable()
    {
        inputReader.AttackEvent += PerformAttack;
        PlayerEvents.SwordPickUp += AddSword;
    }
    private void OnDisable()
    {
        inputReader.AttackEvent -= PerformAttack;
        PlayerEvents.SwordPickUp -= AddSword;
    }

    private void AddSword()
    {
        hasSword = true;
        swordVisual.SetActive(true);
    }

    private void PerformAttack()
    {
        if(hasSword && !attackIncooldown)
        {
            SelectAttack();
           attackIncooldown = true;
        }
    }
    private void SelectAttack()
    {
        switch (attackCount)
        {
            case 0:
                CheckAllNearbyColliders();
                attackCount++;
                StartCoroutine(ResetAttack(timeBetweenAttacks));
                break;
            case 1:
                CheckAllNearbyColliders();
                attackCount++;
                StartCoroutine(ResetAttack(timeBetweenAttacks));
                break;
            case 2:
                CheckAllNearbyColliders();
                attackCount = 0;
                StartCoroutine(ResetAttack(timeToResetAttack));
                break;
        }
    }

    private IEnumerator ResetAttack(float timeToReset)
    {
        yield return new WaitForSeconds(timeToReset);
        attackIncooldown = false;
    }
    private void CheckAllNearbyColliders()
    {
        foreach(Collider enemy in GetAllNearbyColliders())
        {
            Vector3 vectorToCollider = enemy.transform.position - gameObject.transform.position;
            vectorToCollider = vectorToCollider.normalized;

            if (Vector3.Dot(vectorToCollider, gameObject.transform.forward) > 0.5)
            {
                if (!HasWallInfront(enemy.transform))
                {
                    DamageEnemy(enemy);
                }             
            }
        }
    }

    private Collider[] GetAllNearbyColliders()
    {
        return Physics.OverlapSphere(attackCollisionCheck.position, attackRange, damageableLayer);
    }


    private bool HasWallInfront(Transform enemyTransform)
    {
        Vector3 direction = (enemyTransform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawLine(transform.position, hit.transform.position, Color.red, 1f);
            if(hit.transform != enemyTransform)
            {
                return true;
            }
        }
        return false;
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
