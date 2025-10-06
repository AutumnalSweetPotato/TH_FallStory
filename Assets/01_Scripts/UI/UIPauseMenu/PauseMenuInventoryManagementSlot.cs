using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenuInventoryManagementSlot : MonoBehaviour,IBeginDragHandler, IDragHandler, IEndDragHandler,IPointerEnterHandler, IPointerExitHandler
{
    public Image inventoryManagementSlotImage;
    public TextMeshProUGUI textMeshProUGUI;
    public Image greyedOutImageGO;

    [SerializeField] private PauseMenuInventoryManagement inventoryManagement = null;
    [SerializeField] private GameObject inventoryTextBoxPrefab = null;
    [HideInInspector] public ItemDetails itemDetails = null;
    [HideInInspector] public int itemQuintity;
    private int slotNumber;

    public GameObject draggedItem;
    private Canvas parentCanvas;

    private void Awake()
    {
        parentCanvas = GetComponentInParent<Canvas>();

        string name = gameObject.name;
        //获取name最后一个字符
        char lastTwoChar = name[name.Length - 2];
        //判断lastTwoChar是否为数字
        if (!char.IsDigit(lastTwoChar))
        {
            char lastChar = name[name.Length - 1];
            //将lastChar转换为int并赋值给slotNumber
            slotNumber = int.Parse(lastChar.ToString());
        }
        else
        {
            char lastChar = name[name.Length - 1];
            slotNumber = int.Parse(lastTwoChar.ToString() + lastChar.ToString());
        }
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(itemQuintity != 0)
        {
            draggedItem = Instantiate(inventoryManagement.inventoryManagementDraggedItemPrefab, inventoryManagement.transform);
            Image draggedItemImage = draggedItem.GetComponentInChildren<Image>();
            draggedItemImage.sprite = inventoryManagementSlotImage.sprite;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(draggedItem != null)
        {
            draggedItem.transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(draggedItem != null)
        {
            Destroy(draggedItem);
            if(eventData.pointerCurrentRaycast.gameObject != null &&
               eventData.pointerCurrentRaycast.gameObject.GetComponent<PauseMenuInventoryManagementSlot>() != null)
            {
                int toSlotNumber = eventData.pointerCurrentRaycast.gameObject.GetComponent<PauseMenuInventoryManagementSlot>().slotNumber;
                InventoryManager.Instance.SwepInventoryItem(InventoryLocation.Player, slotNumber, toSlotNumber);
                inventoryManagement.DestroyInventoryTextBoxGameobject();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemQuintity != 0)
        {
            inventoryManagement.inventoryTextBoxGameObject = Instantiate(inventoryTextBoxPrefab,
                transform.position,
                Quaternion.identity);
            inventoryManagement.inventoryTextBoxGameObject.transform.SetParent(parentCanvas.transform, false);
            UIInventoryTextBox inventoryTextbox = inventoryManagement.inventoryTextBoxGameObject.GetComponent<UIInventoryTextBox>();

            string itemTypeDescription = InventoryManager.Instance.GetItemTypeDescription(itemDetails.itemType);
            inventoryTextbox.SetTextboxText(itemDetails.itemDescription, itemTypeDescription, "", itemDetails.itemLongDescription, "", "");
            if (slotNumber>23)
            {
                inventoryManagement.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0f);
                inventoryManagement.inventoryTextBoxGameObject.transform.position = new UnityEngine.Vector3(transform.position.x,
                    transform.position.y + 50f, transform.position.z);
            }
            else
            {
                inventoryManagement.inventoryTextBoxGameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
                inventoryManagement.inventoryTextBoxGameObject.transform.position = new UnityEngine.Vector3(transform.position.x,
                    transform.position.y - 50f, transform.position.z);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryManagement.DestroyInventoryTextBoxGameobject();
    }
}
