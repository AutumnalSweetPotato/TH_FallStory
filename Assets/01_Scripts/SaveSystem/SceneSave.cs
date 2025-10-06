
using System.Collections.Generic;

[System.Serializable]
public class SceneSave
{
    public List<SceneItem> listSceneItem;           //场景物品
    public Dictionary<string,bool> boolDictionary;          //Flags
    public Dictionary<string,GridPropertyDetails> dictGridPropertyDetails; //保存地图属性
    public Dictionary<string,string> stringDictionary;
    public Dictionary<string,Vector3Serializable> vector3Dictionary;
    public Dictionary<string,int> intDictionary;
    public Dictionary<string, int[]> intArrayDictionary;    //库存字典
    public List<InventoryItem>[] listInventoryItemArray;    //背包列表

}

