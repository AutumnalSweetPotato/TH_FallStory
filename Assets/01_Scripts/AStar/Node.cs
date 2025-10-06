using System;
using Unity.VisualScripting;
using UnityEngine;


public class Node : IComparable<Node>
{
    public Vector2Int gridPosition;
    public int gCost;                   //起始节点到当前节点的移动成本
    public int hCost;                   //当前节点到目标节点的移动成本
    public bool isObstacle;             //当前节点是否为障碍物
    public int movementPenalty;
    public Node parentNode;

    public Node(Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
        parentNode = null;
    }

    public int FCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);
        if (compare == 0)
            compare = hCost.CompareTo(other.hCost);
        return compare;
    }
}
