using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : Single<SaveLoadManager>
{
    public List<ISaveable> iSaveableObjectList;
    public GameSave gameSave;
    protected override void Awake()
    {
        base.Awake();
        iSaveableObjectList = new List<ISaveable>();
    }
    
    public void LoadDataFromFile()
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        if(File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            gameSave =  new GameSave();
            FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Open);
            gameSave = (GameSave)bf.Deserialize(file);

            for(int i = iSaveableObjectList.Count - 1; i > -1; i--)
            {
                if (gameSave.gameObjectData.ContainsKey(iSaveableObjectList[i].ISaveableUniqueID))
                {
                    iSaveableObjectList[i].ISaveableLoad(gameSave);
                }
                else
                {
                    Component component = (Component)iSaveableObjectList[i];
                    Destroy(component.gameObject);
                }
            }
            file.Close();
        }
        if (UIManager.Instance)
            UIManager.Instance.DisablePauseMenu();
    }

    public void SaveDataToFile()
    {
        
        BinaryFormatter bf = new BinaryFormatter();
        gameSave = new GameSave();
        foreach(ISaveable iSaveableObject in iSaveableObjectList)
        {
            gameSave.gameObjectData.Add(iSaveableObject.ISaveableUniqueID, iSaveableObject.ISaveableSave());
        }
        FileStream file = File.Open(Application.persistentDataPath + "/save.dat", FileMode.Create);
        bf.Serialize(file, gameSave);
        file.Close();
        if (UIManager.Instance)
            UIManager.Instance.DisablePauseMenu();
    }

    public void StoreCurrentSceneData()
    {
        foreach (var iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RestoreCurrentSceneData()
    {
        foreach (var iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}
