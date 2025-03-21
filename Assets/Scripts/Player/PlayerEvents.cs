using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents
{
    public static event UnityAction Jump;
    public static void OnJump() => Jump?.Invoke();

    public static event UnityAction Interact;
    public static void OnInteract() => Interact?.Invoke();


    public static event UnityAction SwordPickUp;
    public static void OnSwordPickUp() => SwordPickUp?.Invoke();

    public static event UnityAction Attack;
    public static void OnAttack() => Attack?.Invoke();

    public static event UnityAction Dash;
    public static void OnDash() => Dash?.Invoke();


}
