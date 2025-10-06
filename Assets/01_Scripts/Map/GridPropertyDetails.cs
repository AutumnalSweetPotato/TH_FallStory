using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridPropertyDetails 
{
    public int gridX;
    public int gridY;
    public bool isDiggable;
    public bool canDropItem;
    public bool canPlaceFuniture;
    public bool isPath;
    public bool isNPCObstacle;
    public int daysSinceDug = -1;           //���ھ�����������
    public int daysSinceWatered = -1;       //�Խ�ˮ����������
    public int seedItemID = -1;             //����ID
    public int growthDays = -1;             //�ɳ�����
    public int daysSinceLastHarvest = -1;   //���ջ�����������
   public GridPropertyDetails() { }

}
