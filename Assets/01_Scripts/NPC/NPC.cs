using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(NPCMovement))]
[RequireComponent(typeof(GenerateGUID))]
public class NPC : MonoBehaviour,ISaveable
{
    private NPCMovement npcMovement;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get => _iSaveableUniqueID; set => _iSaveableUniqueID = value; }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get => _gameObjectSave; set => _gameObjectSave = value; }

    private void OnEnable()
    {
        ISaveableRegister();
    }

    

    /// Unity Message | 0 references
    private void OnDisable()
    {
        ISaveableDeregister();
    }

    

    /// Unity Message | 0 references
    private void Awake()
    {
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }

    /// Unity Message | 0 references
    private void Start()
    {
        // / get npc movement component
        npcMovement = GetComponent<NPCMovement>();
    }

    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }
    public void ISaveableStoreScene(string sceneName)
    {

    }
    public void ISaveableRestoreScene(string sceneName)
    {

    }
    public void ISaveableLoad(GameSave gameSave)
    {
        // Get game object save
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;
            // Get scene save
            if (GameObjectSave.sceneData.TryGetValue("Main", out SceneSave sceneSave))
            {
                // if dictionaries are not null
                if (sceneSave.vector3Dictionary != null && sceneSave.stringDictionary != null)
                {
                    // target grid position
                    if (sceneSave.vector3Dictionary.TryGetValue("npcTargetGridPosition", out Vector3Serializable savedNPCTargetGridPosition))
                    {
                        npcMovement.npcTargetGridPosition = new Vector3Int((int)savedNPCTargetGridPosition.x, (int)savedNPCTargetGridPosition.y, (int)savedNPCTargetGridPosition.z);
                        npcMovement.npcCurrentGridPosition = npcMovement.npcTargetGridPosition;
                    }
                    // target world position
                    if (sceneSave.vector3Dictionary.TryGetValue("npcTargetWorldPosition", out Vector3Serializable savedNPCTargetWorldPosition))
                    {
                        npcMovement.npcTargetWorldPosition = new Vector3(savedNPCTargetWorldPosition.x, savedNPCTargetWorldPosition.y, savedNPCTargetWorldPosition.z);
                        transform.position = npcMovement.npcTargetWorldPosition;
                    }
                    // target scene
                    if (sceneSave.stringDictionary.TryGetValue("npcTargetScene", out string savedTargetScene))
                    {
                        if (Enum.TryParse<SceneName>(savedTargetScene, out SceneName sceneName))
                        {
                            npcMovement.npcTargetScene = sceneName;
                            npcMovement.npcCurrentScene = npcMovement.npcTargetScene;
                        }
                    }
                    // Clear any current NPC movement
                    npcMovement.CancelNPCMovement();
                }
            }
        }

    }



    public GameObjectSave ISaveableSave()
    {

        // Remove current scene save
        GameObjectSave.sceneData.Remove("Main");
        // Create scene save
        SceneSave sceneSave = new SceneSave();
        // Create vector 3 serialisable dictionary
        sceneSave.vector3Dictionary = new Dictionary<string, Vector3Serializable>();
        // Create string dictionary
        sceneSave.stringDictionary = new Dictionary<string, string>();
        // Store target grid position, target world position, and target scene
        sceneSave.vector3Dictionary.Add("npcTargetGridPosition", new Vector3Serializable(npcMovement.npcTargetGridPosition.x, npcMovement.npcTargetGridPosition.y, npcMovement.npcTargetGridPosition.z));
        sceneSave.vector3Dictionary.Add("npcTargetWorldPosition", new Vector3Serializable(npcMovement.npcTargetWorldPosition.x, npcMovement.npcTargetWorldPosition.y, npcMovement.npcTargetWorldPosition.z));
        sceneSave.stringDictionary.Add("npcTargetScene", npcMovement.npcTargetScene.ToString());
        // Add scene save to game object
        GameObjectSave.sceneData.Add("Main", sceneSave);
        return GameObjectSave;

    }
}
