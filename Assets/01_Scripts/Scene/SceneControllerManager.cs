using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControllerManager : Single<SceneControllerManager>
{
    [SerializeField]private string startSceneName;

    public string StartSceneName { get => startSceneName; set => startSceneName = value; }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(IELoadStartScene(StartSceneName));
    }
    
    
    public void GoToNextScene(string sceneName, Vector2 playerPos,bool isLoadFromFile)
    {
        StartCoroutine(IEGoToNextScene(sceneName, playerPos,isLoadFromFile));
    }
    /// <summary>
    /// 加载游戏开始场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator IELoadStartScene(string sceneName)
    {
        int sceneCount = SceneManager.sceneCount;
        EventHandler.CallSceneUnloadBeforeEvent();
        for (int i = 1; i< sceneCount; i++) 
        {
            Scene unloadScene = SceneManager.GetSceneAt(i);
            SceneManager.UnloadSceneAsync(unloadScene);
        }
        yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        
        EventHandler.CallSceneLoadAfterEvent();
        SaveLoadManager.Instance.RestoreCurrentSceneData();
    }

    /// <summary>
    /// 切换场景
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="playerPos"></param>
    /// <returns></returns>
    private IEnumerator IEGoToNextScene(string sceneName,Vector2 playerPos,bool isLoadFromFile)
    {
        EventHandler.CallSceneUnloadBeforeEvent();
        yield return FadeManager.Instance.SceneSwitchFadeOut(isLoadFromFile);
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        Player.Instance.SetPlayerPos(playerPos);
        
        EventHandler.CallSceneLoadAfterEvent();
        yield return FadeManager.Instance.SceneSwitchFadeIn();
    }
}
