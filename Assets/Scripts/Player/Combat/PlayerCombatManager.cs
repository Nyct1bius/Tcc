using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

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
    [Header("Lock On")]
    [SerializeField] private float _lockOnRange;
    private Collider[] _lookAtTargets = new Collider[10];
    [SerializeField] List<Collider> detectedEnemys;
    [SerializeField] private float _lookRotationSpeed;
    private bool _searchForTargets;
    private Vector3 _targetPosition;
    private Vector3 _lookDirection;
    private Quaternion _lookRotation;
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
        GameEvents.EnterCombat += OnEnterCombat;
        GameEvents.ExitCombat += OnExitCombat;
        _machine.inputReader.LockOnEnemy += LockOnEnemy;
    }
    private void OnDisable()
    {
        _machine.inputReader.AttackEvent += CheckAttackButton;
        PlayerEvents.SwordPickUp += AddSword;
        PlayerEvents.AttackFinished += HandleResetAttack;
        PlayerEvents.AttackVfx -= SpawnVFXAttack;
        GameEvents.EnterCombat -= OnEnterCombat;
        GameEvents.ExitCombat -= OnExitCombat;
        _machine.inputReader.LockOnEnemy -= LockOnEnemy;
    }


    private void Start()
    {
        if (_playerStats.hasSword)
        {
            AddSword(_backupWeaponData);
        }
    }
    private void Update()
    {
        if (!_searchForTargets) return;
        LookAtTarget();
    }
    private void OnEnterCombat()
    {
       //To do
    }

    private void OnExitCombat()
    {
        //To do
    }
    #region Lock On
    private void LockOnEnemy()
    {
        _searchForTargets = !_searchForTargets;
        GetLookAtTargets();
    }
    private void GetLookAtTargets()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _lockOnRange, _lookAtTargets, _damageableLayer);
        detectedEnemys = new List<Collider>();

        for (int i = 0; i < count; i++)
        {
            Collider col = _lookAtTargets[i];
            detectedEnemys.Add(col);
        }

        detectedEnemys.Sort((a, b) =>
        {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });
    }
    private void LookAtTarget()
    {
        CleanTargetsList();
        if(detectedEnemys.Count == 0)
        {
            _searchForTargets = false;
            return;
        }

        if (detectedEnemys[0] != null)
        {
            _lookDirection = (detectedEnemys[0].transform.position - transform.position).normalized;
            _lookRotation = Quaternion.LookRotation(new Vector3(_lookDirection.x, 0, _lookDirection.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _lookRotationSpeed);

        }
    
    }
    private void CleanTargetsList()
    {
        detectedEnemys.RemoveAll(item => item == null);
        detectedEnemys.RemoveAll(item => Vector3.Distance(item.transform.position, transform.position) > _lockOnRange);
    }
    private Collider[] GetAllNearbyColliders()
    {
        return Physics.OverlapSphere(transform.position, _lockOnRange, _damageableLayer);
    }

    #endregion


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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _lockOnRange);
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
