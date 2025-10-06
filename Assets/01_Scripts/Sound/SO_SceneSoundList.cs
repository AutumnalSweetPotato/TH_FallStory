using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "so_SceneSoundList", menuName = "ScriptableObject/SceneSoundList")]
public class SO_SceneSoundList : ScriptableObject
{
    [SerializeField] public List<SceneSoundItem> sceneSoundItemList;
}
