using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents
{
    public static event UnityAction SwordPickUp;
    public static void OnSwordPickUp() => SwordPickUp?.Invoke();

}
