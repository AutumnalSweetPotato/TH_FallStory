using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class UIInventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image image;
    public Image highlight;
    public Text textCount;
    private Camera mainCamera;
    private Transform parentThansform;
    public GameObject draggedItem;
    private int slotNumber = 0;
    private Canvas parentCanvas;    //描述信息父物体的画布
    private GridCursor gridCursor;
    private Cursor cursor;
    [HideInInspector] public ItemDetails itemDetails;
    [HideInInspector] public int itemQuanetity;
    [HideInInspector] public bool isSelected;
    [SerializeField] private UIInventoryBar inventoryBar;
    [SerializeField] private GameObject itemPrefab;         //丢弃后地图上生成的物体
    [SerializeField] private GameObject inventoryTextboxPerfab; //描述信息



    private void Start()
    {
        mainCamera = Camera.main;
        InitializedSlotNumber();
        parentCanvas = GetComponentInParent<Canvas>();
        gridCursor = FindObjectOfType<GridCursor>();
        cursor = FindObjectOfType<Cursor>();
    }
    private void OnEnable()
    {
        EventHandler.DropSelectedItemEvent += DropSelectedItemAtMousePosition;
        EventHandler.RemoveSelectealtemFromlnventoryEvent += RemoveSelectedItemFromInventory;
    }
    private void OnDisable()
    {
        EventHandler.DropSelectedItemEvent -= DropSelectedItemAtMousePosition;
        EventHandler.RemoveSelectealtemFromlnventoryEvent -= RemoveSelectedItemFromInventory;
    }

    private void RemoveSelectedItemFromInventory()
    {
        if(itemDetails != null && isSelected)
        {
            int itemID = itemDetails.itemID;
            InventoryManager.Instance.RemoveItem(InventoryLocation.Player,itemID);
            if(InventoryManager.Instance.FindItemInInventory(InventoryLocation.Player,itemID) == -1)
            {
                ClearSelectedItem();
            }
        }
    }

    private void ClearCursor()
    {
        gridCursor.DisableCursor();
        cursor.DisableCursor();

        gridCursor.ItemType = ItemType.None;
        cursor.SelectedItemType = ItemType.None;
    }
    private void InitializedSlotNumber()
    {
        string name = gameObject.name;
        //获取name最后一个字符
        char lastChar = name[name.Length - 1];
        //将lastChar转换为int并赋值给slotNumber
        slotNumber = int.Parse(lastChar.ToString());

    }
    private void SetSelectedItem()
    {
        inventoryBar.ClearHighlightOnInventorySlots();
        isSelected = true;
        inventoryBar.SetHighlightOnInventorySlots();

        //设置光标的使用半径
        gridCursor.ItemUseRadius = itemDetails.itemUseGridRadius;
        cursor.ItemUseRadius = itemDetails.itemUseRadius;
        if (gridCursor.ItemUseRadius > 0)
        { 
            gridCursor.EnableCursor();
        }
        else
        {
            gridCursor.DisableCursor();
        }
        if (cursor.ItemUseRadius > 0)
        {
            cursor.EnableCursor();
        }
        else
        {
            cursor.DisableCursor();
        }
        gridCursor.ItemType = itemDetails.itemType;
        cursor.SelectedItemType = itemDetails.itemType;

        InventoryManager.Instance.SetSelectInventoryItem(InventoryLocation.Player, itemDetails.itemID);

        if (itemDetails.canBeCarried)
            Player.Instance.ShowCarriedItem(itemDetails.itemID);
        else
            Player.Instance.ClearCarriedItem();
    }

    public void ClearSelectedItem()
    {
        ClearCursor();

        inventoryBar.ClearHighlightOnInventorySlots();
        isSelected = false;
        InventoryManager.Instance.ClearSelectInventoryItem(InventoryLocation.Player);

        Player.Instance.ClearCarriedItem();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (parentThansform == null)
            parentThansform = GameObject.FindGameObjectWithTag(Settings.Tags.ItemsParentTransform).
            GetComponent<Transform>();

        if (itemDetails != null)
        {
            Player.Instance.DisablePlayerInput();

            draggedItem = Instantiate(inventoryBar.inventoryBarDraggedItem, inventoryBar.transform.parent);
            draggedItem.GetComponent<Image>().sprite = image.sprite;

            SetSelectedItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedItem != null)
        {
            Destroy(draggedItem);
            if (eventData.pointerCurrentRaycast.gameObject != null &&
               eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>() != null)
            {
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<UIInventorySlot>().slotNumber;
                InventoryManager.Instance.SwepInventoryItem(InventoryLocation.Player, slotNumber, toSlotNumber);
                DestroyInventoryTextbox();
                ClearSelectedItem();
            }
            else
            {
                if (itemDetails.canBeDropped)
                {
                    DropSelectedItemAtMousePosition();
                }
            }
        }
        Player.Instance.EnablePlayerInput();
    }

    private void DropSelectedItemAtMousePosition()
    {
        if (itemDetails != null && isSelected)
        {

            //判断是否可以放置
            if (gridCursor.CursorPositionIsValid)
            {
                //鼠标位置转换为世界坐标
                Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    -mainCamera.transform.position.z));
                GameObject itemGameObject = Instantiate(itemPrefab, new Vector2(worldPos.x, worldPos.y - Settings.gridCellSize / 2f), Quaternion.identity, parentThansform);

                Item item = itemGameObject.GetComponent<Item>();
                item.ItemID = itemDetails.itemID;
                InventoryManager.Instance.RemoveItem(InventoryLocation.Player, itemDetails.itemID);
                if (InventoryManager.Instance.FindItemInInventory(InventoryLocation.Player, item.ItemID) == -1)
                {
                    ClearSelectedItem();
                }
            }

        }
    }




    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemQuanetity != 0)
        {
            inventoryBar.inventoryTextboxGameObject = Instantiate(inventoryTextboxPerfab,
                transform.position,
                Quaternion.identity);
            inventoryBar.inventoryTextboxGameObject.transform.SetParent(parentCanvas.transform, false);
            UIInventoryTextBox inventoryTextbox = inventoryBar.inventoryTextboxGameObject.GetComponent<UIInventoryTextBox>();

            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);
            inventoryTextbox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");
            if (inventoryBar.IsInventoryBarPositionBottom)
            {
                inventoryBar.inventoryTextboxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryBar.inventoryTextboxGameObject.transform.position = new UnityEngine.Vector3(transform.position.x,
                    transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryBar.inventoryTextboxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryBar.inventoryTextboxGameObject.transform.position = new UnityEngine.Vector3(transform.position.x,
                    transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DestroyInventoryTextbox();
    }
    public void DestroyInventoryTextbox()
    {
        if (inventoryBar.inventoryTextboxGameObject != null)
            Destroy(inventoryBar.inventoryTextboxGameObject);
    }

    /// <summary>
    /// 点击接口实现
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelected == true)
                ClearSelectedItem();
            else
                if (itemQuanetity > 0)
                SetSelectedItem();
        }
    }


}
