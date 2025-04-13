using UnityEngine;

public class TestItem : Item
{
    public override void OnPickedUp()
    {
        Debug.Log("PickUp item");
        Destroy(gameObject);
    }
}
