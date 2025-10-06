using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_NPCScheduleEventList", menuName = "ScriptableObject/NPCScheduleEventList")]
public class SO_NPCScheduleEventList : ScriptableObject
{
    [SerializeField]public List<NPCScheduleEvent> npcScheduleEventList;
}


