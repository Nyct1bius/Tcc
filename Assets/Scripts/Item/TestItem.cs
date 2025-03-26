using UnityEngine;

public class TestItem : MonoBehaviour, IInteractable
{
    public void HasBeenPickedUp()
    {
        Debug.Log("PickUp item");
        Destroy(gameObject);
    }
}
