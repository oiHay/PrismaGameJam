using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("Item selecionado")]
    public TMP_Text selectedItemName;
    public Image selectedItemImage;
    public TMP_Text selectedItemDescription;

    [Header("Lista de itens")]
    public Transform itemsListContainer;
    public Button itemSlotPrefab;

    [Header("Apresentar")]
    // public Button presentButton;

    private ItemSO selectedItem;
    private DialogueManager dialogueManager; // null se não veio do diálogo

    public void Open(DialogueManager fromDialogue = null)
    {
        dialogueManager = fromDialogue;
        // presentButton.gameObject.SetActive(dialogueManager != null);

        if(dialogueManager != null)
        {
            dialogueManager.optionsPanel.SetActive(false);
        }

        gameObject.SetActive(true);
        PopulateList();
    }

    public void OpenWithoutDialogue()
    {
        Open();
    }

    void PopulateList()
    {
        foreach (Transform child in itemsListContainer)
            Destroy(child.gameObject);

        List<ItemSO> items = PlayerInventory.Instance.collectedItems;

        foreach (var item in items)
        {
            Button slot = Instantiate(itemSlotPrefab, itemsListContainer);
            slot.gameObject.SetActive(true);
            
            ItemSlot slotScript = slot.GetComponent<ItemSlot>();
            slotScript.iconImage.sprite = item.icon;

            ItemSO captured = item;
            slot.onClick.AddListener(() => SelectItem(captured));
        }

        if (items.Count > 0)
            SelectItem(items[0]);
        else
            ClearSelection();
    }

    void SelectItem(ItemSO item)
    {
        selectedItem = item;
        selectedItemName.text = item.itemName;
        selectedItemImage.sprite = item.icon;
        selectedItemDescription.text = item.description;
    }

    void ClearSelection()
    {
        selectedItem = null;
        selectedItemName.text = "";
        selectedItemImage.sprite = null;
        selectedItemDescription.text = "";
    }

    public void OnPresentClicked()
    {
        if (selectedItem == null || dialogueManager == null) return;

        gameObject.SetActive(false);
        dialogueManager.PresentItem(selectedItem);
    }

    public void Close()
    {
        gameObject.SetActive(false);

        if(dialogueManager != null)
        {
            dialogueManager.optionsPanel.SetActive(true);
            dialogueManager = null;
        }
        
    }
}