using UnityEngine;

public class Sword : MonoBehaviour,IInteractable
{
    public void HasBeenPickedUp()
    {
       PlayerEvents.OnSwordPickUp();
        Destroy(gameObject);
    }
 
}
