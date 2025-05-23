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

    public static event UnityAction HitEnemy;
    public static void OnHitEnemy() => HitEnemy?.Invoke();

}
