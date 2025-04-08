using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public string name;
        public int quantity;
        public GameObject buttonUI; // botão associado a esse item

        public Item(string name, GameObject buttonUI)
        {
            this.name = name;
            this.quantity = 1;
            this.buttonUI = buttonUI;
        }
    }

    private List<Item> itens = new List<Item>();

    [Header("UI Elements")]
    public GameObject itemButtonPrefab; // Prefab do botão
    public Transform itensBox;          // Objeto vazio que tem o Vertical Layout Group

    public void GetIten(string name)
    {
        Item existingItem = itens.Find(item => item.name == name);

        if (existingItem != null)
        {
            existingItem.quantity++;
            UpdateButtonText(existingItem);
        }
        else
        {
            GameObject newButton = Instantiate(itemButtonPrefab, itensBox);
            TextMeshProUGUI textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();
            Item newItem = new Item(name, newButton);
            itens.Add(newItem);
            textComponent.text = $"{name} x{newItem.quantity}";
        }
    }

    private void UpdateButtonText(Item item)
    {
        if (item.buttonUI != null)
        {
            TextMeshProUGUI textComponent = item.buttonUI.GetComponentInChildren<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = $"{item.name} x{item.quantity}";
            }
        }
    }
}
