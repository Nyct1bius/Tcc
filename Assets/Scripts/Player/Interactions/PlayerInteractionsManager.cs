using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionsManager : MonoBehaviour
{
    [SerializeField] private bool hasItemToInteract;
    private InputReader inputReader;
    [SerializeField] private GameObject uiPickUp;
    [SerializeField] private Notifier notifier;
    [SerializeField] private float uiOffset;
    private Item currentInteractable;
    private Transform currentItemTransform;
    private GameObject uiSpawned;


    //Itens
    private List<Item> interactableItens = new List<Item>();
    private List<GameObject> spawnedUIs = new List<GameObject>();
    private float itemHeight;


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
        currentInteractable = other.GetComponent<Item>();
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
            Item exitItem = other.GetComponent<Item>();
           CleanListOfItens(exitItem);
        }
    }

    private void CleanListOfItens(Item exitItem)
    {
        for (int i = 0; i < interactableItens.Count; i++)
        {
            if (exitItem == interactableItens[i])
            {
                interactableItens.Remove(interactableItens[i]);
                Destroy(spawnedUIs[i]);

                spawnedUIs.RemoveAt(i);
            }

        }
    }

    private void SetupItensToInteract()
    {
        hasItemToInteract = true;
        uiSpawned = Instantiate(uiPickUp,currentItemTransform.position + Vector3.up * (currentInteractable.height + uiOffset), Quaternion.identity); 
        interactableItens.Add(currentInteractable);
        spawnedUIs.Add(uiSpawned);
    }



    private void HandleInteraction()
    {
      

        if (hasItemToInteract)
        {
            for(int i = 0; i < interactableItens.Count; i++)
            {
                interactableItens[i].OnPickedUp();
                notifier.NotifyGreen("Collected: " + interactableItens[i].name);
                interactableItens.Remove(interactableItens[i]);
                Destroy(spawnedUIs[i]);

                spawnedUIs.RemoveAt(i);
            }
        }
    }


}
