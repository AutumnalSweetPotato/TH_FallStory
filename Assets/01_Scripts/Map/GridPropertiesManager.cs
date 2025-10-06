using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
[RequireComponent(typeof(GenerateGUID))]
public class GridPropertiesManager : Single<GridPropertiesManager>, ISaveable
{
    private Transform cropParentTransform;
    private Tilemap groundDecoration1;
    private Tilemap groundDecoration2;
    private Grid grid;
    private string iSaveableUniqueID;
    private GameObjectSave gameObjectSave;
    private Dictionary<string, GridPropertyDetails> dicGridPropertyDetails;
    private bool isFirstTimeSceneLoaded = true;

    [SerializeField] private SO_GridProperties[] so_gridPropertiesArray;
    [SerializeField] private Tile[] dugGround;                                  //挖掘地块数组
    [SerializeField] private Tile[] waterGround;                                //浇水地块数组
    [SerializeField] private SO_CropDetailsList so_cropDetailsList;                //农作物详情列表


    public string ISaveableUniqueID { get => iSaveableUniqueID; set => iSaveableUniqueID = value; }
    public GameObjectSave GameObjectSave { get => gameObjectSave; set => gameObjectSave = value; }

    protected override void Awake()
    {
        base.Awake();
        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();
    }
    private void Start()
    {
        InitializeGridProperties();
    }
    private void OnEnable()
    {
        ISaveableRegister();
        EventHandler.SceneLoadAfterEvent += AfterSceneLoad;
        EventHandler.AdvanceGameDayEvent += AdvanceDay;
    }



    private void OnDisable()
    {
        ISaveableDeregister();
        EventHandler.SceneLoadAfterEvent -= AfterSceneLoad;
        EventHandler.AdvanceGameDayEvent -= AdvanceDay;
    }


    /// <summary>
    /// 清除挖掘地块显示
    /// </summary>
    private void ClearDisplayGroundDecoration()
    {
        groundDecoration1.ClearAllTiles();
        groundDecoration2.ClearAllTiles();
    }
    /// <summary>
    /// 清除显示的格子属性详情
    /// </summary>
    private void ClearDisplayGridPropertiesDetails()
    {
        ClearDisplayGroundDecoration();
        ClearDisplayAllPlantedCrops();
    }

    /// <summary>
    /// 清除所有种植的农作物的显示
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    private void ClearDisplayAllPlantedCrops()
    {
        Crop[] cropArray;
        cropArray = FindObjectsOfType<Crop>();
        foreach (Crop crop in cropArray)
        {
            Destroy(crop.gameObject);
        }
    }

    /// <summary>
    /// 显示挖掘地块
    /// </summary>
    /// <param name="gridPropertyDetails"></param>
    public void DisplayDugGround(GridPropertyDetails gridPropertyDetails)
    {
        if (gridPropertyDetails.daysSinceDug > -1)
        {
            ConnectDugGround(gridPropertyDetails);
        }
    }
    public void DisplayWaterGround(GridPropertyDetails gridPropertyDetails)
    {
        if (gridPropertyDetails.daysSinceWatered > -1)
        {
            ConnectWaterGround(gridPropertyDetails);
        }
    }



