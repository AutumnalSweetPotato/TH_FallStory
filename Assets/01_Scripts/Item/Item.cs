using System;
using UnityEngine;
using static UnityEditor.Progress;

public class Item : MonoBehaviour
{
    
    [ItemDescription]
    [SerializeField] private int id;
    private SpriteRenderer sr;
    
    private void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if (id != 0)
            Init(id);
    }
     
    public void Init(int id)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetailsByID(id);
        sr.sprite = itemDetails.itemSprite;
        this.id = id;
        if(itemDetails.itemType == ItemType.Reapable_scenary)
        {
            gameObject.AddComponent<ItemNudge>();
        }
        if(itemDetails.canBeDropped)
        {
            BoxCollider2D bc = gameObject.GetComponent<BoxCollider2D>();
            CollsionBoxBound collsionBoxBound = itemDetails.itemCollsionBoxBound;
            bc.size = collsionBoxBound.size;
            bc.offset = collsionBoxBound.offset;
        }
        

    }

    public int ItemID
    {
        get { return id; }
        set { id = value; }
    }
}
