using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Vector3Serializable
{
    public float x, y, z;
    public Vector3Serializable(UnityEngine.Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public Vector3Serializable(float x,float y,float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    public Vector3Serializable()
    {
        
    }
}
