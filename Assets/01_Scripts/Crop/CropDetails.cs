using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CropDetails
{
    [ItemDescription]
    public int seedID;                                          //����ID
    public int[] growthDays;                                    //��������
    
    public GameObject[] growthPrefabs;                          //�����׶�Ԥ����
    public Sprite[] growthSprites;                              //�����׶�ͼƬ
    public Season[] growthSeasons;                              //��������
    public Sprite harvestedSprite;                              //�ջ�ͼƬ
    [ItemDescription]
    public int harvestTransformItemID;                          //�ջ���ƷID
    public bool hideCropBeforeHarvestedAnimation;               //�ջ񶯻�ǰ��������
    public bool disableCropcollidersBeforeHarvestedAnimation;   //�ջ񶯻�ǰ������ײ��

    public bool isHarvestedAnimation;                           //�Ƿ��ջ񶯻�
    public bool isHarvestActionEffect;                          //�Ƿ��ջ���Ч��
    public bool spawnCropProducedAtPlayerPosition;              //���������������λ��
    public HarvestActionEffect harvestActionEffect;             //�ջ���Ч��

    [ItemDescription]
    public int[] harvestToolItemID;                             //�ջ񹤾�ID
    public int[] requiredHarvestActions;                        //�ջ����
    [ItemDescription]
    public int[] cropProducedItemID;                            //�ջ���ID
    public int[] cropProducedMinQuality;                        //��������СƷ��
    public int[] cropProducedMaxQuality;                        //���������Ʒ��
    public int daysToRegrow;                                    //��������

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
