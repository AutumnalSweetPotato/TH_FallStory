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
    Seed,               //����
    Commodity,          //��Ʒ
    Watering_tool,      //��ˮ����
    Hoeing_tool,        //���ع���
    Chopping_tool,      //��������
    Breaking_tool,      //�ƻ�����
    Reaping_tool,       //�ո��
    Collection_tool,    //�ɼ�����
    Reapable_scenary,   //���ո�ĳ���
    Furniture,          //�Ҿ�
    None,
    Count               //ö������
}

public enum HarvestActionEffect
{ 
    deciduousLeavesFalling,         //��ҶƮ��
    pineConesFalling,               //�ɹ�����
    choppingTreeTrunk,              //��������
    breakingStone,                  //�ƻ�ʯͷ
    reaping,                        //�ո�
    none
}

public enum InventoryLocation 
{
    Player,
    Chest,              //����
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
    hoe,            //��ͷ
    pickaxe,        //��ͷ
    axe,            //��
    scythe,         //����
    wateringCan,    //��ˮ��
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
    gathering, //�ռ�
    talk, //�Ի�
    reach,//̽��
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