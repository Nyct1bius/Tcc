using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerCombatManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerStateMachine _machine;
    [SerializeField] private PlayerStatsSO _playerStats;

    [Header("Weapon")]
    [SerializeField] private LayerMask _damageableLayer;
    [SerializeField] private Transform _attackCollisionCheck;
    [SerializeField] private Transform _weaponPos;
    [SerializeField] private WeaponSO _currentWeaponData;
    [SerializeField] private WeaponSO _backupWeaponData;
    private GameObject _currentWeaponVisual;


    [Header("Combat")]
    [SerializeField] private bool _attackIncooldown;
    [SerializeField] private float _timeBetweenAttacks = 0.5f;
    [SerializeField] private int _attackCount;
    private bool _isAttacking;
    [Header("VFX")]
    [SerializeField] private GameObject _vfxAttack;

    #region Getters and Setters

    //COMBAT
    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }
    public bool AttackIncooldown { get { return _attackIncooldown; } set { _attackIncooldown = value; } }
    public float TimeBetweenAttacks { get { return _timeBetweenAttacks; } }
    public bool IsAttacking { get { return _isAttacking; } }
    public LayerMask DamageableLayer { get { return _damageableLayer; } }
    public WeaponSO CurrentWeaponData { get { return _currentWeaponData; } }

    //VFX
    public GameObject VfxAttack { get { return _vfxAttack; } set {_vfxAttack = value; } }

    #endregion

    private void OnEnable()
    {
        _machine.inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
        PlayerEvents.AttackVfx += SpwanVfxAttack;
    }

    private void OnDisable()
    {
        _machine.inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
        PlayerEvents.AttackVfx -= SpwanVfxAttack;
    }
    private void Start()
    {
        if (_playerStats.hasSword)
        {
            AddSword(_backupWeaponData);
        }
    }
    #region Combat

    private void AddSword(WeaponSO currentWeapon)
    {
        if (_currentWeaponVisual != null)
        {
            Destroy(_currentWeaponVisual);
        }
        _currentWeaponData = currentWeapon;
        _currentWeaponVisual = Instantiate(_currentWeaponData.weaponVisual, _weaponPos);
        _playerStats.hasSword = true;

    }

    private void CheckAttackButton(bool attacking)
    {
        _isAttacking = attacking;
    }

    public void HandleResetAttack()
    {
        _attackIncooldown = false;
    }
    private void OnDrawGizmos()
    {
        if (_currentWeaponData != null)
        {
            Gizmos.color = Color.blue;

            Gizmos.DrawWireSphere(_attackCollisionCheck.position, _currentWeaponData.attackRange);
        }
    }


    private void SpwanVfxAttack()
    {
        Instantiate(_currentWeaponData.vfxAttacks[_attackCount], _vfxAttack.transform.position,_vfxAttack.transform.rotation);
    }

    #endregion
}
