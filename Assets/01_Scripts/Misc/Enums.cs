public enum ToolEffect
{
    None,
    Watering
}

public enum PlayerDirection
{
    Up,
    Down,
    Left,
    Right
}

public enum ItemType
{
    Seed,               //种子
    Commodity,          //商品
    Watering_tool,      //浇水工具
    Hoeing_tool,        //锄地工具
    Chopping_tool,      //砍树工具
    Breaking_tool,      //破坏工具
    Reaping_tool,       //收割工具
    Collection_tool,    //采集工具
    Reapable_scenary,   //可收割的场景
    Furniture,          //家具
    None,
    Count               //枚举数量
}

public enum HarvestActionEffect
{ 
    deciduousLeavesFalling,         //树叶飘落
    pineConesFalling,               //松果掉落
    choppingTreeTrunk,              //砍伐树干
    breakingStone,                  //破坏石头
    reaping,                        //收割
    none
}

public enum InventoryLocation 
{
    Player,
    Chest,              //箱子
    Count
}

public enum AnimationName
{
    idleDown,
    idleUp,
    idleLeft,
    idleRight,
    walkDown,
    walkUp,
    walkLeft,
    walkRight,
    runDown,
    runUp,
    runLeft,
    runRight,
    useToolDown,
    useToolUp,
    useToolLeft,
    useToolRight,
    swingToolDown,
    swingToolUp,
    swingToolLeft,
    swingToolRight,
    liftToolDown,
    liftToolUp,
    liftToolLeft,
    liftToolRight,
    holdToolDown,
    holdToolUp,
    holdToolLeft,
    holdToolRight,
    pickDown,
    pickUp,
    pickLeft,
    pickRight,
    count
}
public enum CharacterPartAnimator
{
    body,
    arms,
    hair,
    tool,
    hat,
    count
}
public enum PartVarinatColour
{
    none,
    count
}
public enum PartVarinatType
{
    none, 
    carry,
    hoe,            //锄头
    pickaxe,        //镐头
    axe,            //斧
    scythe,         //镰刀
    wateringCan,    //浇水罐
    count
}

public enum GridBoolProperty
{
    diggable,
    canDropItem,
    canPlaceFuniture,
    isPath,
    isNPCObstacle
}

public enum Season
{
    Spring,
    Summer,
    Autumn,
    Winter,
    None,
    Count
}

public enum SceneName
{
    Scene1_Fram,
    Scene2_Fram
}

public enum Weather
{
    dry,
    raining,
    snowing,
    none,
    count
}

public enum Direction
{
    none,
    up,
    down,
    left,
    right
}

public enum SoundName
{
    none = 0,
    effectFootstepSoftGround = 10,
    effectFootstepHardGround = 20,
    effectAxe = 30,
    effectPickaxe = 40,
    effectScythe = 50,
    effectHoe = 60,
    effectWateringCan = 70,
    effectBasket = 80,
    effectPickupSound = 90,
    effectTrustee = 100,
    effectTreeFalling = 110,
    effectPlantingSound = 120,
    effectPluck = 130,
    effectStoneShatter = 140,
    effectWoodSplinters = 150,
    ambientCountryside1 = 1000,
    ambientCountryside2 = 1010,
    ambientIndoors1 = 1020,
    musicCalm3 = 2000,
    musicCalm1 = 2010

}

public enum QuestType
{
    none,
    gathering, //收集
    talk, //对话
    reach,//探索
    count
}
public enum QuestStatus
{
    none,
    waitting,
    accepted,
    completed,
    count
}