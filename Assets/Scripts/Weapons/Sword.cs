using UnityEngine;

public class Sword : Item
{
    private void OnEnable()
    {
        GetItemHeight();
    }
    public override void OnPickedUp()
    {
       PlayerEvents.OnSwordPickUp(weaponData);
       Destroy(gameObject);
    }
}


