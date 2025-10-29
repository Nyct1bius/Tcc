using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ItemShield : Item, IProximityEventTrigger, IDataPersistence
{
   [SerializeField] bool hasShield;
  
    private void OnEnable()
    {
        StartCoroutine(DelayStart());
    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(.5f);
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
        hasShield = true;
        DataPersistenceManager.Instance.SaveGame();
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
        hasShield = data.hasShield;

        if (hasShield)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void SaveData(PlayerData data)
    {
        data.hasShield = hasShield;
    }
}
