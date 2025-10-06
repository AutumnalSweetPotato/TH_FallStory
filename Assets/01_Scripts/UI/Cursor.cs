using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    private Canvas canvas;
    private Camera mainCamera;
    [SerializeField] private Image cursorImage;
    [SerializeField] private RectTransform cursorRectTransform; //光标矩形
    [SerializeField] private Sprite greenCursorSprite;
    [SerializeField] private Sprite transparentCursorSprite;
    [SerializeField] private GridCursor gridCursor;

    private bool _cursorIsEnabled;                              //光标是否启用
    private bool _cursorPositionIsValid;                        //光标位置是否有效
    private ItemType _selectedItemType;                         //当前选中物品类型
    private float _itemUseRadius;                               //当前物品使用半径
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }
    public ItemType SelectedItemType { get => _selectedItemType; set => _selectedItemType = value; }
    public float ItemUseRadius { get => _itemUseRadius; set => _itemUseRadius = value; }

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }
    private void Update()
    {
        if (CursorIsEnabled)
        {
            DisplayCursro();
        }
    }

    /// <summary>
    /// 显示光标
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void DisplayCursro()
    {
        Vector3 currentWorldPosition = GetWorldPositonForCursor();
        SetCursorValidity(currentWorldPosition, Player.Instance.GetPlayerCenterPosition());
        cursorRectTransform.position = GetRectTransformPositionForCursor();
    }



    private void SetCursorValidity(Vector3 currentWorldPosition, Vector3 playerCenterPosition)
    {
        SetCursorToValid();
        //检查使用半径
        if (
            currentWorldPosition.x > (playerCenterPosition.x + ItemUseRadius / 2f) && currentWorldPosition.y > (playerCenterPosition.y + ItemUseRadius / 2f)
            ||
            currentWorldPosition.x < (playerCenterPosition.x - ItemUseRadius / 2f) && currentWorldPosition.y > (playerCenterPosition.y + ItemUseRadius / 2f)
            ||
            currentWorldPosition.x < (playerCenterPosition.x - ItemUseRadius / 2f) && currentWorldPosition.y < (playerCenterPosition.y - ItemUseRadius / 2f)
            ||
            currentWorldPosition.x > (playerCenterPosition.x + ItemUseRadius / 2f) && currentWorldPosition.y < (playerCenterPosition.y - ItemUseRadius / 2f)
            )
        {
            SetCursorToInvalid();
            return;
        }
        if(Mathf.Abs(currentWorldPosition.x - playerCenterPosition.x) > ItemUseRadius
            || Mathf.Abs(currentWorldPosition.y - playerCenterPosition.y) > ItemUseRadius)
        {
            SetCursorToInvalid();
            return;
        }

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.Player);
        if(itemDetails == null)
        {
            SetCursorToInvalid();
            return;
        }

        switch(itemDetails.itemType)
        {
            case ItemType.Watering_tool:
            case ItemType.Hoeing_tool:
            case ItemType.Breaking_tool:
            case ItemType.Chopping_tool:
            case ItemType.Reaping_tool:
            case ItemType.Collection_tool:
                if(!SetCursorValidityTool(currentWorldPosition,playerCenterPosition,itemDetails))
                {
                    SetCursorToInvalid();
                    return;
                }
                break;
        }
    }

    private bool SetCursorValidityTool(Vector3 currentWorldPosition, Vector3 playerCenterPosition, ItemDetails itemDetails)
    {
        switch(itemDetails.itemType)
        {
            case ItemType.Reaping_tool:
                return SetCursorValidyReapingTool(currentWorldPosition, playerCenterPosition, itemDetails);
                default:
                    return false;
        }
    }

    private bool SetCursorValidyReapingTool(Vector3 currentWorldPosition, Vector3 playerCenterPosition, ItemDetails itemDetails)
    {
        List<Item> itemList = new List<Item>();
        if(HelperMethods.GetCompomemtsAtCursorLocation<Item>(out itemList, currentWorldPosition))
        {
            if(itemList.Count != 0)
            {
                foreach(Item item in itemList)
                {
                    if(InventoryManager.Instance.GetItemDetailsByID(item.ItemID).itemType == ItemType.Reapable_scenary)
                        return true;
                }
            }
        }
        return false;
    }

    private void SetCursorToInvalid()
    {
        CursorPositionIsValid = false;
        cursorImage.sprite = transparentCursorSprite;
        gridCursor.EnableCursor(); 
    }

    private void SetCursorToValid()
    {
        CursorPositionIsValid = true;
        cursorImage.sprite = greenCursorSprite;
        gridCursor.DisableCursor();
    }
    public void DisableCursor()
    {
        cursorImage.color = new Color(1f,1f,1f,0f);
        CursorIsEnabled = false;
        
    }
    public void EnableCursor()
    {
        cursorImage.color = new Color(1f,1f,1f,1f);
        CursorIsEnabled = true;
    }
    private Vector3 GetRectTransformPositionForCursor()
    {
        Vector2 screenPosition = new Vector2(Input.mousePosition.x ,Input.mousePosition.y );
        return RectTransformUtility.PixelAdjustPoint(screenPosition, cursorRectTransform, canvas);
    }
    public Vector3 GetWorldPositonForCursor()
    {
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
