
using System.Collections.Generic;

[System.Serializable]
public class SceneSave
{
    public List<SceneItem> listSceneItem;           //������Ʒ
    public Dictionary<string,bool> boolDictionary;          //Flags
    public Dictionary<string,GridPropertyDetails> dictGridPropertyDetails; //�����ͼ����
    public Dictionary<string,string> stringDictionary;
    public Dictionary<string,Vector3Serializable> vector3Dictionary;
    public Dictionary<string,int> intDictionary;
    public Dictionary<string, int[]> intArrayDictionary;    //����ֵ�
    public List<InventoryItem>[] listInventoryItemArray;    //�����б�

}

