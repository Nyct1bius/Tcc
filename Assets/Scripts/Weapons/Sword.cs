using DG.Tweening;
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
        transform.DORotate(new Vector3(0, 180f, 0),10f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        transform.DOLocalMoveY(transform.position.y + 1f, 3f).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
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


