using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryBar : MonoBehaviour
{
    //创建全透明Sprite
    [SerializeField] private Sprite transparentSprite;
    [SerializeField] private UIInventorySlot[] inventorySlots;
    public GameObject inventoryBarDraggedItem;
    private RectTransform rectTransform;
    private bool isInventoryBarPositionBottom = true;
    [HideInInspector] public GameObject inventoryTextboxGameObject;
    public bool IsInventoryBarPositionBottom
    {
        get => isInventoryBarPositionBottom;
        set => isInventoryBarPositionBottom = value;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ClearInventorySlots();
    }
    private void Update()
    {
        SwitchInventoryBarPosition();
    }

    private void OnEnable()
    {
        EventHandler.InventoryUpdateEvent += InventoryUpdate;
    }



    private void OnDisable()
    {
        EventHandler.InventoryUpdateEvent -= InventoryUpdate;
    }
    private void SwitchInventoryBarPosition()
    {
        Vector3 playerViewportPosition = Player.Instance.GetPlayerViewportPosition();
        if (playerViewportPosition.y > 0.3 && IsInventoryBarPositionBottom == false)
        {
            rectTransform.pivot = new Vector2(0.5f, 0);
            rectTransform.anchorMin = new Vector2(0.5f, 0);
            rectTransform.anchorMax = new Vector2(0.5f, 0);
            rectTransform.anchoredPosition = new Vector2(0, 4.5f);
            IsInventoryBarPositionBottom = true;
        }
        else if (playerViewportPosition.y < 0.3 && IsInventoryBarPositionBottom == true)
        {
            rectTransform.pivot = new Vector2(0.5f, 1);
            rectTransform.anchorMin = new Vector2(0.5f, 1);
            rectTransform.anchorMax = new Vector2(0.5f, 1);
            rectTransform.anchoredPosition = new Vector2(0, -4.5f);
            IsInventoryBarPositionBottom = false;
        }
    }

    private void InventoryUpdate(InventoryLocation location, List<InventoryItem> list)
    {
        if (location == InventoryLocation.Player)
        {
            ClearInventorySlots();
            if (inventorySlots.Length > 0 && list.Count > 0)
            {
                for (int i = 0; i < inventorySlots.Length; i++)
                {
                    if (i < list.Count)
                    {
                        int itemID = list[i].itemID;
                        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetailsByID(itemID);
                        if (itemDetails != null)
                        {
                            inventorySlots[i].image.sprite = itemDetails.itemSprite;
                            inventorySlots[i].textCount.text = list[i].itemQuantity.ToString();
                            inventorySlots[i].itemDetails = itemDetails;
                            inventorySlots[i].itemQuanetity = list[i].itemQuantity;
                            SetHighlightOnInventorySlots(i);
                        }
                    }
                    else
                        break;
                }
            }
        }
    }

    private void ClearInventorySlots()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].image.sprite = transparentSprite;
            inventorySlots[i].textCount.text = "";
            inventorySlots[i].itemDetails = null;
            inventorySlots[i].itemQuanetity = 0;

            SetHighlightOnInventorySlots(i);
        }
    }

    public void ClearHighlightOnInventorySlots()
    {
        if (inventorySlots.Length > 0)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                if (inventorySlots[i].isSelected)
                {
                    inventorySlots[i].isSelected = false;
                    inventorySlots[i].highlight.gameObject.SetActive(false);
                    InventoryManager.Instance.ClearSelectInventoryItem(InventoryLocation.Player);
                }
            }
        }
    }
    public void SetHighlightOnInventorySlots()
    {
        if (inventorySlots.Length > 0)
        {
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                SetHighlightOnInventorySlots(i);
            }
        }
    }
    public void SetHighlightOnInventorySlots(int itemPositon)
    {
        if(inventorySlots.Length > 0&& inventorySlots[itemPositon].itemDetails != null)
        {
            if (inventorySlots[itemPositon].isSelected)
            {
                inventorySlots[itemPositon].highlight.gameObject.SetActive(true);
                InventoryManager.Instance.SetSelectInventoryItem(InventoryLocation.Player, inventorySlots[itemPositon].itemDetails.itemID);
            }
        }
    }

    public void DestoryCurrentDraggedItem()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            if(inventorySlots[i].draggedItem != null)
            {
                Destroy(inventorySlots[i].draggedItem);
            }
        }
    }
    public void ClearCurrentSelectedItem()
    {
        for(int i = 0; i < inventorySlots.Length; i++)
        {
            inventorySlots[i].ClearSelectedItem();
        }
    }
}
