using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AStar))]
public class NPCManager : Single<NPCManager>
{
    [HideInInspector]
    public NPC[] npcArray;
    private AStar aStar;

    [SerializeField] private SO_SceneRouteList so_SceneRouteList = null;
    private Dictionary<string, SceneRoute> sceneRouteDictionary;


    protected override void Awake()
    {
        base.Awake();
        aStar = GetComponent<AStar>();
        npcArray = FindObjectsOfType<NPC>();

        InitiateSceneRouteDictionary();

    }

    private void InitiateSceneRouteDictionary()
    {
        sceneRouteDictionary = new Dictionary<string, SceneRoute>();
        if (so_SceneRouteList.sceneRouteList.Count > 0)
        {
            foreach (SceneRoute sceneRoute in so_SceneRouteList.sceneRouteList)
            {
                if (sceneRouteDictionary.ContainsKey(sceneRoute.fromSceneName.ToString() + sceneRoute.toSceneName.ToString()))
                    continue;
                sceneRouteDictionary.Add(sceneRoute.fromSceneName.ToString() + sceneRoute.toSceneName.ToString(), sceneRoute);
            }
        }
    }

    private void OnEnable()
    {
        EventHandler.SceneLoadAfterEvent += AfterSceneLoad;
    }
    private void OnDisable()
    {
        EventHandler.SceneLoadAfterEvent -= AfterSceneLoad;
    }

    private void AfterSceneLoad()
    {
        SetNPCsActiveStatus();
    }

    private void SetNPCsActiveStatus()
    {
        foreach (NPC npc in npcArray)
        {
            NPCMovement npcMovement = npc.GetComponent<NPCMovement>();
            if (npcMovement.npcCurrentScene.ToString() == SceneManager.GetActiveScene().name)
            {
                npcMovement.SetNPCActiveInScene();
            }
            else
            {
                npcMovement.SetNPCInactiveInScene();
            }
        }
    }
    public SceneRoute GetSceneRoute(string fromSceneName, string toSceneName)
    {
        SceneRoute sceneRoute;

        // Get scene route from dictionary
        if (sceneRouteDictionary.TryGetValue(fromSceneName + toSceneName, out sceneRoute))
        {
            return sceneRoute;
        }
        else
        {
            return null;
        }
    }
    public bool BuildPath(SceneName sceneName, Vector2Int startGridPosition, Vector2Int endGridPosition, Stack<NPCMovementStep> npcMovementStepStack)
    {
        if (aStar.BuildPath(sceneName, startGridPosition, endGridPosition, npcMovementStepStack))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
