
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : Single<Player>, ISaveable
{
    private AnimationOverrides animationOverrides;                      //动画覆盖类
    private List<CharacterAttribute> characterAttributeList;            //角色状态列表
    private CharacterAttribute armsCharacterAttribute;                  //角色状态-手臂
    private CharacterAttribute toolCharacterAttribute;                  //角色状态-工具
    private Rigidbody2D rb;                                             //刚体
    private PlayerDirection dir;                                        //玩家移动方向
    private Camera mainCamera;                                          //摄像机
    private bool playerInputIsDisabled = false;                         //玩家输入是否禁用
    private bool playerToolUseDisabled = false;                         //玩家是否禁用工具使用
    private GridCursor gridCursor;                                      //网格游标
    private Cursor cursor;                                              //游标
    private WaitForSeconds afterUseToolAnimationPause;                  //工具使用后动画时长
    private WaitForSeconds useToolAnimationPause;                       //工具使用动画时长
    private WaitForSeconds liftToolAnimationPause;                      //提起水壶动画时长
    private WaitForSeconds afterLiftToolAnimationPause;                 //提起水壶后动画时长
    private WaitForSeconds pickAnimaitonPause;                          //采摘动画时长
    private WaitForSeconds afterPickAnimationPause;                     //采摘后动画时长

    [SerializeField] private float speedMultiplier = 1;                 //玩家速度倍数
    [SerializeField] private SpriteRenderer equippedItemSpriteRenderer; //玩家装备物品的SpriteRenderer
    //[SerializeField] private GameObject perfab;

    //*********************************************任务系统字段*********************************************
    public Dictionary<string, Quest> questDictionary = new Dictionary<string, Quest>();
    //*********************************************-----------*********************************************

    //*********************************************玩家属性字段********************************************
    public int exp;
    public int gold;
    private float movementSpeed;    
    //*********************************************-----------*********************************************

    private string _iSaveableUniqueID;
    public string ISaveableUniqueID { get => _iSaveableUniqueID; set => _iSaveableUniqueID = value; }

    private GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get => _gameObjectSave; set => _gameObjectSave = value; }

    #region 玩家动画参数
    private float xInput;
    private float yInput;
    private bool isWalking;
    private bool isRunning;
    private bool isIdle;
    private bool isCarrying;
    private ToolEffect toolEffect = ToolEffect.None;
    private bool isUsingToolRight;
    private bool isUsingToolLeft;
    private bool isUsingToolUp;
    private bool isUsingToolDown;
    private bool isLiftingToolRight;
    private bool isLiftingToolLeft;
    private bool isLiftingToolUp;
    private bool isLiftingToolDown;
    private bool isPickingRight;
    private bool isPickingLeft;
    private bool isPickingUp;
    private bool isPickingDown;
    private bool isSwingingToolRight;
    private bool isSwingingToolLeft;
    private bool isSwingingToolUp;
    private bool isSwingingToolDown;
    private bool idleUp;
    private bool idleDown;
    private bool idelLeft;
    private bool idleRight;
    #endregion
    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animationOverrides = GetComponentInChildren<AnimationOverrides>();
        armsCharacterAttribute = new CharacterAttribute
            (CharacterPartAnimator.arms, PartVarinatColour.none, PartVarinatType.none);
        toolCharacterAttribute = new CharacterAttribute
            (CharacterPartAnimator.tool, PartVarinatColour.none, PartVarinatType.hoe);
        characterAttributeList = new List<CharacterAttribute>();
        mainCamera = Camera.main;

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();

    }
    private void Start()
    {
        afterUseToolAnimationPause = new WaitForSeconds(Settings.afterUseToolAnimationPause);
        useToolAnimationPause = new WaitForSeconds(Settings.useToolAnimationPause);
        liftToolAnimationPause = new WaitForSeconds(Settings.liftToolAnimationPause);
        afterLiftToolAnimationPause = new WaitForSeconds(Settings.afterLiftToolAnimationPause);
        pickAnimaitonPause = new WaitForSeconds(Settings.pickAnimationPause);
        afterPickAnimationPause = new WaitForSeconds(Settings.afterPickAnimationPause);
    }

    private void OnEnable()
    {
        ISaveableRegister();
    }
    private void OnDisable()
    {
        ISaveableDeregister();
    }
    private void Update()
    {


        #region 玩家输入
        if (!playerInputIsDisabled)
        {
            ResetAnimationTriggers();
            PlayerMovenmentInput();
            PlayerWalkInput();
            PlayerClickInput();
            PlayerTestInput();
            EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
              isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
              isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
              isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
              isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
              idleUp, idleDown, idelLeft, idleRight
              );
        }
        #endregion 玩家输入

    }



    private void FixedUpdate()
    {
        if (!playerInputIsDisabled)
        {
            Vector2 move = new Vector2(xInput * movementSpeed * Time.deltaTime * speedMultiplier, yInput * movementSpeed * Time.deltaTime * speedMultiplier);
            rb.MovePosition(rb.position + move);
        }

    }

    public void ShowCarriedItem(int itemID)
    {
        ItemDetails itemDetails = InventoryManager.Instance.GetItemDetailsByID(itemID);
        if (itemDetails != null)
        {
            equippedItemSpriteRenderer.sprite = itemDetails.itemSprite;
            equippedItemSpriteRenderer.color = new Color(1, 1, 1, 1);
            armsCharacterAttribute.partVarinatType = PartVarinatType.carry;
            characterAttributeList.Clear();
            characterAttributeList.Add(armsCharacterAttribute);
            animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);
            isCarrying = true;
        }
    }
    public void ClearCarriedItem()
    {
        equippedItemSpriteRenderer.sprite = null;
        equippedItemSpriteRenderer.color = new Color(1, 1, 1, 0);
        armsCharacterAttribute.partVarinatType = PartVarinatType.none;
        characterAttributeList.Clear();
        characterAttributeList.Add(armsCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);
        isCarrying = false;
    }

    private void PlayerWalkInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            movementSpeed = Settings.walkingSpeed;
            speedMultiplier = 1;
        }
        else if (Input.GetMouseButton(2))
        {
            isRunning = false;
            isWalking = true;
            isIdle = false;
            speedMultiplier = 5;
        }
        else
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
            speedMultiplier = 1;
        }
    }

    private void PlayerMovenmentInput()
    {
        yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0 && yInput != 0)
        {
            xInput = xInput * 0.71f;
            yInput = yInput * 0.71f;
        }
        if (xInput != 0 || yInput != 0)
        {
            isRunning = true;
            isWalking = false;
            isIdle = false;
            movementSpeed = Settings.runningSpeed;
            if (xInput < 0)
                dir = PlayerDirection.Left;
            else if (xInput > 0)
                dir = PlayerDirection.Right;
            else if (yInput < 0)
                dir = PlayerDirection.Down;
            else if (yInput > 0)
                dir = PlayerDirection.Up;
        }
        else if (xInput == 0 && yInput == 0)
        {
            isWalking = false;
            isRunning = false;
            isIdle = true;
        }
    }

    private void ResetAnimationTriggers()
    {
        toolEffect = ToolEffect.None;
        isUsingToolRight = false;
        isUsingToolLeft = false;
        isUsingToolUp = false;
        isUsingToolDown = false;
        isLiftingToolRight = false;
        isLiftingToolLeft = false;
        isLiftingToolUp = false;
        isLiftingToolDown = false;
        isPickingRight = false;
        isPickingLeft = false;
        isPickingUp = false;
        isPickingDown = false;
        isSwingingToolRight = false;
        isSwingingToolLeft = false;
        isSwingingToolUp = false;
        isSwingingToolDown = false;
    }
    private void PlayerClickInput()
    {
        if (!playerToolUseDisabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //获取gridCursor实例
                if (gridCursor == null)
                {
                    gridCursor = FindObjectOfType<GridCursor>();
                }
                if (cursor == null)
                {
                    cursor = FindObjectOfType<Cursor>();
                }
                if (gridCursor.CursorIsEnabled || cursor.CursorIsEnabled)
                {
                    //获取网格位置和玩家网格位置
                    Vector3Int cursorGridPosition = gridCursor.GetGridPositionForCursor();
                    Vector3Int playerGridPosition = gridCursor.GetGridPositionForPlayer();
                    ProcessPlayerClickInput(cursorGridPosition, playerGridPosition);
                }
            }
        }

    }

    private void ProcessPlayerClickInput(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        ResetMovenment();

        //玩家点击方向
        Vector3Int playerClickDirection = GetPlayerClickDirection(cursorGridPosition, playerGridPosition);
        //获取网格属性详情
        GridPropertyDetails gridPropertyDetails = GridPropertiesManager.Instance.GetGridPropertyDetails(cursorGridPosition);

        ItemDetails itemDetails = InventoryManager.Instance.GetSelectedInventoryItemDetails(InventoryLocation.Player);
        if (itemDetails != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Seed:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputSeed(gridPropertyDetails, itemDetails);
                    }
                    break;
                case ItemType.Commodity:
                    if (Input.GetMouseButtonDown(0))
                    {
                        ProcessPlayerClickInputCommodity(itemDetails);
                    }
                    break;
                case ItemType.Watering_tool:
                case ItemType.Hoeing_tool:
                case ItemType.Reaping_tool:
                case ItemType.Collection_tool:
                case ItemType.Chopping_tool:
                case ItemType.Breaking_tool:
                    if (Input.GetMouseButtonDown(0))
                        ProcessPlayerClickInputTool(gridPropertyDetails, itemDetails, playerClickDirection);
                    break;


            }
        }
    }



    private Vector3Int GetPlayerClickDirection(Vector3Int cursorGridPosition, Vector3Int playerGridPosition)
    {
        if (cursorGridPosition.x > playerGridPosition.x)
            return Vector3Int.right;
        else if (cursorGridPosition.x < playerGridPosition.x)
            return Vector3Int.left;
        else if (cursorGridPosition.y > playerGridPosition.y)
            return Vector3Int.up;
        else if (cursorGridPosition.y < playerGridPosition.y)
            return Vector3Int.down;
        else return Vector3Int.down;
    }
    private void ProcessPlayerClickInputTool(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
       
        switch (itemDetails.itemType)
        {
            case ItemType.Hoeing_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    HoeGroundAtCursor(gridPropertyDetails, playerClickDirection);
                }
                break;
            case ItemType.Watering_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    WaterGroundAtCursor(gridPropertyDetails, playerClickDirection);
                }
                break;
            case ItemType.Reaping_tool:
                if (cursor.CursorPositionIsValid)
                {
                    playerClickDirection = GetPlayerDirection(cursor.GetWorldPositonForCursor(), GetPlayerCenterPosition());
                    ReapInPlayerDirectionAtCursor(itemDetails, playerClickDirection);
                }
                break;
            case ItemType.Collection_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    CollectInPlayerDirection(gridPropertyDetails, itemDetails, playerClickDirection);
                }
                break;
            case ItemType.Chopping_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    ChopInPlayerDirection(gridPropertyDetails, itemDetails, playerClickDirection);
                   
                }
                break;
            case ItemType.Breaking_tool:
                if (gridCursor.CursorPositionIsValid)
                {
                    BreakInPlayerDirectior(gridPropertyDetails, itemDetails, playerClickDirection);
                   
                }
                break;
        }
        
    }



    private Vector3Int GetPlayerDirection(Vector3 cursorPosition, Vector3 playerPosition)
    {
        if (cursorPosition.x > playerPosition.x
            && cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f)
            && cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f)
            )
        {
            return Vector3Int.right;
        }
        else if (cursorPosition.x < playerPosition.x
            && cursorPosition.y < (playerPosition.y + cursor.ItemUseRadius / 2f)
            && cursorPosition.y > (playerPosition.y - cursor.ItemUseRadius / 2f)
            )
        {
            return Vector3Int.left;
        }
        else if (cursorPosition.y > playerPosition.y)
        {
            return Vector3Int.up;
        }
        else
        {
            return Vector3Int.down;
        }

    }

    #region 工具使用
    private void ChopInPlayerDirection(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        StartCoroutine(ChopInPlayerDirectionRoutine(gridPropertyDetails, itemDetails, playerClickDirection));
    }
    private void ReapInPlayerDirectionAtCursor(ItemDetails gridPropertyDetails, Vector3Int playerClickDirection)
    {
        StartCoroutine(ReapInPlayerDirectionAtCursorRoutine(gridPropertyDetails, playerClickDirection));
    }

    private void BreakInPlayerDirectior(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        StartCoroutine(BreakInPlayerDirectionRoutine(gridPropertyDetails, itemDetails, playerClickDirection));
    }

    private void WaterGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerClickDirection)
    {
        StartCoroutine(WaterGroundAtCursorRoutine(gridPropertyDetails, playerClickDirection));
    }

    private void HoeGroundAtCursor(GridPropertyDetails gridPropertyDetails, Vector3Int playerClickDirection)
    {
        StartCoroutine(HoeGroundAtCursorRoutine(gridPropertyDetails, playerClickDirection));
    }

    private void CollectInPlayerDirection(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        StartCoroutine(CollectInPlayerDirectionRoutine(gridPropertyDetails, itemDetails, playerClickDirection));
    }

    private IEnumerator ChopInPlayerDirectionRoutine(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        playerInputIsDisabled = true;
        playerToolUseDisabled = true;

        toolCharacterAttribute.partVarinatType = PartVarinatType.axe;
        characterAttributeList.Clear();
        characterAttributeList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);

        ProcessCropWithEquippedItemInPlayerDirection(playerClickDirection, itemDetails, gridPropertyDetails);

        yield return useToolAnimationPause;

        yield return afterUseToolAnimationPause;

        playerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }
    private IEnumerator CollectInPlayerDirectionRoutine(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        //禁用玩家输入和工具使用
        playerInputIsDisabled = true;
        playerToolUseDisabled = true;
        ProcessCropWithEquippedItemInPlayerDirection(playerClickDirection, itemDetails, gridPropertyDetails);
        yield return pickAnimaitonPause;
        //yield return afterPickAnimationPause;
        playerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }



    private IEnumerator HoeGroundAtCursorRoutine(GridPropertyDetails gridPropertyDetails, Vector3Int playerClickDirection)
    {
        //禁用玩家输入和工具使用
        playerInputIsDisabled = true;
        playerToolUseDisabled = true;

        //设置工具动画覆盖
        toolCharacterAttribute.partVarinatType = PartVarinatType.hoe;
        characterAttributeList.Clear();
        characterAttributeList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);

        if (playerClickDirection == Vector3Int.right)
            isUsingToolRight = true;
        else if (playerClickDirection == Vector3Int.left)
            isUsingToolLeft = true;
        else if (playerClickDirection == Vector3Int.up)
            isUsingToolUp = true;
        else if (playerClickDirection == Vector3Int.down)
            isUsingToolDown = true;
        yield return useToolAnimationPause;

        //设置网格的挖掘属性
        if (gridPropertyDetails.daysSinceDug == -1)
        {
            gridPropertyDetails.daysSinceDug = 0;
        }
        //设置网格属性
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);
        GridPropertiesManager.Instance.DisplayDugGround(gridPropertyDetails);
        yield return afterUseToolAnimationPause;

        playerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }
    private IEnumerator WaterGroundAtCursorRoutine(GridPropertyDetails gridPropertyDetails, Vector3Int playerClickDirection)
    {
        //禁用玩家输入和工具使用
        playerInputIsDisabled = true;
        playerToolUseDisabled = true;

        //设置工具动画覆盖
        toolCharacterAttribute.partVarinatType = PartVarinatType.wateringCan;
        characterAttributeList.Clear();
        characterAttributeList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);

        toolEffect = ToolEffect.Watering;

        if (playerClickDirection == Vector3Int.right)
            isLiftingToolRight = true;
        else if (playerClickDirection == Vector3Int.left)
            isLiftingToolLeft = true;
        else if (playerClickDirection == Vector3Int.up)
            isLiftingToolUp = true;
        else if (playerClickDirection == Vector3Int.down)
            isLiftingToolDown = true;
        yield return liftToolAnimationPause;

        //设置网格的挖掘属性
        if (gridPropertyDetails.daysSinceWatered == -1)
        {
            gridPropertyDetails.daysSinceWatered = 0;
        }
        //设置网格属性
        GridPropertiesManager.Instance.SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails);
        GridPropertiesManager.Instance.DisplayWaterGround(gridPropertyDetails);
        yield return afterLiftToolAnimationPause;

        playerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }
    private IEnumerator ReapInPlayerDirectionAtCursorRoutine(ItemDetails gridPropertyDetails, Vector3Int playerClickDirection)
    {
        //禁用玩家输入和工具使用
        playerInputIsDisabled = true;
        playerToolUseDisabled = true;


        //设置工具动画覆盖
        toolCharacterAttribute.partVarinatType = PartVarinatType.scythe;
        characterAttributeList.Clear();
        characterAttributeList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);

        UseToolInPlayerDirection(gridPropertyDetails, playerClickDirection);

        yield return useToolAnimationPause;
        playerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }
    private IEnumerator BreakInPlayerDirectionRoutine(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        //禁用玩家输入和工具使用
        playerInputIsDisabled = true;
        playerToolUseDisabled = true;


        //设置工具动画覆盖
        toolCharacterAttribute.partVarinatType = PartVarinatType.pickaxe;
        characterAttributeList.Clear();
        characterAttributeList.Add(toolCharacterAttribute);
        animationOverrides.ApplyCharacterCustomisationParameters(characterAttributeList);

        ProcessCropWithEquippedItemInPlayerDirection(playerClickDirection, itemDetails, gridPropertyDetails);

        yield return useToolAnimationPause;
        yield return afterUseToolAnimationPause;
        playerInputIsDisabled = false;
        playerToolUseDisabled = false;
    }



    #endregion


    private void ProcessCropWithEquippedItemInPlayerDirection(Vector3Int playerClickDirection, ItemDetails itemDetails, GridPropertyDetails gridPropertyDetails)
    {
        switch (itemDetails.itemType)
        {
            case ItemType.Collection_tool:
                if (playerClickDirection == Vector3Int.right)
                    isPickingRight = true;
                else if (playerClickDirection == Vector3Int.left)
                    isPickingLeft = true;
                else if (playerClickDirection == Vector3Int.up)
                    isPickingUp = true;
                else if (playerClickDirection == Vector3Int.down)
                    isPickingDown = true;
                break;
            case ItemType.Chopping_tool:
            case ItemType.Breaking_tool:
                if (playerClickDirection == Vector3Int.right)
                    isUsingToolRight = true;
                else if (playerClickDirection == Vector3Int.left)
                    isUsingToolLeft = true;
                else if (playerClickDirection == Vector3Int.up)
                    isUsingToolUp = true;
                else if (playerClickDirection == Vector3Int.down)
                    isUsingToolDown = true;
                break;

            default:
                break;
        }
        Crop crop = GridPropertiesManager.Instance.GetCropObjectAtGridLocation(gridPropertyDetails);

        if (crop != null)
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Collection_tool:
                    crop.ProcessToolAction(itemDetails, isPickingRight, isPickingLeft, isPickingDown, isPickingUp);
                    break;
                case ItemType.Chopping_tool:
                case ItemType.Breaking_tool:
                    crop.ProcessToolAction(itemDetails, isUsingToolRight, isUsingToolLeft, isUsingToolDown, isUsingToolUp);
                    break;
            }
        }
    }

    private void UseToolInPlayerDirection(ItemDetails itemDetails, Vector3Int playerClickDirection)
    {
        if (Input.GetMouseButton(0))
        {
            switch (itemDetails.itemType)
            {
                case ItemType.Reaping_tool:
                    if (playerClickDirection == Vector3Int.right)
                    {
                        isSwingingToolRight = true;
                    }
                    else if (playerClickDirection == Vector3Int.left)
                    {
                        isSwingingToolLeft = true;
                    }
                    else if (playerClickDirection == Vector3Int.up)
                    {
                        isSwingingToolUp = true;
                    }
                    else if (playerClickDirection == Vector3Int.down)
                    {
                        isSwingingToolDown = true;
                    }
                    break;
            }

            Vector2 point = new Vector2(GetPlayerCenterPosition().x + (playerClickDirection.x * (itemDetails.itemUseRadius / 2f)),
                GetPlayerCenterPosition().y + playerClickDirection.y * (itemDetails.itemUseRadius / 2f));
            Vector2 size = new Vector2(itemDetails.itemUseRadius, itemDetails.itemUseRadius);
            Item[] itemArray = HelperMethods.GetComponentsAtBoxLocationNonAlloc<Item>(Settings.maxCollidersToTestPerReapSwing
                , point, size, 0f);
            int reapableItemCount = 0;
            for (int i = itemArray.Length - 1; i >= 0; i--)
            {
                if (itemArray[i] != null)
                {
                    if (InventoryManager.Instance.GetItemDetailsByID(itemArray[i].ItemID).itemType == ItemType.Reapable_scenary)
                    {
                        //显示收割特效
                        Vector3 effectPosition = new Vector3(itemArray[i].transform.position.x, itemArray[i].transform.position.y
                            + Settings.gridCellSize / 2f, itemArray[i].transform.position.z);
                        EventHandler.CallHarvestActionEffectEvent(effectPosition, HarvestActionEffect.reaping);

                        Destroy(itemArray[i].gameObject);
                        reapableItemCount++;
                        if (reapableItemCount >= Settings.maxTargetComponentsToDestoryPerReapSwing)
                            break;
                    }
                }
            }
        }

    }

    private void ProcessPlayerClickInputCommodity(ItemDetails itemDetails)
    {
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();

        }
    }

    private void ProcessPlayerClickInputSeed(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {

        //检查种子是否可以种植
        if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid && gridPropertyDetails.daysSinceDug > -1 && gridPropertyDetails.seedItemID == -1)
        {
            PlantSeetAtCursor(gridPropertyDetails, itemDetails);
        }
        else if (itemDetails.canBeDropped && gridCursor.CursorPositionIsValid)
        {
            EventHandler.CallDropSelectedItemEvent();
        }
    }

    private void PlantSeetAtCursor(GridPropertyDetails gridPropertyDetails, ItemDetails itemDetails)
    {
        gridPropertyDetails.seedItemID = itemDetails.itemID;
        gridPropertyDetails.growthDays = 0;

        GridPropertiesManager.Instance.DisplayPlantedCrop(gridPropertyDetails);

        EventHandler.CallRemoveSelectealtemFromlnventoryEvent();
    }

    private void PlayerTestInput()
    {
        if (Input.GetKey(KeyCode.T))
        {
            TimeManager.Instance.TestAdvanceGameMinute();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TimeManager.Instance.TestAdvanceGameDay();
        }
        //if(Input.GetMouseButtonDown(1))
        //{
        //    Vector3 pos = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,-mainCamera.transform.position.z));
        //    PoolManager.Instance.ReuseObject(perfab, pos,Quaternion.identity).SetActive(true);
        //}
    }
    public Vector3 GetPlayerViewportPosition()
    {
        return mainCamera.WorldToViewportPoint(transform.position);
    }

    public void EnablePlayerInput()
    {
        playerInputIsDisabled = false;
    }
    public void DisablePlayerInput()
    {
        playerInputIsDisabled = true;
        ResetAnimationTriggers();
        isWalking = false;
        isRunning = false;
        isIdle = true;

        EventHandler.CallMovementEvent(xInput, yInput, isWalking, isRunning, isIdle, isCarrying, toolEffect,
              isUsingToolRight, isUsingToolLeft, isUsingToolUp, isUsingToolDown,
              isLiftingToolRight, isLiftingToolLeft, isLiftingToolUp, isLiftingToolDown,
              isPickingRight, isPickingLeft, isPickingUp, isPickingDown,
              isSwingingToolRight, isSwingingToolLeft, isSwingingToolUp, isSwingingToolDown,
              idleUp, idleDown, idelLeft, idleRight
              );


    }
    public void SetPlayerPos(Vector3 pos)
    {
        transform.position = pos;
    }
    private void ResetMovenment()
    {
        xInput = 0;
        yInput = 0;
        isRunning = false;
        isWalking = false;
        isIdle = true;
    }

    public Vector3 GetPlayerCenterPosition()
    {
        return new Vector3(transform.position.x, transform.position.y + Settings.playerCenterYOffset, transform.position.z);
    }
    private void SetPlayerDirection(PlayerDirection dir)
    {
        switch (dir)
        {
            case PlayerDirection.Up:
                EventHandler.CallMovementEvent(0,0,false,false,false,false,ToolEffect.None,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,true,false,false,false);
                break;
            case PlayerDirection.Down:
                EventHandler.CallMovementEvent(0, 0, false, false, false, false, ToolEffect.None, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false, false);
                break;
            case PlayerDirection.Left:
                EventHandler.CallMovementEvent(0, 0, false, false, false, false, ToolEffect.None, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true, false);
                break;
            case PlayerDirection.Right:
                EventHandler.CallMovementEvent(0, 0, false, false, false, false, ToolEffect.None, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true);
                break;
        }
    }

    #region 保存
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
            if (gameObjectSave.sceneData.TryGetValue("Main", out SceneSave sceneSave))
            {
                if (sceneSave.vector3Dictionary != null && sceneSave.vector3Dictionary.TryGetValue("playerPosition", out Vector3Serializable vector3Serializable))
                {
                    transform.position = new Vector3(vector3Serializable.x, vector3Serializable.y, vector3Serializable.z);
                }
                if (sceneSave.stringDictionary != null)
                {
                    if (sceneSave.stringDictionary.TryGetValue("currentScene", out string sceneName))
                    {
                        SceneControllerManager.Instance.GoToNextScene(sceneName, transform.position,true);
                    }
                    if (sceneSave.stringDictionary.TryGetValue("playerDirection", out string direction))
                    {
                        bool playerDir = Enum.TryParse(direction, true, out PlayerDirection directionEnum);
                        if (playerDir)
                        {
                            dir = directionEnum;
                            
                            SetPlayerDirection(dir);
                        }
                    }
                }
            }
        }
    }



    public GameObjectSave ISaveableSave()
    {
        GameObjectSave.sceneData.Remove("Main");
        SceneSave sceneSave = new SceneSave();
        sceneSave.vector3Dictionary = new Dictionary<string, Vector3Serializable>();
        sceneSave.stringDictionary = new Dictionary<string, string>();
        Vector3Serializable vector3Serializable = new Vector3Serializable(transform.position);
        sceneSave.vector3Dictionary.Add("playerPosition", vector3Serializable);
        sceneSave.stringDictionary.Add("currentScene", SceneManager.GetActiveScene().name);
        sceneSave.stringDictionary.Add("playerDirection", dir.ToString());
        GameObjectSave.sceneData.Add("Main", sceneSave);
        return GameObjectSave;
        
    }
    #endregion
}
