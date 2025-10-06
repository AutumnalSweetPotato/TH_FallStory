
using System;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : Single<TimeManager>,ISaveable
{
    private int gameYear = 1;
    private Season gameSeason = Season.Spring;
    private int gameDay = 1;
    private int gameHour = 6;
    private int gameMinute = 30;
    private int gameSecond = 0;
    private string gameDayOfWeek = "Mon";
    private bool gameClockPaused = false;
    private float gameTick = 0f;

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get => _iSaveableUniqueID; set => _iSaveableUniqueID = value; }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get => _gameObjectSave; set => _gameObjectSave = value; }
    protected override void Awake()
    {
        base.Awake();
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;

        GameObjectSave = new GameObjectSave();
    }
    private void Start()
    {
        EventHandler.CallAdvanceGameMinuteEvent(gameYear,gameSeason,gameDay,gameDayOfWeek,gameHour,gameMinute,gameSecond);
    }
    private void Update()
    {
        gameTick += Time.deltaTime;
        if(gameTick >= Settings.secondPerGameSecond)
        {
            gameTick -= Settings.secondPerGameSecond;
            UpdateGameSecond();
        }
    }
    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.SceneLoadAfterEvent += AfterSceneLoad;
        EventHandler.SceneUnloadBeforeEvent += BeforeSceneUnload;
        
    }
    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.SceneLoadAfterEvent -= AfterSceneLoad;
        EventHandler.SceneUnloadBeforeEvent -= BeforeSceneUnload;
    }
    private void AfterSceneLoad()
    {
        gameClockPaused = false;
    }

    private void BeforeSceneUnload()
    {
        gameClockPaused = true;
    }

   
    private void UpdateGameSecond()
    {
        if(!gameClockPaused)
        {
            gameSecond++;
            if (gameSecond >= 59)
            {
                gameSecond = 0;
                gameMinute++;

                if (gameMinute >= 59)
                {
                    gameMinute = 0;
                    gameHour++;

                    if (gameHour >= 23)
                    {
                        gameHour = 0;
                        gameDay++;

                        if (gameDay > 30)
                        {
                            gameDay = 1;
                            int gs = ((int)gameSeason);
                            gs++;
                            gameSeason = (Season)gs;

                            if (gs > 3)
                            {
                                gs = 0;
                                gameSeason = (Season)gs;
                                gameYear++;
                                if (gameYear > 9999)
                                    gameYear = 1;
                                EventHandler.CallAdvanceGameYearEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                            }
                            EventHandler.CallAdvanceGameSeasonEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                        }
                        gameDayOfWeek = GetDayOfWeek();
                        EventHandler.CallAdvanceGameDayEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                    }
                    EventHandler.CallAdvanceGameHourEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                }
                EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                //Debug.Log("GameYear:"+gameYear+"--GameSeason:"+gameSeason+"--GameDay:"+gameDay+"--GameDayOfWeek"+gameDayOfWeek
                //    +"--GameHour:"+gameHour+"--GameMinute:"+gameMinute);
            }
        }
        
    }

    private string GetDayOfWeek()
    {
        int totalDays = ((int)gameSeason) * 30 + gameDay;
        int dayOfWeek = totalDays % 7;  
        switch (dayOfWeek)
        {
            case 1:
                return "Mon";
            case 2:
                return "Tus";
            case 3:
                return "Wed";
            case 4:
                return "Thu";
            case 5:
                return "Fri";
            case 6:
                return "Sat";
            case 0:
                return "Sun";
            default:
                return "";
        }
        
    }

    #region ±£´æ
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }
    public void ISaveableDeregister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }
    public void ISaveableStoreScene(string sceneName)
    {

    }
    public void ISaveableRestoreScene(string sceneName)
    {

    }
    public void ISaveableLoad(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;
            if (gameObjectSave.sceneData.TryGetValue("Main", out SceneSave sceneSave))
            {
                if (sceneSave.intDictionary != null && sceneSave.stringDictionary != null)
                {
                    if (sceneSave.intDictionary.TryGetValue("gameYear", out int storedGameYear))
                    {
                        gameYear = storedGameYear;
                    }
                    if (sceneSave.intDictionary.TryGetValue("gameDay", out int storedGameDay))
                    {
                        gameDay = storedGameDay;
                    }
                    if (sceneSave.intDictionary.TryGetValue("gameHour", out int storedGameHour))
                    {
                        gameHour = storedGameHour;
                    }
                    if (sceneSave.intDictionary.TryGetValue("gameMinute", out int storedGameMinute))
                    {
                        gameMinute = storedGameMinute;
                    }
                    if (sceneSave.intDictionary.TryGetValue("gameSecond", out int storedGameSecond))
                    {
                        gameSecond = storedGameSecond;
                    }
                    if (sceneSave.stringDictionary.TryGetValue("gameDayOfWeek", out string storedGameDayOfWeek))
                    {
                        gameDayOfWeek = storedGameDayOfWeek;
                    }
                    if (sceneSave.stringDictionary.TryGetValue("gameSeason", out string storedGameSeason))
                    {
                        if (Enum.TryParse(storedGameSeason, out Season storedGameSeasonEnum))
                        {
                            gameSeason = storedGameSeasonEnum;
                        }
                    }
                    gameTick = 0f;
                    EventHandler.CallAdvanceGameMinuteEvent(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
                }
            }
        }
    }
    public GameObjectSave ISaveableSave()
    {
        SceneSave sceneSave = new SceneSave();
        GameObjectSave.sceneData.Remove("Main");
        sceneSave.intDictionary = new Dictionary<string, int>();
        sceneSave.stringDictionary = new Dictionary<string, string>();

        sceneSave.intDictionary.Add("gameYear", gameYear);
        sceneSave.intDictionary.Add("gameDay", gameDay);
        sceneSave.intDictionary.Add("gameHour", gameHour);
        sceneSave.intDictionary.Add("gameMinute", gameMinute);
        sceneSave.intDictionary.Add("gameSecond", gameSecond);

        sceneSave.stringDictionary.Add("gameDayOfWeek", gameDayOfWeek);
        sceneSave.stringDictionary.Add("gameSeason", gameSeason.ToString());

        GameObjectSave.sceneData.Add("Main", sceneSave);
        return GameObjectSave;

    }
    #endregion

    public TimeSpan GetGameTime()
    {
        TimeSpan gameTime = new TimeSpan(gameHour,gameMinute,gameSecond);
        return gameTime;
    }
    public Season GetGameSeason()
    {
        return gameSeason;
    }
    public void TestAdvanceGameMinute()
    {
        for(int i = 0;i < 60;i++)
        {
            UpdateGameSecond();
        }
    }
    public void TestAdvanceGameDay()
    {
        for (int i = 0; i < 86400; i++)
        {
            UpdateGameSecond();
        }
    }
}
