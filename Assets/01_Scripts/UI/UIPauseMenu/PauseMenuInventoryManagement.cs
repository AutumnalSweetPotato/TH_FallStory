using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuInventoryManagement : MonoBehaviour
{
    [SerializeField] private PauseMenuInventoryManagementSlot[] inventoryManagementSlots = null;
    public GameObject inventoryManagementDraggedItemPrefab;
    [SerializeField] private Sprite transparentSprite = null;
    [HideInInspector] public GameObject inventoryTextBoxGameObject = null;

    private void OnEnable()
    {
        EventHandler.InventoryUpdateEvent += PopulatePlayerInventory;

        if (InventoryManager.Instance != null)
        {
            PopulatePlayerInventory(InventoryLocation.Player, InventoryManager.Instance.inventoryLists[(int)InventoryLocation.Player]);
        }
    }
    private void OnDisable()
    {
        EventHandler.InventoryUpdateEvent -= PopulatePlayerInventory;
        DestroyInventoryTextBoxGameobject();
    }



    public void DestroyInventoryTextBoxGameobject()
    {
        if(inventoryTextBoxGameObject != null)
        {
            Destroy(inventoryTextBoxGameObject);
        }
    }
    public void DestroyCurrentDraggedItems()
    {
        for(int i = 0;i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.Player].Count;i++)
        {
            if(inventoryManagementSlots[i].draggedItem != null)
            {
                Destroy(inventoryManagementSlots[i].draggedItem);
            }
        }
    }

    

    private void PopulatePlayerInventory(InventoryLocation player, List<InventoryItem> inventoryItems)
    {
        if(player == InventoryLocation.Player)
        {
            InitialiseInventoryManagementSlots();

            for(int i = 0; i < InventoryManager.Instance.inventoryLists[(int)InventoryLocation.Player].Count; i++)
            {
                inventoryManagementSlots[i].itemDetails = InventoryManager.Instance.GetItemDetailsByID(inventoryItems[i].itemID);
                inventoryManagementSlots[i].itemQuintity = inventoryItems[i].itemQuantity;

                if(inventoryManagementSlots[i].itemDetails != null)
                {
                    inventoryManagementSlots[i].inventoryManagementSlotImage.sprite = inventoryManagementSlots[i].itemDetails.itemSprite;
                    inventoryManagementSlots[i].textMeshProUGUI.text = inventoryManagementSlots[i].itemQuintity.ToString();
                }
            }
        }
    }

    private void InitialiseInventoryManagementSlots()
    {
        for(int i = 0;i < Settings.playerMaxInventoryCapacity; i++)
        {
            inventoryManagementSlots[i].greyedOutImageGO.gameObject.SetActive(false);
            inventoryManagementSlots[i].itemDetails = null;
            inventoryManagementSlots[i].itemQuintity = 0;
            inventoryManagementSlots[i].inventoryManagementSlotImage.sprite = transparentSprite;
            inventoryManagementSlots[i].textMeshProUGUI.text = "";
        }

        for(int i = InventoryManager.Instance.inventoryListCapacityIntArray[(int)InventoryLocation.Player];i <Settings.playerMaxInventoryCapacity; i++)
        {
            inventoryManagementSlots[i].greyedOutImageGO.gameObject.SetActive(true);
        }
    }
}
