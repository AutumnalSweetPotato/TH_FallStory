
using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Single<InventoryManager>,ISaveable
{
    private UIInventoryBar inventoryBar;
    private Dictionary<int, ItemDetails> itemDetailsDictionary;
    private int[] selectInventoryItem;
    [SerializeField] private SO_ItemList itemList;
    
    public List<InventoryItem>[] inventoryLists; // [0] = player inventory, [1] = chest inventory
    [HideInInspector] public int[] inventoryListCapacityIntArray; // [0] = player inventory, [1] = chest inventory

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get => _iSaveableUniqueID; set => _iSaveableUniqueID = value; }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get => _gameObjectSave; set => _gameObjectSave = value; }


    private void OnEnable()
    {
        ISaveableRegister();
    }
    private void OnDisable()
    {
        ISaveableDeregister();
    }

    protected override void Awake()
    {
        base.Awake();
        CreateItemDetailsDictionary();
        CreateInventoryLists();

        //��ʼ��ѡ����Ʒ
        selectInventoryItem = new int[((int)InventoryLocation.Count)];
        for(int i = 0; i < selectInventoryItem.Length; i++)
        {
            selectInventoryItem[i] = -1;
        }

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;

        GameObjectSave = new GameObjectSave();
    }
    private void Start()
    {
        inventoryBar = FindObjectOfType<UIInventoryBar>();
    }
    /// <summary>
    /// �����Ʒ
    /// </summary>
    /// <param name="location"></param>
    /// <param name="item"></param>
    public void AddItem(InventoryLocation location, Item item)
    {
        int id = item.ItemID;
        List<InventoryItem> inventorList = inventoryLists[((int)InventoryLocation.Player)];
        int itemPosIndex = FindItemInInventory(location, id);

        if (itemPosIndex != -1)
        {
            AddItemPosition(inventorList, id, itemPosIndex);
        }
        else
        {
            AddItemPosition(inventorList, id);
        }
        EventHandler.CallInventoryUpdateEvent(location, inventoryLists[((int)InventoryLocation.Player)]);
    }
    public void AddItem(InventoryLocation location, Item item, GameObject gameObject)
    {
        AddItem(location, item);
        Destroy(gameObject);
    }
    public void AddItem(InventoryLocation location, int itemID)
    {
        
        List<InventoryItem> inventorList = inventoryLists[((int)InventoryLocation.Player)];
        int itemPosIndex = FindItemInInventory(location, itemID);

        if (itemPosIndex != -1)
        {
            AddItemPosition(inventorList, itemID, itemPosIndex);
        }
        else
        {
            AddItemPosition(inventorList, itemID);
        }
        EventHandler.CallInventoryUpdateEvent(location, inventoryLists[((int)InventoryLocation.Player)]);
    }

    /// <summary>
    /// �����Ʒ��ָ��λ��
    /// </summary>
    /// <param name="inventorList"></param>
    /// <param name="id"></param>
    private void AddItemPosition(List<InventoryItem> inventorList, int id)
    {
        InventoryItem inventoryItem = new InventoryItem();
        inventoryItem.itemID = id;
        inventoryItem.itemQuantity = 1;
        inventorList.Add(inventoryItem);

        //DebugPrintInventoryList(inventorList);
    }



    private void AddItemPosition(List<InventoryItem> inventorList, int id, int itemPosIndex)
    {
        InventoryItem inventoryItem = new InventoryItem();
        int quantity = inventorList[itemPosIndex].itemQuantity;
        inventoryItem.itemID = id;
        inventoryItem.itemQuantity = quantity + 1;
        inventorList[itemPosIndex] = inventoryItem;

        //DebugPrintInventoryList(inventorList);
    }

    /// <summary>
    /// �ڿ���в�����Ʒ
    /// </summary>
    /// <param name="location"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public int FindItemInInventory(InventoryLocation location, int id)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)location];
        for (int i = 0; i < inventoryList.Count; i++)
        {
            if (inventoryList[i].itemID == id)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// ��������б�
    /// </summary>
    private void CreateInventoryLists()
    {
        inventoryLists = new List<InventoryItem>[((int)InventoryLocation.Count)];
        for (int i = 0; i < (int)InventoryLocation.Count; i++)
        {
            inventoryLists[i] = new List<InventoryItem>();
        }
        inventoryListCapacityIntArray = new int[((int)InventoryLocation.Count)];
        inventoryListCapacityIntArray[((int)InventoryLocation.Player)] = Settings.playerInitialInventoryCapacity;
    }

    /// <summary>
    /// ������Ʒ�����ֵ�
    /// </summary>
    private void CreateItemDetailsDictionary()
    {
        itemDetailsDictionary = new Dictionary<int, ItemDetails>();
        foreach (ItemDetails itemDetails in itemList.ItemDetailsList)
        {
            itemDetailsDictionary.Add(itemDetails.itemID, itemDetails);
        }
    }
    public ItemDetails GetItemDetailsByID(int itemID)
    {
        ItemDetails itemDetails;
        if (itemDetailsDictionary.TryGetValue(itemID, out itemDetails)) return itemDetails;
        else return null;
    }
    /// <summary>
    /// �Ƴ���Ʒ
    /// </summary>
    /// <param name="location"></param>
    /// <param name="itemID"></param>
    public void RemoveItem(InventoryLocation location, int itemID)
    {
        List<InventoryItem> inventoryList = inventoryLists[(int)location];
        int itemPosIndex = FindItemInInventory(location, itemID);
        if (itemPosIndex != -1)
        {
            RemoveItemAtPosition(inventoryList, itemID, itemPosIndex);
        }
        EventHandler.CallInventoryUpdateEvent(location, inventoryList);
    }
    /// <summary>
    /// �Ƴ�ָ��λ�õ�Item
    /// </summary>
    /// <param name="inventoryList"></param>
    /// <param name="itemID"></param>
    /// <param name="itemPosIndex"></param>
    private void RemoveItemAtPosition(List<InventoryItem> inventoryList, int itemID, int itemPosIndex)
    {
        InventoryItem inventoryItem = new InventoryItem();
        int quantity = inventoryList[itemPosIndex].itemQuantity - 1;
        if (quantity > 0)
        {
            inventoryItem.itemQuantity = quantity;
            inventoryItem.itemID = itemID;
            inventoryList[itemPosIndex] = inventoryItem;
        }
        else
        {
            inventoryList.RemoveAt(itemPosIndex);
        }
    }
    /// <summary>
    /// ������Ʒλ��
    /// </summary>
    /// <param name="location"></param>
    /// <param name="fromNum"></param>
    /// <param name="toNum"></param>
    public void SwepInventoryItem(InventoryLocation location, int fromNum, int toNum)
    {
        if (fromNum < inventoryLists[((int)location)].Count && toNum < inventoryLists[((int)location)].Count
            && fromNum != toNum && fromNum >= 0 && toNum >= 0)
        {
            InventoryItem formInventoryItem = inventoryLists[((int)location)][fromNum];
            InventoryItem toInventoryItem = inventoryLists[((int)location)][toNum];
            inventoryLists[((int)location)][fromNum] = toInventoryItem;
            inventoryLists[((int)location)][toNum] = formInventoryItem;
            EventHandler.CallInventoryUpdateEvent(location, inventoryLists[((int)location)]);

        }
    }
    /// <summary>
    /// ��ȡ��Ʒ��������
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    public string GetItemTypeDescription(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Seed:
                return "����";
            case ItemType.Commodity:
                return "��Ʒ";
            case ItemType.Watering_tool:
                return "��ˮ����";
            case ItemType.Hoeing_tool:
                return "��ͷ";
            case ItemType.Chopping_tool:
                return "��ͷ";
            case ItemType.Breaking_tool:
                return "����";
            case ItemType.Reaping_tool:
                return "����";
            case ItemType.Collection_tool:
                return "�ɼ�����";
            case ItemType.Furniture:
                return "�Ҿ�";
        }
        return "";
    }

    /// <summary>
    /// ����ѡ����Ʒ
    /// </summary>
    /// <param name="location"></param>
    /// <param name="itemID"></param>
    public void SetSelectInventoryItem(InventoryLocation location,int itemID)
    {
        selectInventoryItem[(int)location] = itemID;
    }

    /// <summary>
    /// ���ѡ����Ʒ
    /// </summary>
    /// <param name="location"></param>
    public void ClearSelectInventoryItem(InventoryLocation location)
    {
        selectInventoryItem[(int)location] = -1;
    }


    /// <summary>
    /// ��ȡѡ����Ʒ��ID
    /// </summary>
    /// <param name="location"></param>
    /// <returns></returns>
    private int GetSelectedInventoryItem(InventoryLocation location)
    {
        return selectInventoryItem[(int)location];
    }
    
    /// <summary>
    /// ��ȡѡ����Ʒ������
    /// </summary>
    /// <param name="location">��Ʒ����λ��</param>
    /// <returns></returns>
    public ItemDetails GetSelectedInventoryItemDetails(InventoryLocation location)
    {
        int itemID = GetSelectedInventoryItem(location);
        if (itemID != -1)
        {
            return GetItemDetailsByID(itemID);
        }
        return null;
    }

    #region ����
    public void ISaveableRegister()
    { 
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }
    public void ISaveableStoreScene(string sceneName)
    {

    }
    public void ISaveableRestoreScene(string sceneName)
    {

    }
    public void ISaveableLoad(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;
            if (gameObjectSave.sceneData.TryGetValue("Main", out SceneSave sceneSave))
            {
                if (sceneSave.listInventoryItemArray != null)
                {
                    inventoryLists = sceneSave.listInventoryItemArray;
                    for (int i = 0; i < (int)InventoryLocation.Count; i++)
                    {
                        EventHandler.CallInventoryUpdateEvent((InventoryLocation)i, inventoryLists[i]);
                    }
                    Player.Instance.ClearCarriedItem();
                    inventoryBar.ClearHighlightOnInventorySlots();
                }
            }
            if (sceneSave.intArrayDictionary != null && sceneSave.intArrayDictionary.TryGetValue("inventoryListCapacityIntArray", out int[] inventoryListCapacityIntArray))
            {
                this.inventoryListCapacityIntArray = inventoryListCapacityIntArray;
            }
        }
    }
    public GameObjectSave ISaveableSave()
    {
        SceneSave sceneSave = new SceneSave();
        GameObjectSave.sceneData.Remove("Main");
        sceneSave.listInventoryItemArray = inventoryLists;
        sceneSave.intArrayDictionary = new Dictionary<string, int[]>();
        sceneSave.intArrayDictionary.Add("inventoryListCapacityIntArray", inventoryListCapacityIntArray);
        GameObjectSave.sceneData.Add("Main", sceneSave);
        return GameObjectSave;
        
    }
    #endregion


    private void DebugPrintInventoryList(List<InventoryItem> inventorList)
    {
        foreach (InventoryItem item in inventorList)
        {
            Debug.Log("��Ʒ������" + GetItemDetailsByID(item.itemID).itemDescription + "     ��Ʒ������" + item.itemQuantity);
        }
        
    }

    
}
