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
    public int daysSinceDug = -1;           //自挖掘以来的天数
    public int daysSinceWatered = -1;       //自浇水以来的天数
    public int seedItemID = -1;             //种子ID
    public int growthDays = -1;             //成长天数
    public int daysSinceLastHarvest = -1;   //自收获以来的天数
   public GridPropertyDetails() { }

}
