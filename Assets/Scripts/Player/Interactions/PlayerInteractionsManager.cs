using UnityEngine;

public class PlayerInteractionsManager : MonoBehaviour
{
    [SerializeField] private bool hasItemToInteract;
    [SerializeField] private GameObject uiPickUp;
    private IInteractable currentInteractable;
    private Vector3 currentItemTransform;
    private GameObject uiSpawned;
    private void OnEnable()
    {
        PlayerEvents.Interact += HandleInteraction;
    }
    private void OnDisable()
    {
        PlayerEvents.Interact -= HandleInteraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentInteractable = other.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            currentItemTransform = other.transform.position;
            hasItemToInteract = true;
            uiSpawned = Instantiate(uiPickUp, currentItemTransform + Vector3.up * 2, Quaternion.identity);
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (currentInteractable != null)
        {
            Destroy(uiSpawned);
            currentInteractable = null;
        }
    }





    private void HandleInteraction()
    {
      

        if (hasItemToInteract)
        {
            currentInteractable.HasBeenPickedUp();
            hasItemToInteract = false;
            Destroy(uiSpawned);
        }
    }


}
