using System;
using UnityEngine;

public class ShipInteraction : MonoBehaviour, IInteractable
{
    public bool isPickable { get; set;}

    public void OnPickedUp()
    {
        Debug.Log("Player interact");
    }
}
