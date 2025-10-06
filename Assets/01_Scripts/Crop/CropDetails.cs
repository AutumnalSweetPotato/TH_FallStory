using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    [ItemDescription]
    public int seedID;                                          //种子ID
    public int[] growthDays;                                    //生长天数
    
    public GameObject[] growthPrefabs;                          //生长阶段预制体
    public Sprite[] growthSprites;                              //生长阶段图片
    public Season[] growthSeasons;                              //生长季节
    public Sprite harvestedSprite;                              //收获图片
    [ItemDescription]
    public int harvestTransformItemID;                          //收获物品ID
    public bool hideCropBeforeHarvestedAnimation;               //收获动画前隐藏作物
    public bool disableCropcollidersBeforeHarvestedAnimation;   //收获动画前禁用碰撞器

    public bool isHarvestedAnimation;                           //是否收获动画
    public bool isHarvestActionEffect;                          //是否收获动作效果
    public bool spawnCropProducedAtPlayerPosition;              //生长物生成在玩家位置
    public HarvestActionEffect harvestActionEffect;             //收获动作效果

    [ItemDescription]
    public int[] harvestToolItemID;                             //收获工具ID
    public int[] requiredHarvestActions;                        //收获次数
    [ItemDescription]
    public int[] cropProducedItemID;                            //收获物ID
    public int[] cropProducedMinQuality;                        //生长物最小品质
    public int[] cropProducedMaxQuality;                        //生长物最大品质
    public int daysToRegrow;                                    //重生天数

    public SoundName soundName;
    public bool CanUseToolHarvestCrop(int toolItemID)
    {
        if (RequiredHarvestActionsForTool(toolItemID) == -1)
            return false;
        else
            return true;
    }

    public int RequiredHarvestActionsForTool(int toolItemID)
    {
        for(int i = 0;i < harvestToolItemID.Length;i++)
        {
            if (harvestToolItemID[i] == toolItemID)
            {
                return requiredHarvestActions[i];
            }
        }
        return -1;
    }

}
