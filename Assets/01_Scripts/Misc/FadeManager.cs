using System.Collections;

using UnityEngine;

public class FadeManager : Single<FadeManager>
{
    public float fadeTime = 1.0f;
    private bool isFadeing;

    public IEnumerator SceneSwitchFadeIn()//关卡加载后
    {
        if (!isFadeing)
        {
            Player.Instance.EnablePlayerInput();
            SaveLoadManager.Instance.RestoreCurrentSceneData();
            yield return Fade(0);
            
        }
    }
    public IEnumerator SceneSwitchFadeOut(bool isLoadFromFile) //关卡卸载前
    {
        if (!isFadeing)
        {
            Player.Instance.DisablePlayerInput();
            if(!isLoadFromFile)
            SaveLoadManager.Instance.StoreCurrentSceneData();
            yield return Fade(1); 
        }
       
        
    }
    private IEnumerator Fade(float targetAlpha)
    {
        CanvasGroup fadeCanvas = GameObject.FindGameObjectWithTag
        (Settings.Tags.SceneSwitchFade).GetComponent<CanvasGroup>();
        isFadeing = true;
        fadeCanvas.blocksRaycasts = true;
        float speed = Mathf.Abs(fadeCanvas.alpha - targetAlpha) / fadeTime;
        while (!Mathf.Approximately(fadeCanvas.alpha, targetAlpha))
        {
            fadeCanvas.alpha = Mathf.MoveTowards(fadeCanvas.alpha, targetAlpha, Time.deltaTime * speed);
            yield return null;
        }
        fadeCanvas.blocksRaycasts = false;
        isFadeing = false;
    }
}
