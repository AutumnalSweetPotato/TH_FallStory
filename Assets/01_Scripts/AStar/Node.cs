using System;
using Unity.VisualScripting;
using UnityEngine;


public class Node : IComparable<Node>
{
    public Vector2Int gridPosition;
    public int gCost;                   //��ʼ�ڵ㵽��ǰ�ڵ���ƶ��ɱ�
    public int hCost;                   //��ǰ�ڵ㵽Ŀ��ڵ���ƶ��ɱ�
    public bool isObstacle;             //��ǰ�ڵ��Ƿ�Ϊ�ϰ���
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
