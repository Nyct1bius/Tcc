using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents
{
    public static event UnityAction<WeaponSO> SwordPickUp;
    public static void OnSwordPickUp(WeaponSO currentWeapon) => SwordPickUp?.Invoke(currentWeapon);

    public static event UnityAction AttackFinished;
    public static void OnAttackFinished() => AttackFinished?.Invoke();


}
