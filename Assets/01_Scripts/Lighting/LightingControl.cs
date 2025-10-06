using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering.Universal;
public class LightingControl : MonoBehaviour
{
    [SerializeField] private LightingSchedule lightingSchedule;
    [SerializeField] private bool isLightFlicker = false;
    [SerializeField][Range(0, 1)] private float lightFlickerIntensity;
    [SerializeField][Range(0, 0.2f)] private float lightFlickerTimeMin;
    [SerializeField][Range(0, 0.2f)] private float lightFlickerTimeMax;

    private Light2D light2D;
    private Dictionary<string, float> lightingBrightnessDictionary = new Dictionary<string, float>();
    private float currentLightIntensity;
    private float lightFlickerTimer = 0f;
    private Coroutine fadeInLightRoutine;
    private void Awake()
    {
        light2D = GetComponentInChildren<Light2D>();
        if (light2D == null)
        {
            enabled = false;
        }
        foreach (LightingBrightness lightingBrightness in lightingSchedule.lightingBrightnessArray)
        {
            string key = lightingBrightness.season.ToString() + lightingBrightness.hour.ToString();
            lightingBrightnessDictionary.Add(key, lightingBrightness.lightIntensity);
        }

    }
    private void OnEnable()
    {
        EventHandler.AdvanceGameHourEvent += EventHandler_AdvanceGameHourEvent;
        EventHandler.SceneLoadAfterEvent += EventHandler_AfterSceneLoadEvent;
    }



    private void OnDisable()
    {
        EventHandler.AdvanceGameHourEvent -= EventHandler_AdvanceGameHourEvent;
        EventHandler.SceneLoadAfterEvent -= EventHandler_AfterSceneLoadEvent;
    }
    private void EventHandler_AfterSceneLoadEvent()
    {
        SetLightingAfterSceneLoaded();
    }

    private void SetLightingAfterSceneLoaded()
    {
        Season gameSeason = TimeManager.Instance.GetGameSeason();
        int gameHour = TimeManager.Instance.GetGameTime().Hours;
        SetLightingIntensity(gameSeason, gameHour, false);
    }

    private void EventHandler_AdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek, int gameHour, int gameMinute, int gameSecond)
    {
        SetLightingIntensity(gameSeason, gameHour, true);
    }

    private void SetLightingIntensity(Season gameSeason, int gameHour, bool fadein)
    {
        int i = 0;
        while (i < 23)
        {
            string key = gameSeason.ToString() + gameHour.ToString();
            if (lightingBrightnessDictionary.TryGetValue(key, out float targetLightingIntensity))
            {
                if (fadein)
                {
                    if (fadeInLightRoutine != null) StopCoroutine(fadeInLightRoutine);
                    fadeInLightRoutine = StartCoroutine(FadeInLightRoutine(targetLightingIntensity));
                }
                else
                {
                    currentLightIntensity = targetLightingIntensity;
                }
                break;
            }
            i++;
            gameHour--;
            if (gameHour < 0)
                gameHour = 23;
        }
    }
    private IEnumerator FadeInLightRoutine(float targetLightingIntensity)
    {
        float fadeDuration = 5f;
        float fadeSpeed = Mathf.Abs(currentLightIntensity - targetLightingIntensity) / fadeDuration;
        while (!Mathf.Approximately(currentLightIntensity, targetLightingIntensity))
        {
            currentLightIntensity = Mathf.MoveTowards(currentLightIntensity, targetLightingIntensity, fadeSpeed * Time.deltaTime);
            yield return null; 
        }
        currentLightIntensity = targetLightingIntensity;
    }
    private void Update()
    {
        if (isLightFlicker)
            lightFlickerTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        if(lightFlickerTimer < 0 && isLightFlicker)
        {
            LightFlicker();
        }
        else
        {
            light2D.intensity = currentLightIntensity;
        }
    }

    private void LightFlicker()
    {
        light2D.intensity = Random.Range(currentLightIntensity, currentLightIntensity + (currentLightIntensity*lightFlickerIntensity));
        lightFlickerTimer = Random.Range(lightFlickerTimeMin, lightFlickerTimeMax);
    }
}
