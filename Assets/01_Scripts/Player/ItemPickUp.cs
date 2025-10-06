using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.GetComponent<Item>();
        if(item != null)
        {
            ItemDetails itemDetails = InventoryManager.Instance.GetItemDetailsByID(item.ItemID);
            if (itemDetails.canBePickUp)
            {
                InventoryManager.Instance.AddItem(InventoryLocation.Player, item, collision.gameObject);
                AudioManager.Instance.PlaySound(SoundName.effectPickupSound);
            }
        }
        
    }
}
