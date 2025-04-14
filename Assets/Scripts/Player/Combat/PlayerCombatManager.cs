using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatManager : MonoBehaviour
{
    //Components
    [SerializeField] private InputReader inputReader;
    [SerializeField] private PlayerMovement inputs;

    [Header("State")]
    [SerializeField] private AttackState attackState;


    //wepon variables
    private bool hasWeapon;
    [SerializeField] private LayerMask damageableLayer;
    [SerializeField] private Transform attackCollisionCheck;
    [SerializeField] private Transform weaponPos;
    [SerializeField] private WeaponSO currentWeaponData;
    private GameObject currentWeaponVisual;


    //Combat variables
    public int attackCount;
    [SerializeField] private bool attackIncooldown;
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    private bool isAttacking;
    private void OnEnable()
    {
        inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
    }
    private void OnDisable()
    {
        inputReader.AttackEvent -= CheckAttackButton;
        PlayerEvents.SwordPickUp -= AddSword;
        PlayerEvents.AttackFinished -= HandleResetAttack;
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
    private void CheckAttackButton(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }

    private void Update()
    {
        PerformAttack();
    }

    private void PerformAttack()
    {
        if(currentWeaponData!= null && isAttacking && !attackIncooldown)
        {
            Debug.Log("Attack");
            attackIncooldown = true;
            SelectAttack();
        }
    }
    public void SelectAttack()
    {
        inputs.machine.Set(attackState, true);
        currentWeaponData.OnAttack(transform, damageableLayer);

        attackCount++;

        if (attackCount >= 3)
        {
            attackCount = 0;
        }

    }

    public void HandleResetAttack()
    {
       StartCoroutine(WaitToResetAttack());
    }
    private IEnumerator WaitToResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        attackIncooldown = false;
    }

    public bool IsAttackHeld()
    {
        return isAttacking;
    }

    public void ResetCombo()
    {
        attackCount = 0;
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
