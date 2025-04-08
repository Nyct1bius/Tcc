using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UIButton = UnityEngine.UI.Button;

public class InventoryManager : MonoBehaviour
{

    [System.Serializable]
    public class Item
    {
        public string name;
        public int quantity;
        public GameObject buttonUI;
        public Sprite icon;
        public string description;

        public Item(string name, Sprite icon, string description, GameObject buttonUI)
        {
            this.name = name;
            this.quantity = 1;
            this.icon = icon;
            this.description = description;
            this.buttonUI = buttonUI;
        }
    }

    private List<Item> itens = new List<Item>();

    [Header("UI References")]
    public GameObject itemButtonPrefab;
    public Transform itensBox;
    public Image itemImageDisplay;
    public TextMeshProUGUI itemDescriptionDisplay;

    [Header("Item Data")]
    public List<Sprite> itemIcons;
    public List<string> itemDescriptions;
    public List<string> itemNames;

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
            // Busca dados do item
            int index = itemNames.IndexOf(name);
            Sprite icon = (index >= 0 && index < itemIcons.Count) ? itemIcons[index] : null;
            string description = (index >= 0 && index < itemDescriptions.Count) ? itemDescriptions[index] : "Sem descrição.";

            // Cria botão
            GameObject newButton = Instantiate(itemButtonPrefab, itensBox);
            TextMeshProUGUI textComponent = newButton.GetComponentInChildren<TextMeshProUGUI>();

            // Cria item
            Item newItem = new Item(name, icon, description, newButton);
            itens.Add(newItem);
            textComponent.text = $"{name} x{newItem.quantity}";

            // Adiciona evento de clique no botão
            UIButton buttonComponent = newButton.GetComponent<UIButton>();
            buttonComponent.onClick.AddListener(() => SelectItem(newItem));
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

    // Chamada quando o jogador clica em um botão de item
    private void SelectItem(Item item)
    {
        itemImageDisplay.sprite = item.icon;
        itemImageDisplay.enabled = true;
        itemDescriptionDisplay.text = item.description;
    }
}
