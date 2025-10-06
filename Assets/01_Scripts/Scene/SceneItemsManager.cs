using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(GenerateGUID))]
public class SceneItemsManager : Single<SceneItemsManager>, ISaveable
{
    private Transform parentItem;
    [SerializeField]private GameObject itemPrefab = null;

    private string _ISaveableUniqueID; 
    public string ISaveableUniqueID { get => _ISaveableUniqueID; set => _ISaveableUniqueID = value; }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get => _gameObjectSave; set => _gameObjectSave = value; }

    private void AfterSceneLoad()
    {
        parentItem = GameObject.FindGameObjectWithTag(Settings.Tags.ItemsParentTransform).transform;
    }

    protected override void Awake()
    {
        base.Awake();
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.SceneLoadAfterEvent += AfterSceneLoad;
    }
    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.SceneLoadAfterEvent -= AfterSceneLoad;
    }

    public void ISaveableDeregister()
    {
        if(SaveLoadManager.Instance != null)
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableRestoreScene(string sceneName)
    {
        if(GameObjectSave.sceneData.TryGetValue(sceneName,out SceneSave sceneSave))
        {
            if(sceneSave.listSceneItem != null )
            {
                DestroySceneItems();
                InstantiateSceneItems(sceneSave.listSceneItem);
            }
        }
    }

    public void ISaveableLoad(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;
            ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
    public GameObjectSave ISaveableSave()
    {
        ISaveableStoreScene(SceneManager.GetActiveScene().name);
        return GameObjectSave;
    }

    public void ISaveableStoreScene(string sceneName)
    {
        GameObjectSave.sceneData.Remove(sceneName);
        List<SceneItem> sceneItemList = new List<SceneItem>();
        Item[] itemInScene = FindObjectsOfType<Item>();
        foreach (Item item in itemInScene)
        {
            SceneItem sceneItem = new SceneItem();
            sceneItem.itemID = item.ItemID;
            sceneItem.position = new Vector3Serializable(item.transform.position);
            sceneItem.itemName = item.name;
            sceneItemList.Add(sceneItem);
        }
        SceneSave sceneSave = new SceneSave();
        sceneSave.listSceneItem = sceneItemList;
        GameObjectSave.sceneData.Add(sceneName,sceneSave);

    }
    public void InstantiateSceneItems(int itemID,Vector3 itemPosition)
    {
        GameObject itemGameObject = Instantiate(itemPrefab, itemPosition, Quaternion.identity, parentItem);
        Item item = itemGameObject.GetComponent<Item>();
        item.Init(itemID);
        
       
    }

    private void InstantiateSceneItems(List<SceneItem> sceneItemList)
    {
        GameObject itemGameObject;
        foreach (SceneItem sceneItem in sceneItemList)
        {
            itemGameObject = Instantiate(itemPrefab,
                new Vector3(sceneItem.position.x, sceneItem.position.y, sceneItem.position.z),
                Quaternion.identity, parentItem);
            Item item = itemGameObject.GetComponent<Item>();
            item.ItemID = sceneItem.itemID;
            item.name = sceneItem.itemName;
           
        }
    }

    private void DestroySceneItems()
    {
        Item[] itemsInScene = FindObjectsOfType<Item>();
        for (int i = itemsInScene.Length - 1; i > -1; i--)
        {
            Destroy(itemsInScene[i].gameObject);
        }
    }
}
