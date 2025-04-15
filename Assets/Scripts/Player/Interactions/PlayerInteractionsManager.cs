using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionsManager : MonoBehaviour
{
    [SerializeField] private bool hasItemToInteract;
    private InputReader inputReader;
    [SerializeField] private GameObject uiPickUp;
    [SerializeField] private Notifier notifier;
    private IInteractable currentInteractable;
    private Transform currentItemTransform;
    private GameObject uiSpawned;


    //Itens
    private List<IInteractable> interactableItens = new List<IInteractable>();
    private List<GameObject> uiSpawneds = new List<GameObject>();


    //CheckPoint
    private CheckPoint checkPoint;

    private void Awake()
    {
        inputReader = GetComponentInParent<PlayerStateMachine>().inputReader;
    }
    private void OnEnable()
    {
        inputReader.InteractEvent += HandleInteraction;
    }
    private void OnDisable()
    {
        inputReader.InteractEvent -= HandleInteraction;
    }

    private void OnTriggerEnter(Collider other)
    {
        currentInteractable = other.GetComponent<IInteractable>();
        if (currentInteractable != null)
        {
            currentItemTransform = other.transform;
            SetupItensToInteract();
        }

        checkPoint = other.GetComponent<CheckPoint>();
        if(checkPoint!= null)
        {
            checkPoint.SetupCheckpoint();
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        if (interactableItens.Count != 0)
        {
            IInteractable exitItem = other.GetComponent<IInteractable>();
           CleanListOfItens(exitItem);
        }
    }

    private void CleanListOfItens(IInteractable exitItem)
    {
        for (int i = 0; i < interactableItens.Count; i++)
        {
            if (exitItem == interactableItens[i])
            {
                interactableItens.Remove(interactableItens[i]);
                Destroy(uiSpawneds[i]);

                uiSpawneds.RemoveAt(i);
            }

        }
    }

    private void SetupItensToInteract()
    {
        hasItemToInteract = true;
        uiSpawned = Instantiate(uiPickUp, currentItemTransform.position + Vector3.up * 2, Quaternion.identity, currentItemTransform);
        interactableItens.Add(currentInteractable);
        uiSpawneds.Add(uiSpawned);
    }



    private void HandleInteraction()
    {
      

        if (hasItemToInteract)
        {
            for(int i = 0; i < interactableItens.Count; i++)
            {
                interactableItens[i].OnPickedUp();
                notifier.NotifyGreen("Collected: " + interactableItens[i]);
                interactableItens.Remove(interactableItens[i]);
            }
        }
    }


}
