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
    [SerializeField] private Transform _vfxAttackSpawnpoint;
    [SerializeField] private Transform _vfxLightniningSpawnpoint;
    [SerializeField] private GameObject _vfxLightnining;

    #region Getters and Setters

    //COMBAT
    public int AttackCount { get { return _attackCount; } set { _attackCount = value; } }
    public bool AttackIncooldown { get { return _attackIncooldown; } set { _attackIncooldown = value; } }
    public float TimeBetweenAttacks { get { return _timeBetweenAttacks; } }
    public bool IsAttacking { get { return _isAttacking; } }
    public LayerMask DamageableLayer { get { return _damageableLayer; } }
    public WeaponSO CurrentWeaponData { get { return _currentWeaponData; } }

    //VFX
    public GameObject VfxLightnining { get { return _vfxLightnining; } }
        

    #endregion

    private void OnEnable()
    {
        _machine.inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
        PlayerEvents.AttackVfx += SpawnVFXAttack;
    }

    private void OnDisable()
    {
        _machine.inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
        PlayerEvents.AttackVfx -= SpawnVFXAttack;
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

        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(_attackCollisionCheck.position, _backupWeaponData.attackRange);
    }


    private void SpawnVFXAttack()
    {
        Instantiate(_currentWeaponData.vfxAttacks[_attackCount], _vfxAttackSpawnpoint.position,_vfxAttackSpawnpoint.rotation);
    }
    
    public void SpawnVFXLightining()
    {
        Instantiate(_vfxLightnining, _vfxLightniningSpawnpoint);
    }

    #endregion
}
