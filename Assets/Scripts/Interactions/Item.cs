using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public bool isImportantItem;
    public GameObject visual;
    public WeaponSO weaponData;

    public virtual void OnPickedUp()
    {
        
    }
}
