
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(ItemDescriptionAttribute))]
public class ItemDescriptionDrawsr : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property) * 2;
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        if (property.propertyType == SerializedPropertyType.Integer)
        {
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.IntField
                (new Rect(position.x,position.y,position.width,position.height/2),label,property.intValue);

            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "ŒÔ∆∑√Ë ˆ", GetItemDescription(property.intValue));


            if (EditorGUI.EndChangeCheck())
            {
                property.intValue = newValue;
            }
        }

        EditorGUI.EndProperty();
    }

    private string GetItemDescription(int id)
    {
        SO_ItemList so_ItemList;
        so_ItemList = AssetDatabase.LoadAssetAtPath("Assets/03_GameData/Item/ItemList.asset",typeof(SO_ItemList)) 
            as SO_ItemList;
        List<ItemDetails> list = so_ItemList.ItemDetailsList;
        ItemDetails itemDetails = list.Find(x => x.itemID == id);
        if (itemDetails != null)
            return itemDetails.itemDescription;
        else
            return "";
    }
}
