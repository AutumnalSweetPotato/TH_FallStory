
using System;
using UnityEngine;

[ExecuteAlways]
public class GenerateGUID:MonoBehaviour
{
    [SerializeField]
    private string gUID = "";

    public string GUID { get => gUID; set => gUID = value; }
    private void Awake()
    {
        if(!Application.IsPlaying(gameObject))
        {
            if(gUID == "")
            {
                gUID = Guid.NewGuid().ToString();
            }
        }
    }
}
