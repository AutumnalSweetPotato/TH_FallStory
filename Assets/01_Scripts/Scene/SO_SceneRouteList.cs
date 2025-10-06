using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "so_SceneRouteList", menuName = "ScriptableObject/SceneRouteList")]
public class SO_SceneRouteList : ScriptableObject
{
    [SerializeField]
    public List<SceneRoute> sceneRouteList; 
}
