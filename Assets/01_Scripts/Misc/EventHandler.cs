using System;
using System.Collections.Generic;
using UnityEngine;


public delegate void MovementDelegate(float xInput, float yInput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idelLeft, bool idleRight);
public static class EventHandler
{
    public static event MovementDelegate MovementEvent;
    public static void CallMovementEvent(float xInput, float yInput, bool isWalking, bool isRunning, bool isIdle, bool isCarrying,
    ToolEffect toolEffect,
    bool isUsingToolRight, bool isUsingToolLeft, bool isUsingToolUp, bool isUsingToolDown,
    bool isLiftingToolRight, bool isLiftingToolLeft, bool isLiftingToolUp, bool isLiftingToolDown,
    bool isPickingRight, bool isPickingLeft, bool isPickingUp, bool isPickingDown,
    bool isSwingingToolRight, bool isSwingingToolLeft, bool isSwingingToolUp, bool isSwingingToolDown,
    bool idleUp, bool idleDown, bool idelLeft, bool idleRight)
    {

        MovementEvent?.Invoke(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
    isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
    isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
    isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
    isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
    idleUp, idleDown, idelLeft, idleRight);
    }

    public static event Action RemoveSelectealtemFromlnventoryEvent;
    public static void CallRemoveSelectealtemFromlnventoryEvent()
    {

        RemoveSelectealtemFromlnventoryEvent?.Invoke();
    }


    public static event Action SceneLoadAfterEvent;//场景加载完成
    public static void CallSceneLoadAfterEvent()
    {

        SceneLoadAfterEvent?.Invoke();
    }
    public static event Action SceneUnloadBeforeEvent;//场景加载完成
    public static void CallSceneUnloadBeforeEvent()
    {

        SceneUnloadBeforeEvent?.Invoke();
    }
    public static event Action<InventoryLocation, List<InventoryItem>> InventoryUpdateEvent;
    public static void CallInventoryUpdateEvent(InventoryLocation location, List<InventoryItem> inventory)
    {

        InventoryUpdateEvent?.Invoke(location, inventory);
    }

    public static event Action DropSelectedItemEvent;
    public static void CallDropSelectedItemEvent()
    {

        DropSelectedItemEvent?.Invoke();
    }

    public static event Action<Vector3, HarvestActionEffect> HarvestActionEffectEvent;
    public static void CallHarvestActionEffectEvent(Vector3 position, HarvestActionEffect effect)
    {

        HarvestActionEffectEvent?.Invoke(position, effect);
    }

    public static event Action InstantiateCropPrefabsEvent;
    public static void CallInstantiateCropPrefabsEvent()
    {

        InstantiateCropPrefabsEvent?.Invoke();
    }

    #region 时间系统事件
    public static event Action<int, Season, int, string, int, int, int> AdvanceGameMinuteEvent;
    public static void CallAdvanceGameMinuteEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {

        AdvanceGameMinuteEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameHourEvent;
    public static void CallAdvanceGameHourEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {

        AdvanceGameHourEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameDayEvent;
    public static void CallAdvanceGameDayEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameDayEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameSeasonEvent;
    public static void CallAdvanceGameSeasonEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameSeasonEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }

    public static event Action<int, Season, int, string, int, int, int> AdvanceGameYearEvent;
    public static void CallAdvanceGameYearEvent(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        AdvanceGameYearEvent?.Invoke(gameYear, gameSeason, gameDay, gameDayOfWeek, gameHour, gameMinute, gameSecond);
    }
    #endregion
}
