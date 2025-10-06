
[System.Serializable]
public class SceneItem 
{
    public int itemID;
    public Vector3Serializable position;
    public string itemName;

    public SceneItem()
    {
        position = new Vector3Serializable();
    }
}
