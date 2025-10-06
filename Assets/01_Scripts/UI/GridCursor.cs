
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;

public class GridCursor : MonoBehaviour
{
    private Canvas canvas;
    private Grid grid;
    private Camera mainCamera;
    [SerializeField] private Image cursorImage = null;
    [SerializeField] private RectTransform cursorRectTransform = null;
    [SerializeField] private Sprite greenCursorSprite = null;
    [SerializeField] private Sprite redcursorSprite = null;
    [SerializeField] private SO_CropDetailsList so_cropDetailsList = null;

    private bool _cursorPositionIsValid = false;
    public bool CursorPositionIsValid { get => _cursorPositionIsValid; set => _cursorPositionIsValid = value; }

    private int _itemUseRadius = 0;
    public int ItemUseRadius { get => _itemUseRadius; set => _itemUseRadius = value; }

    private ItemType _itemType;
    public ItemType ItemType { get => _itemType; set => _itemType = value; }

    private bool _cursorIsEnabled = false;
    public bool CursorIsEnabled { get => _cursorIsEnabled; set => _cursorIsEnabled = value; }

    private void OnEnable()
    {
        EventHandler.SceneLoadAfterEvent += SceneLoaded;
    }

    private void OnDisable()
    {
        EventHandler.SceneLoadAfterEvent -= SceneLoaded;
    }

    private void Start()
    {
        mainCamera = Camera.main;
        canvas = GetComponentInParent<Canvas>();
    }
    private void Update()
    {
        if (CursorIsEnabled)
        {
            DisplayCursor();
        }


    }

    public void DisableCursor()
    {

        cursorImage.color = Color.clear;
        CursorIsEnabled = false;

    }
    public void EnableCursor()
    {
        cursorImage.color = new Color(1, 1, 1, 1);
        CursorIsEnabled = true;
    }

    public Vector3 GetRectTransformPositionForCursor(Vector3Int gridPosition)
    {
        Vector3 gridWorldPosition = grid.CellToWorld(gridPosition);
        Vector2 gridScreenPosition = mainCamera.WorldToScreenPoint(gridWorldPosition);
        return RectTransformUtility.PixelAdjustPoint(gridScreenPosition, cursorRectTransform, canvas);
    }

    public Vector3Int GetGridPositionForPlayer()
    {
        return grid.WorldToCell(Player.Instance.transform.position);
    }

    public Vector3Int GetGridPositionForCursor()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCamera.transform.position.z));
        return grid.WorldToCell(worldPosition);
    }

    private Vector3Int DisplayCursor()
    {
        if (grid != null)
        {
            Vector3Int gridPosition = GetGridPositionForCursor();
            Vector3Int playerGridPosition = GetGridPositionForPlayer();
            SetCursorValidity(gridPosition, playerGridPosition);
            cursorRectTransform.position = GetRectTransformPositionForCursor(gridPosition);
            return gridPosition;
        }
        else
        {
            return Vector3Int.zero;
        }
    }

    private void SetCursorValidity(Vector3Int gridPosition, Vector3Int playerGridPosition)
    {
        SetCursorToValid();

        if (Mathf.Abs(gridPosition.x - playerGridPosition.x) > ItemUseRadius ||
            Mathf.Abs(gridPosition.y - playerGridPosition.y) > ItemUseRadius)
        {
            SetCursorToInvalid();
            //Debug.Log(1);
            return;
        }

        //获取选择物品详情
        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.Player);
        if (itemDetails == null)
        {
            SetCursorToInvalid();
            //Debug.Log(2);
            return;
        }


        //获取网格属性
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(gridPosition.x, gridPosition.y);
        if (gridPropertyDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (!IsCursoValidForSeed(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;
                case ItemType.Commodity:
                    if (!IsCursoValidForCommodity(gridPropertyDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;
                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Breaking_tool:
                case ItemType.Chopping_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collection_tool:
                    if (!IsCursoValidForTool(gridPropertyDetails, itemDetails))
                    {
                        SetCursorToInvalid();
                        return;
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            SetCursorToInvalid();

            return;
        }



    }

    private bool IsCursoValidForTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if (gridPropertyDetails.isDiggable && gridPropertyDetails.daysSinceDug == -1)
                {
                    //获取网格左下角的世界坐标
                    Vector3 cursorWorldPosition = new Vector3(GetWorldPositionForCursor().x + 0.5f,
                        GetWorldPositionForCursor().y + 0.5f, 0f);

                    List<Item> itemList = new List<Item>();
                    //检查格子中是否有Item
                    HelperMethods.GetComponentsAtBoxLocation<Item>(out itemList, cursorWorldPosition,
                        Settings.cursorSize, 0f);

                    bool foundReapable = false;
                    //检查格子中是否有可收获的物品
                    foreach (var item in itemList)
                    {
                        if (InventoryManager.Instance.GetItemDetailsByID(item.ItemID).itemType == ItemType.Reapable_scenary)
                        {
                            foundReapable = true;
                            break;
                        }
                    }
                    if (foundReapable)
                        return false;
                    else
                        return true;

                }
                else
                {
                    return false;
                }
            case ItemType.Watering_tool:
                if (gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.daysSinceWatered == -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case ItemType.Chopping_tool:
            case ItemType.Collection_tool:
            case ItemType.Breaking_tool:
                if(gridPropertyDetails.seedItemID != -1)
                {
                    CropDetails cropDetails = so_cropDetailsList.GetCropDetails(gridPropertyDetails.seedItemID);
                    if(cropDetails != null)
                    {
                        if(gridPropertyDetails.growthDays >= cropDetails.growthDays[cropDetails.growthDays.Length - 1])
                        {
                            if(cropDetails.CanUseToolHarvestCrop(itemDetails.itemID)) 
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            
            default:
                return false;
        }
    }



    private bool IsCursoValidForSeed(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem;
    }

    private bool IsCursoValidForCommodity(GridPropertyDetails gridPropertyDetails)
    {
        return gridPropertyDetails.canDropItem && gridPropertyDetails.seedItemID == -1;
    }

    private void SetCursorToInvalid()
    {
        cursorImage.sprite = redcursorSprite;
        CursorPositionIsValid = false;
    }

    private void SetCursorToValid()
    {
        cursorImage.sprite = greenCursorSprite;
        CursorPositionIsValid = true;
    }

    private void SceneLoaded()
    {
        grid = FindObjectOfType<Grid>();
    }

    private Vector3 GetWorldPositionForCursor()
    {
        return grid.CellToWorld(GetGridPositionForCursor());
    }
}
