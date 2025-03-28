using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionsManager : MonoBehaviour
{
    [SerializeField] private bool hasItemToInteract;
    [SerializeField] private GameObject uiPickUp;
    [SerializeField] private Notifier notifier;
    private IInteractable currentInteractable;
    private Transform currentItemTransform;
    private GameObject uiSpawned;


    //Itens
    private List<IInteractable> interactableItens = new List<IInteractable>();
    private List<GameObject> uiSpawneds = new List<GameObject>();


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
            currentItemTransform = other.transform;
            Debug.Log(currentItemTransform.name);
            hasItemToInteract = true;
            uiSpawned = Instantiate(uiPickUp,currentItemTransform.position + Vector3.up * 2, Quaternion.identity, currentItemTransform);
            interactableItens.Add(currentInteractable);
            uiSpawneds.Add(uiSpawned);
        }
        

    }
    private void OnTriggerExit(Collider other)
    {
        if (interactableItens.Count != 0)
        {
            IInteractable exitItem = other.GetComponent<IInteractable>();
            for (int i = 0; i < interactableItens.Count; i++)
            {
                if(exitItem == interactableItens[i])
                {
                    interactableItens.Remove(interactableItens[i]);
                    Destroy(uiSpawneds[i]);

                    uiSpawneds.RemoveAt(i);
                }
            
            }

        }
    }





    private void HandleInteraction()
    {
      

        if (hasItemToInteract)
        {
            for(int i = 0; i < interactableItens.Count; i++)
            {
                interactableItens[i].HasBeenPickedUp();
                notifier.NotifyGreen("Collected" + interactableItens[i]);
                interactableItens.Remove(interactableItens[i]);
            }
        }
    }


}
