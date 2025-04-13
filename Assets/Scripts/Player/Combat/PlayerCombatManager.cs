using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatManager : MonoBehaviour
{
    //wepon variables
    private bool hasWeapon;
    private InputReader inputReader;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private Transform attackCollisionCheck;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private WeaponSO currentWeaponData;
    private GameObject currentWeaponVisual;


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

    private void AddSword(WeaponSO currentWeapon)
    {
        if(currentWeaponVisual != null)
        {
            Destroy(currentWeaponVisual);   
        }
        currentWeaponData = currentWeapon;
        hasWeapon = true;
        currentWeaponVisual = Instantiate(currentWeaponData.weaponVisual, weaponPos);


    }

    private void PerformAttack()
    {
        if(hasWeapon && !attackIncooldown)
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
                currentWeaponData.OnAttack(transform, damageableLayer);
                attackCount++;
                StartCoroutine(ResetAttack(timeBetweenAttacks));
                break;
            case 1:
                currentWeaponData.OnAttack(transform, damageableLayer);
                attackCount++;
                StartCoroutine(ResetAttack(timeBetweenAttacks));
                break;
            case 2:
                currentWeaponData.OnAttack(transform, damageableLayer);
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



    private void OnDrawGizmos()
    {
        if (hasWeapon)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(attackCollisionCheck.position, currentWeaponData.attackRange);
        }
    }
}
