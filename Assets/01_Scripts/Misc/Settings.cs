

using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //玩家相关属性
    public const float runningSpeed = 5.333f;
    public const float walkingSpeed = 2.666f;
    
    public static float playerCenterYOffset = 0.875f;

    //玩家动画相关
    public static float useToolAnimationPause = 0.25f;
    public static float liftToolAnimationPause = 0.4f;
    public static float pickAnimationPause = 1f;
    public static float afterUseToolAnimationPause = 0.2f;
    public static float afterLiftToolAnimationPause = 0.6f;
    public static float afterPickAnimationPause = 1f;

    #region 玩家动画参数
    public static int xInput;
    public static int yInput;
    public static int isWalking;
    public static int isRunning;
    public static int isIdle;
    public static int isCarrying;
    public static int toolEffect;
    public static int isUsingToolRight;
    public static int isUsingToolLeft;
    public static int isUsingToolUp;
    public static int isUsingToolDown;
    public static int isLiftingToolRight;
    public static int isLiftingToolLeft;
    public static int isLiftingToolUp;
    public static int isLiftingToolDown;
    public static int isPickingRight;
    public static int isPickingLeft;
    public static int isPickingUp;
    public static int isPickingDown;
    public static int isSwingingToolRight;
    public static int isSwingingToolLeft;
    public static int isSwingingToolUp;
    public static int isSwingingToolDown;
    

    public static int idleUp;
    public static int idleDown;
    public static int idleLeft;
    public static int idleRight;
    #endregion
    #region NPC动画参数
    public static int walkUp;
    public static int walkDown;
    public static int walkLeft;
    public static int walkRight;
    public static int eventAnimation;

    #endregion
    static Settings()
    { 
        xInput = Animator.StringToHash("xInput");
        yInput = Animator.StringToHash("yInput");
        isWalking = Animator.StringToHash("isWalking");
        isRunning = Animator.StringToHash("isRunning");
        isIdle = Animator.StringToHash("isIdle");
        isCarrying = Animator.StringToHash("isCarrying");
        toolEffect = Animator.StringToHash("toolEffect");
        isUsingToolRight = Animator.StringToHash("isUsingToolRight");
        isUsingToolLeft = Animator.StringToHash("isUsingToolLeft");
        isUsingToolUp = Animator.StringToHash("isUsingToolUp");
        isUsingToolDown = Animator.StringToHash("isUsingToolDown");
        isLiftingToolRight = Animator.StringToHash("isLiftingToolRight");
        isLiftingToolLeft = Animator.StringToHash("isLiftingToolLeft");
        isLiftingToolUp = Animator.StringToHash("isLiftingToolUp");
        isLiftingToolDown = Animator.StringToHash("isLiftingToolDown");
        isPickingRight = Animator.StringToHash("isPickingRight");
        isPickingLeft = Animator.StringToHash("isPickingLeft");
        isPickingUp = Animator.StringToHash("isPickingUp");
        isPickingDown = Animator.StringToHash("isPickingDown");
        isSwingingToolRight = Animator.StringToHash("isSwingingToolRight");
        isSwingingToolLeft = Animator.StringToHash("isSwingingToolLeft");
        isSwingingToolUp = Animator.StringToHash("isSwingingToolUp");
        isSwingingToolDown = Animator.StringToHash("isSwingingToolDown");
        idleUp = Animator.StringToHash("idleUp");
        idleDown = Animator.StringToHash("idleDown");
        idleLeft = Animator.StringToHash("idleLeft");
        idleRight = Animator.StringToHash("idleRight");

        walkDown = Animator.StringToHash("walkDown");
        walkUp = Animator.StringToHash("walkUp");
        walkLeft = Animator.StringToHash("walkLeft");
        walkRight = Animator.StringToHash("walkRight");
        eventAnimation = Animator.StringToHash("eventAnimation");
    }

    //瓦片地图相关
    public const float gridCellSize = 1f;
    public static Vector2 cursorSize = Vector2.one;
    public const float gridCellDiagonalSize = 1.41f;
    public const int maxGridWidth = 999999;
    public const int maxGridHeight = 999999;
    //玩家库存
    public static int playerInitialInventoryCapacity = 24;  //玩家初始库存容量
    public static int playerMaxInventoryCapacity = 24;  //玩家最大库存容量

    //NPC移动
    public static float pixelSize = 0.0625f;
    public const float secondsPerGameSecond = 0.012f;
    

    //标签
    public static class Tags
    {
        public static string ItemsParentTransform = "ItemsParentTransform";
        public static string SceneSwitchFade = "SceneSwitchFade";
        public static string SceneBound = "SceneBound";
        public static string GroundDecoration1 = "GroundDecoration1";
        public static string GroundDecoration2 = "GroundDecoration2";
        public static string CropsParentTransform = "CropsParentTransform";
    }
    //时间系统
    public const float secondPerGameSecond = 0.012f;

    //镰刀
    public const int maxCollidersToTestPerReapSwing = 15;
    public const int maxTargetComponentsToDestoryPerReapSwing = 2;

    //淡入淡出
    public const float fadeInSeconds = 0.25f;
    public const float fadeOutSeconds = 0.35f;
    public const float targetAlpha = 0.45f;

}
