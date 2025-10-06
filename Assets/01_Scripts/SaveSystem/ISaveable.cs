 
public interface ISaveable
{
    string ISaveableUniqueID { get; set; }
    GameObjectSave GameObjectSave { get; set; }
    void ISaveableRegister();
    void ISaveableDeregister();
    void ISaveableStoreScene(string sceneName);
    void ISaveableRestoreScene(string sceneName);
    GameObjectSave ISaveableSave(); //保存到文件
    void ISaveableLoad(GameSave gameSave); //从文件加载

}
