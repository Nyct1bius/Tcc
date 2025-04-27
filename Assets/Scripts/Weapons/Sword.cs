using UnityEngine;

public class Sword : Item
{
    [SerializeField] private PlayerStatsSO _playerStats;
    private void Start()
    {
        if (_playerStats.hasSword)
        {
            Destroy(gameObject);
        }
    }
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


