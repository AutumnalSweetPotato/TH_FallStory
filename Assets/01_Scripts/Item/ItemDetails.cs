
using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;
    public ItemType itemType;
    public string itemDescription;
    public Sprite itemSprite;
    public string itemLongDescription;
    public short itemUseGridRadius;
    public float itemUseRadius;
    public bool isStartingItem;
    public bool canBePickUp;
    public bool canBeDropped;
    public bool canBeEatten;
    public bool canBeCarried;
    public CollsionBoxBound itemCollsionBoxBound;

}
