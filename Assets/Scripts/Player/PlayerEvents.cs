using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents
{
    public static event UnityAction<WeaponSO> SwordPickUp;
    public static void OnSwordPickUp(WeaponSO currentWeapon) => SwordPickUp?.Invoke(currentWeapon);

    public static event UnityAction AttackFinished;
    public static void OnAttackFinished() => AttackFinished?.Invoke();

    public static event UnityAction DashFinished;
    public static void OnDashFinished() => DashFinished?.Invoke();

    public static event UnityAction AttackVfx;
    public static void OnAttackVfx() => AttackVfx?.Invoke();


    public static event UnityAction StartAttackDetection;
    public static void OnStartAttackDetection() => StartAttackDetection?.Invoke();

    public static event UnityAction<Vector3> HitEnemy;
    public static void OnHitEnemy(Vector3 enemyPos) => HitEnemy?.Invoke(enemyPos);
    #region SFX
    public static event UnityAction AttackSFX;
    public static void OnAttackSFX() => AttackSFX?.Invoke();

    public static event UnityAction WalkSFX;
    public static void OnWalkSFX() => WalkSFX?.Invoke();

    public static event UnityAction DashSFX;
    public static void OnDashSFX() => DashSFX?.Invoke();

    public static event UnityAction JumpSFX;
    public static void OnJumpSFX() => JumpSFX?.Invoke();

    public static event UnityAction LandSFX;
    public static void OnLandSFX() => LandSFX?.Invoke();

    public static event UnityAction IdleSFX;
    public static void OnIdleSFX() => IdleSFX?.Invoke();

    public static event UnityAction ChickenSFX;
    public static void OnChickenSFX() => ChickenSFX?.Invoke();

    public static event UnityAction StopChickenSFX;
    public static void OnStopChickenSFX() => StopChickenSFX?.Invoke();


    #endregion

}
