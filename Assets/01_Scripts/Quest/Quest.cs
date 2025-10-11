using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public string questName;
    public QuestType questType;
    public QuestStatus questStatus;

    public int expRewards;
    public int goldRewards;

    [Header("Gathering Type Quest")]
    public int requireAmount;
}