    private void ConnectDugGround(GridPropertyDetails gridPropertyDetails)
    {
        //挖掘一个地块
        Tile dugTile = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), dugTile);

        GridPropertyDetails adjacentGridPropertyDetails;
        #region 重新设置边缘格子
        //右边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            dugTile = SetDugTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), dugTile);
        }
        //上边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            dugTile = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), dugTile);
        }
        //左边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            dugTile = SetDugTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), dugTile);
        }
        //下边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceDug > -1)
        {
            dugTile = SetDugTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration1.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), dugTile);
        }
        #endregion
    }

    private void ConnectWaterGround(GridPropertyDetails gridPropertyDetails)
    {
        //浇水一个地块
        Tile waterTile = SetWaterTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0), waterTile);

        GridPropertyDetails adjacentGridPropertyDetails;
        #region 重新设置边缘格子
        //右边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            waterTile = SetWaterTile(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX + 1, gridPropertyDetails.gridY, 0), waterTile);
        }
        //上边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            waterTile = SetWaterTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY + 1, 0), waterTile);
        }
        //左边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            waterTile = SetWaterTile(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX - 1, gridPropertyDetails.gridY, 0), waterTile);
        }
        //下边
        adjacentGridPropertyDetails = GetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
        if (adjacentGridPropertyDetails != null && adjacentGridPropertyDetails.daysSinceWatered > -1)
        {
            waterTile = SetWaterTile(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1);
            groundDecoration2.SetTile(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY - 1, 0), waterTile);
        }
        #endregion
    }


    /// <summary>
    /// 挖掘地块
    /// </summary>
    /// <param name="gridX"></param>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private Tile SetDugTile(int gridX, int gridY)
    {
        bool upDug = IsGridSquareDug(gridX, gridY + 1);
        bool downDug = IsGridSquareDug(gridX, gridY - 1);
        bool leftDug = IsGridSquareDug(gridX - 1, gridY);
        bool rightDug = IsGridSquareDug(gridX + 1, gridY);
        if (!upDug && !downDug && !rightDug && !leftDug)
            return dugGround[0];
        else if (!upDug && downDug && rightDug && !leftDug)
            return dugGround[1];
        else if (!upDug && downDug && rightDug && leftDug)
            return dugGround[2];
        else if (!upDug && downDug && !rightDug && leftDug)
            return dugGround[3];
        else if (!upDug && downDug && !rightDug && !leftDug)
            return dugGround[4];
        else if (upDug && downDug && rightDug && !leftDug)
            return dugGround[5];
        else if (upDug && downDug && rightDug && leftDug)
            return dugGround[6];
        else if (upDug && downDug && !rightDug && leftDug)
            return dugGround[7];
        else if (upDug && downDug && !rightDug && !leftDug)
            return dugGround[8];
        else if (upDug && !downDug && rightDug && !leftDug)
            return dugGround[9];
        else if (upDug && !downDug && rightDug && leftDug)
            return dugGround[10];
        else if (upDug && !downDug && !rightDug && leftDug)
            return dugGround[11];
        else if (upDug && !downDug && !rightDug && !leftDug)
            return dugGround[12];
        else if (!upDug && !downDug && rightDug && !leftDug)
            return dugGround[13];
        else if (!upDug && !downDug && rightDug && leftDug)
            return dugGround[14];
        else if (!upDug && !downDug && !rightDug && leftDug)
            return dugGround[15];

        return null;
    }

    private Tile SetWaterTile(int gridX, int gridY)
    {
        bool upWater = IsGridSquareWater(gridX, gridY + 1);
        bool downWater = IsGridSquareWater(gridX, gridY - 1);
        bool leftWater = IsGridSquareWater(gridX - 1, gridY);
        bool rightWater = IsGridSquareWater(gridX + 1, gridY);
        if (!upWater && !downWater && !rightWater && !leftWater)
            return waterGround[0];
        else if (!upWater && downWater && rightWater && !leftWater)
            return waterGround[1];
        else if (!upWater && downWater && rightWater && leftWater)
            return waterGround[2];
        else if (!upWater && downWater && !rightWater && leftWater)
            return waterGround[3];
        else if (!upWater && downWater && !rightWater && !leftWater)
            return waterGround[4];
        else if (upWater && downWater && rightWater && !leftWater)
            return waterGround[5];
        else if (upWater && downWater && rightWater && leftWater)
            return waterGround[6];
        else if (upWater && downWater && !rightWater && leftWater)
            return waterGround[7];
        else if (upWater && downWater && !rightWater && !leftWater)
            return waterGround[8];
        else if (upWater && !downWater && rightWater && !leftWater)
            return waterGround[9];
        else if (upWater && !downWater && rightWater && leftWater)
            return waterGround[10];
        else if (upWater && !downWater && !rightWater && leftWater)
            return waterGround[11];
        else if (upWater && !downWater && !rightWater && !leftWater)
            return waterGround[12];
        else if (!upWater && !downWater && rightWater && !leftWater)
            return waterGround[13];
        else if (!upWater && !downWater && rightWater && leftWater)
            return waterGround[14];
        else if (!upWater && !downWater && !rightWater && leftWater)
            return waterGround[15];

        return null;
    }
    /// <summary>
    /// 检查地块是否被挖掘
    /// </summary>
    /// <param name="v"></param>
    /// <param name="gridY"></param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private bool IsGridSquareDug(int gridX, int gridY)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(gridX, gridY);
        if (gridPropertyDetails == null)
            return false;
        else if (gridPropertyDetails.daysSinceDug > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsGridSquareWater(int gridX, int gridY)
    {
        GridPropertyDetails gridPropertyDetails = GetGridPropertyDetails(gridX, gridY);
        if (gridPropertyDetails == null)
            return false;
        else if (gridPropertyDetails.daysSinceWatered > -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 显示场景的格子属性
    /// </summary>
    private void DisplayGridPropertiesDetails()
    {
        foreach (KeyValuePair<string, GridPropertyDetails> item in dicGridPropertyDetails)
        {
            GridPropertyDetails propertyDetails = item.Value;
            DisplayDugGround(propertyDetails);
            DisplayWaterGround(propertyDetails);
            DisplayPlantedCrop(propertyDetails);
        }
    }

    public void DisplayPlantedCrop(GridPropertyDetails gridPropertyDetails)
    {
        
        if (gridPropertyDetails.seedItemID > -1)
        {
            CropDetails cropDetails = so_cropDetailsList.GetCropDetails(gridPropertyDetails.seedItemID);
            GameObject cropPrefab;

            int growthStages = cropDetails.growthDays.Length;

            int currentGrowthStage = 0;
            
            for (int i = growthStages - 1; i >= 0; i--)
            {
                
                if (gridPropertyDetails.growthDays >= cropDetails.growthDays[i])
                {
                    currentGrowthStage = i;
                    break;
                }

            }
            
            cropPrefab = cropDetails.growthPrefabs[currentGrowthStage];
            Sprite growthSprite = cropDetails.growthSprites[currentGrowthStage];
            Vector3 worldPosition = groundDecoration2.CellToWorld(new Vector3Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY, 0));
            worldPosition = new Vector3(worldPosition.x + Settings.gridCellSize / 2, worldPosition.y, worldPosition.z);
            GameObject cropInstance = Instantiate(cropPrefab, worldPosition, Quaternion.identity);
            cropInstance.GetComponentInChildren<SpriteRenderer>().sprite = growthSprite;
            cropInstance.transform.SetParent(cropParentTransform);
            cropInstance.GetComponent<Crop>().cropGridPosition = new Vector2Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY);
        }
    }


    /// <summary>
    /// 移除保存数据
    /// </summary>
    public void ISaveableDeregister()
    {
        if (SaveLoadManager.Instance != null)
            SaveLoadManager.Instance.iSaveableObjectList.Remove(this);
    }

    /// <summary>
    /// 添加保存数据
    /// </summary>
    public void ISaveableRegister()
    {
        SaveLoadManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableLoad(GameSave gameSave)
    { 
        if(gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            GameObjectSave = gameObjectSave;
            ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
    public GameObjectSave ISaveableSave()
    {
        ISaveableStoreScene(SceneManager.GetActiveScene().name);
        return GameObjectSave;
    }
    /// <summary>
    /// 恢复场景数据
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableRestoreScene(string sceneName)
    {
        if (GameObjectSave.sceneData.TryGetValue(sceneName, out SceneSave sceneSave))
        {
            if (sceneSave.dictGridPropertyDetails != null)
            {
                dicGridPropertyDetails = sceneSave.dictGridPropertyDetails;
            }

            if(sceneSave.boolDictionary != null && sceneSave.boolDictionary.TryGetValue("isFirstTimeSceneLoaded",out bool storedIsFirstTimeSceneLoaded))
            {
                isFirstTimeSceneLoaded = storedIsFirstTimeSceneLoaded;
            }
            if (isFirstTimeSceneLoaded)
                EventHandler.CallInstantiateCropPrefabsEvent();

            if (dicGridPropertyDetails.Count > 0)
            {
                ClearDisplayGridPropertiesDetails();
                DisplayGridPropertiesDetails();
            }

            if(isFirstTimeSceneLoaded)
                isFirstTimeSceneLoaded = false;
        }
    }
    /// <summary>
    /// 保存场景数据
    /// </summary>
    /// <param name="sceneName"></param>
    public void ISaveableStoreScene(string sceneName)
    {
        GameObjectSave.sceneData.Remove(sceneName);
        SceneSave sceneSave = new SceneSave();
        sceneSave.dictGridPropertyDetails = dicGridPropertyDetails;

        sceneSave.boolDictionary = new Dictionary<string, bool>();
        sceneSave.boolDictionary.Add("isFirstTimeSceneLoaded", isFirstTimeSceneLoaded);

        GameObjectSave.sceneData.Add(sceneName, sceneSave);
    }



    private void AfterSceneLoad()
    {
        grid = FindObjectOfType<Grid>();
        groundDecoration1 = GameObject.FindGameObjectWithTag(Settings.Tags.GroundDecoration1).GetComponent<Tilemap>();
        groundDecoration2 = GameObject.FindGameObjectWithTag(Settings.Tags.GroundDecoration2).GetComponent<Tilemap>();
        cropParentTransform = GameObject.FindGameObjectWithTag(Settings.Tags.CropsParentTransform).transform;
    }
    private void AdvanceDay(int gameYear, Season gameSeason, int gameDay, string gameDayOfWeek,
        int gameHour, int gameMinute, int gameSecond)
    {
        ClearDisplayGridPropertiesDetails();
        foreach (SO_GridProperties so_GridProperties in so_gridPropertiesArray)
        {
            if (GameObjectSave.sceneData.TryGetValue(so_GridProperties.sceneName.ToString(), out SceneSave sceneSave))
            {
                if (sceneSave.dictGridPropertyDetails != null)
                {
                    for (int i = sceneSave.dictGridPropertyDetails.Count - 1; i >= 0; i--)
                    {
                        KeyValuePair<string, GridPropertyDetails> item = sceneSave.dictGridPropertyDetails.ElementAt(i);
                        GridPropertyDetails gridPropertyDetails = item.Value;
                        if (gridPropertyDetails.daysSinceWatered > -1)
                        {
                            gridPropertyDetails.daysSinceWatered = -1;
                        }
                        if (gridPropertyDetails.growthDays >-1 )
                        {
                            gridPropertyDetails.growthDays += 1;
                        }
                        SetGridPropertyDetails(gridPropertyDetails.gridX, gridPropertyDetails.gridY, gridPropertyDetails, sceneSave.dictGridPropertyDetails);
                    }
                }
            }
        }
        DisplayGridPropertiesDetails();
    }
    /// <summary>
    /// 初始化网格属性
    /// </summary>
    private void InitializeGridProperties()
    {
        foreach (var so_gridProperties in so_gridPropertiesArray)
        {
            Dictionary<string, GridPropertyDetails> dicGridPropertyDetails = new Dictionary<string, GridPropertyDetails>();
            foreach (var gridProperty in so_gridProperties.gridPropertyList)
            {
                GridPropertyDetails gridPropertyDetails;
                gridPropertyDetails = GetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y,
                    dicGridPropertyDetails);

                if (gridPropertyDetails == null)
                {
                    gridPropertyDetails = new GridPropertyDetails();
                }
                switch (gridProperty.gridBoolProperty)
                {
                    case GridBoolProperty.diggable:
                        gridPropertyDetails.isDiggable = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.canDropItem:
                        gridPropertyDetails.canDropItem = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.canPlaceFuniture:
                        gridPropertyDetails.canPlaceFuniture = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.isPath:
                        gridPropertyDetails.isPath = gridProperty.gridBoolValue;
                        break;
                    case GridBoolProperty.isNPCObstacle:
                        gridPropertyDetails.isNPCObstacle = gridProperty.gridBoolValue;
                        break;

                }
                SetGridPropertyDetails(gridProperty.gridCoordinate.x, gridProperty.gridCoordinate.y,
                    gridPropertyDetails, dicGridPropertyDetails);
            }
            SceneSave sceneSave = new SceneSave();
            sceneSave.dictGridPropertyDetails = dicGridPropertyDetails;
            if (so_gridProperties.sceneName.ToString() == SceneControllerManager.Instance.StartSceneName)
            {
                this.dicGridPropertyDetails = dicGridPropertyDetails;
            }
            sceneSave.boolDictionary = new Dictionary<string, bool>();
            sceneSave.boolDictionary.Add("isFirstTimeSceneLoaded", true);
            

            GameObjectSave.sceneData.Add(so_gridProperties.sceneName.ToString(), sceneSave);
        }
    }
    /// <summary>
    /// 设置网格属性详情
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="gridPropertyDetails"></param>
    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails gridPropertyDetails)
    {
        string key = "x" + x + "y" + y;
        gridPropertyDetails.gridX = x;
        gridPropertyDetails.gridY = y;
        dicGridPropertyDetails[key] = gridPropertyDetails;
    }
    public void SetGridPropertyDetails(int x, int y, GridPropertyDetails gridPropertyDetails, Dictionary<string, GridPropertyDetails> dicGridPropertyDetails)
    {
        string key = "x" + x + "y" + y;
        gridPropertyDetails.gridX = x;
        gridPropertyDetails.gridY = y;
        dicGridPropertyDetails[key] = gridPropertyDetails;
    }

    /// <summary>
    /// 获取网格属性详情
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="dicGridPropertyDetails"></param>
    /// <returns></returns>
    public GridPropertyDetails GetGridPropertyDetails(int x, int y, Dictionary<string, GridPropertyDetails> dicGridPropertyDetails)
    {
        string key = "x" + x + "y" + y;
        GridPropertyDetails gridPropertyDetails;
        if (!dicGridPropertyDetails.TryGetValue(key, out gridPropertyDetails))
        {
            return null;
        }
        else
            return gridPropertyDetails;
    }
    
    public GridPropertyDetails GetGridPropertyDetails(int x, int y)
    {
        return GetGridPropertyDetails(x, y, dicGridPropertyDetails);
    }
    
    public GridPropertyDetails GetGridPropertyDetails(Vector3Int vector3Int)
    {
        return GetGridPropertyDetails(vector3Int.x, vector3Int.y, dicGridPropertyDetails);
    }

    public Crop GetCropObjectAtGridLocation(GridPropertyDetails gridPropertyDetails)
    {
        Vector3 worldPosition = grid.GetCellCenterWorld(new Vector3Int(gridPropertyDetails.gridX,gridPropertyDetails.gridY,0));
        Collider2D[] collider2DArray = Physics2D.OverlapPointAll(worldPosition);
        Crop crop;
        for(int i = 0; i < collider2DArray.Length; i++)
        {
            crop = collider2DArray[i].gameObject.GetComponentInParent<Crop>();
            if (crop != null && crop.cropGridPosition == new Vector2Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY))
            {
                return crop;
            }
            crop = collider2DArray[i].gameObject.GetComponentInChildren<Crop>();
            if (crop != null && crop.cropGridPosition == new Vector2Int(gridPropertyDetails.gridX, gridPropertyDetails.gridY))
            {
                return crop;
            }
        }
        return null;
    }
    public CropDetails GetCropDetails(int seedID)
    {
        return so_cropDetailsList.GetCropDetails(seedID);
    }

    /// <summary>
    /// 获取场景的网格尺寸和原点
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="gridDimensions"></param>
    /// <param name="gridOrigin"></param>
    /// <returns></returns>
    public bool GetGridDimensions(SceneName sceneName,out Vector2Int gridDimensions,out Vector2Int gridOrigin)
    {
        gridDimensions = Vector2Int.zero;
        gridOrigin = Vector2Int.zero;
        foreach (SO_GridProperties so_gridProperties in so_gridPropertiesArray)
        {
            if(so_gridProperties.sceneName == sceneName)
            {
                gridDimensions = new Vector2Int(so_gridProperties.gridWidth, so_gridProperties.gridHeight);
                gridOrigin = new Vector2Int(so_gridProperties.originX, so_gridProperties.originY);
                return true;
            }
        }

        return false;
    }

    
}
