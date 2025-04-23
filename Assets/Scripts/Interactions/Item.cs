using UnityEngine;

public abstract class Item : MonoBehaviour, IInteractable
{
    public bool isImportantItem;
    public GameObject visual;
    public string Name;
    public WeaponSO weaponData;
    public float height;

    public virtual void OnPickedUp()
    {
        
    }
    protected void GetItemHeight()
    {
        height = transform.localScale.y;
    }
}
