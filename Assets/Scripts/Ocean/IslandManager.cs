using DG.Tweening;
using UnityEngine;

public class IslandManager : MonoBehaviour
{
    [SerializeField] private float scaleAmount;


    private void OnTriggerEnter(Collider other)
    {
        ShipController shipController = other.gameObject.GetComponent<ShipController>();
        if (shipController != null)
        {
            //transform.DOScale(scaleAmount);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        
    }
}
