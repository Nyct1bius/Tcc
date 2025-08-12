using System;
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
    [Header("Scenes Interactions")]
    [Range(0f, 5f)]
    [SerializeField] private float interactionRange;
    [SerializeField] private LayerMask interactionLayer;
    private Collider[] interactables = new Collider[10];
    private List<Collider> colliders = new List<Collider>();


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
        DetectNearEvents(trigger => trigger.OnEnter());

    }
    private void OnTriggerExit(Collider other)
    {
        if (interactableItens.Count != 0)
        {
            Item exitItem = other.GetComponent<Item>();
           CleanListOfItens(exitItem);
        }
        //DetectNearEvents(trigger => trigger.OnExit());
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
    private void DetectNearEvents(Action<IProximityEventTrigger> eventAction)
    {
        CleanTargetsList();
        int count = Physics.OverlapSphereNonAlloc(transform.position, interactionRange, interactables);
        colliders = new List<Collider>();

        for (int i = 0; i < count; i++)
        {
            Collider col = interactables[i];
            colliders.Add(col);
        }

        colliders.Sort((a, b) =>
        {
            float distA = Vector3.Distance(a.transform.position, transform.position);
            float distB = Vector3.Distance(b.transform.position, transform.position);
            return distA.CompareTo(distB);
        });

        for (int i = 0; i < count; i++)
        {
            IProximityEventTrigger eventTrigger = colliders[i].gameObject.GetComponent<IProximityEventTrigger>();
            if (eventTrigger != null)
            {
                eventAction(eventTrigger);
                Debug.Log(eventAction);
                break;
            }
        }
    }

    public void CleanTargetsList()
    {
        colliders.RemoveAll(item => item == null);
        colliders.RemoveAll(item => Vector3.Distance(item.transform.position, transform.position) > interactionRange);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }

}
