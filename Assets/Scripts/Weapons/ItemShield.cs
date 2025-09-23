using DG.Tweening;
using UnityEngine;

public class ItemShield : Item, IProximityEventTrigger, IDataPersistence
{
    bool hasSword;
    private void Start()
    {
        DataPersistenceManager.instance.LoadGame();
        if (hasSword)
        {
            Destroy(gameObject);
        }
        transform.DORotate(new Vector3(0, 180f, 0), 10f).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Linear);
        transform.DOLocalMoveY(transform.position.y + 1f, 3f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }
    private void OnEnable()
    {
        GetItemHeight();
    }
    private void OnDisable()
    {
        transform.DOKill();
    }
    public override void OnPickedUp()
    {
        transform.DOKill();
        PlayerEvents.OnShieldPickUp();
        OnExit();
        Destroy(gameObject);
    }

    public void OnEnter()
    {
        CameraTargetingTrigger.Instance.TrackingTriggerEnter(this.transform);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInteractionsManager>() != null)
        {
            CameraTargetingTrigger.Instance.TrackingTriggerExit();
        }
    }
    public void OnExit()
    {
        CameraTargetingTrigger.Instance.TrackingTriggerExit();
    }

    public void LoadData(PlayerData data)
    {
        hasSword = data.hasShield;
    }

    public void SaveData(PlayerData data)
    {

    }
}
