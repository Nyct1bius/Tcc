using System;
using UnityEngine;

public class ShipInteraction : MonoBehaviour, IInteractable
{
    public bool isPickable { get; set;}

    public void HasBeenPickedUp()
    {
        Debug.Log("Player interact");
    }
}
