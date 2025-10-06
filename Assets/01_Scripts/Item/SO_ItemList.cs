
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemList",menuName ="ScriptableObject/Item/ItemList")]
public class SO_ItemList : ScriptableObject
{
    [SerializeField]
    public List<ItemDetails> ItemDetailsList;
}
