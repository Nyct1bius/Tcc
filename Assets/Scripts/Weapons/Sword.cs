using UnityEngine;

public class Sword : Item
{
    public override void OnPickedUp()
    {
       PlayerEvents.OnSwordPickUp(weaponData);
       Destroy(gameObject);
    }
}


